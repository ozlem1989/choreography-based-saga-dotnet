using System;

namespace PaymentService.Infra.Models
{
    public class OutboxMessage
    {

        public OutboxMessage()
        {
            EventDate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string EventType { get; set; }
        public string EventPayload { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentDate { get; set; }

    }
}
