using StatusTracking.Core.Entities;
using StatusTracking.UseCases.DTOs;

namespace StatusTracking.UseCases.Interfaces
{
    public interface IStatusTrackingUseCases
    {
        Task SaveUploadedVideoAsync(string videoId);
        Task AtualizaStatusComoInProcessAsync(string videoId);
        Task AtualizaStatusComoReadyAsync(VideoProcessedMessageDto videoDto);    
        Task AtualizaStatusComoErrorAsync(string videoId);    
        Task<StatusTrackingAggregate?>GetVideoByIdAsync(string videoId);    
    }
}