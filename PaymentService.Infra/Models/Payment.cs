using System;

namespace PaymentService.Infra.Models
{
    public class Payment
    {
        public Payment()
        {
            PaymentDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int WalletId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
