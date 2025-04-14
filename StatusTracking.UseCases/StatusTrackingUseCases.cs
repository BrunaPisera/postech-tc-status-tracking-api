using StatusTracking.Core.Entities;
using StatusTracking.Core.Entities.Enums;
using StatusTracking.UseCases.DTOs;
using StatusTracking.UseCases.Exceptions;
using StatusTracking.UseCases.Gateway;
using StatusTracking.UseCases.Interfaces;

namespace StatusTracking.UseCases
{       
    public class StatusTrackingUseCases : IStatusTrackingUseCases
    {
        private readonly IStatusTrackingPersistenceGateway StatusTrackingPersistenceGateway;

        public StatusTrackingUseCases (IStatusTrackingPersistenceGateway statusTrackingPersistenceGateway)
        {
            StatusTrackingPersistenceGateway = statusTrackingPersistenceGateway;
        }
        public async Task AtualizaStatusComoInProcessAsync(string videoId)
        {
            var video =  await TryGetVideoById(videoId);

            if (video.Status != Status.Uploaded)
                throw new OperacaoInvalidaException("Status de processamento do video precisa estar como uploaded para ser atualizado como InProcess.");

            video.Status = Status.InProcess;
        
            await TryToSaveStatus(video);
        }

        public async Task AtualizaStatusComoReadyAsync(VideoProcessedMessageDto videoDto)
        {
            var video = await TryGetVideoById(videoDto.VideoKey);

            if (video.Status != Status.InProcess)
                throw new OperacaoInvalidaException("Status de processamento do video precisa estar como InProcess para ser atualizado como Ready.");

            video.Status = Status.Ready;
            video.VideoImagesZipFileUrl = videoDto.FilesURL;

            await TryToSaveStatus(video);
        }

        public async Task SaveUploadedVideoAsync(string videoId)
        {        
            var video = new StatusTrackingAggregate()
            {
                VideoId = videoId,
                Status = Status.Uploaded
            };

            await TryToSaveStatus(video);
        }

        public async Task AtualizaStatusComoErrorAsync(string videoId)
        {
            var video = new StatusTrackingAggregate()
            {
                VideoId = videoId,
                Status = Status.Error,
                Errors = "Could not process the video properly."
            };

            await TryToSaveStatus(video);
        }

        private async Task<StatusTrackingAggregate?> TryGetVideoById(string videoId)
        {
            var pedido = await StatusTrackingPersistenceGateway.GetVideoByIdAsync(videoId);

            return pedido;
        }

        private async Task TryToSaveStatus(StatusTrackingAggregate? status)
        {
            var statusAtualizado = await StatusTrackingPersistenceGateway.SaveStatusAsync(status);

            if (!statusAtualizado) throw new OperacaoInvalidaException("Nao foi possivel atualizar o status do video.");
        }

        public async Task<StatusTrackingAggregate?> GetVideoByIdAsync(string videoId)
        {
            return await TryGetVideoById(videoId);
        }
    }
}
