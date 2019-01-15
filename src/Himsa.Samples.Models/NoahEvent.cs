using System;

namespace Himsa.Samples.Models
{
    public class NoahEvent
    {
        public Guid TenantId { get; set; }

        public Guid NotificationEventSubscriptionId { get; set; }

        public Guid NotificationEventId { get; set; }
    }
}
