using ProcessService.Infrastructure.Broker;
using StatusTracking.Infrastructure;
using StatusTracking.UseCases.DTOs;
using StatusTracking.UseCases.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    services.EnsureDatabaseMigrated();

    var rabbitMqConnection = new BrokerConnection();
    var rabbitMqConsumer = new BrokerConsumer(rabbitMqConnection);

    rabbitMqConsumer.BrokerStartConsumer<VideoProcessedMessageDto>(
        queueName: "videoStatus.Ready",
        exchange: "videoOperations",
        routingKey: "video.processed",
        callback: async (videoProcessedMessage) => {
            using (var innerScope = app.Services.CreateScope())
            {
                var scopedServices = innerScope.ServiceProvider;
                var statusTrackingUseCases = scopedServices.GetRequiredService<IStatusTrackingUseCases>();
                await statusTrackingUseCases.AtualizaStatusComoReadyAsync(videoProcessedMessage);
            }
        });

    rabbitMqConsumer.BrokerStartConsumer(
        queueName: "videoStatus.Upload",
        exchange: "videoOperations",
        routingKey: "video.uploaded",
        callback: async (videoId) => {
            using (var innerScope = app.Services.CreateScope())
            {
                var scopedServices = innerScope.ServiceProvider;
                var statusTrackingUseCases = scopedServices.GetRequiredService<IStatusTrackingUseCases>();
                await statusTrackingUseCases.SaveUploadedVideoAsync(videoId);
            }
        });

    rabbitMqConsumer.BrokerStartConsumer(
        queueName: "videoStatus.Process",
        exchange: "videoOperations",
        routingKey: "video.inprocess",
        callback: async (videoId) => {
            using (var innerScope = app.Services.CreateScope())
            {
                var scopedServices = innerScope.ServiceProvider;
                var statusTrackingUseCases = scopedServices.GetRequiredService<IStatusTrackingUseCases>();
                await statusTrackingUseCases.AtualizaStatusComoInProcessAsync(videoId);
            }
        });

    rabbitMqConsumer.BrokerStartConsumer(
        queueName: "videoStatus.Error",
        exchange: "videoOperations",
        routingKey: "video.error",
        callback: async (videoId) => {
            using (var innerScope = app.Services.CreateScope())
            {
                var scopedServices = innerScope.ServiceProvider;
                var statusTrackingUseCases = scopedServices.GetRequiredService<IStatusTrackingUseCases>();
                await statusTrackingUseCases.AtualizaStatusComoErrorAsync(videoId);
            }
        });
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();