using StatusTracking.Core.Entities.Enums;

namespace StatusTracking.Core.Entities
{
    public class StatusTrackingAggregate : Entity<Guid>, IAggregateRoot
    {
        public string VideoId  { get; set; }
        public string? VideoImagesZipFileUrl { get; set; }
        public Status Status { get; set; }
        public string? Errors { get; set; }
    }
}