using StatusTracking.Core.Entities;

namespace StatusTracking.UseCases.Gateway
{
    public interface IStatusTrackingPersistenceGateway
    {
        Task<StatusTrackingAggregate> GetVideoByIdAsync(string videoId);
        Task<bool> SaveStatusAsync (StatusTrackingAggregate aggregate);
    }
}
