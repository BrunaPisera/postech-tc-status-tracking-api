namespace StatusTracking.UseCases.Interfaces
{
    public interface IStatusTrackingUseCases
    {
        Task AtualizaStatusComoUploadedAsync(string videoId);
        Task AtualizaStatusComoInProcessAsync(string videoId);
        Task AtualizaStatusComoReadyAsync(string videoId);    
    }
}