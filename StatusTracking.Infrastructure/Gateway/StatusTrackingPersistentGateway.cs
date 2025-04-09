using StatusTracking.Core.Entities;
using StatusTracking.Infrastructure.Data;
using StatusTracking.UseCases.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusTracking.Infrastructure.Gateway
{
    public class StatusTrackingPersistentGateway : IStatusTrackingPersistenceGateway
    {
        private ApplicationContext Context;

        public StatusTrackingPersistentGateway(ApplicationContext context)
        {
            Context = context;
        }

        public Task<StatusTrackingAggregate> GetVideoByIdAsync(string videoId)
        {
            return await Context.Stat
                       .FirstOrDefaultAsync(x => x.IdPedido == idPedido);
        }

        public Task<bool> SaveStatusAsync(StatusTrackingAggregate aggregate)
        {
            throw new NotImplementedException();
        }
    }
}
