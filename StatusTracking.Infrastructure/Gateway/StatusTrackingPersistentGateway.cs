using Microsoft.EntityFrameworkCore;
using StatusTracking.Core.Entities;
using StatusTracking.Infrastructure.Data;
using StatusTracking.UseCases.Gateway;

namespace StatusTracking.Infrastructure.Gateway
{
    internal class StatusTrackingPersistentGateway : IStatusTrackingPersistenceGateway
    {
        private ApplicationContext Context;

        public StatusTrackingPersistentGateway(ApplicationContext context)
        {
            Context = context;
        }

        public async Task<StatusTrackingAggregate?> GetVideoByIdAsync(string videoId)
        {
            return await Context.StatusTracking
                        .FirstOrDefaultAsync(x => x.VideoId == videoId);
        }

        public async Task<bool> SaveStatusAsync(StatusTrackingAggregate aggregate)
        {
            Context.StatusTracking.Update(aggregate);

            var result = await Context.SaveChangesAsync();

            return result > 0;
        }
    }
}