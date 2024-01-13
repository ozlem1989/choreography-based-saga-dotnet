using Newtonsoft.Json;
using PaymentService.Infra;
using PaymentService.Infra.Models;
using Shared.Contracts;
using System.Threading.Tasks;

namespace PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _appDbContext;
        public PaymentService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task DoPaymentAsync(int orderId, int walletId, int userId, decimal totalAmount)
        {
            // after payment operation is done...

            var isPaid = true;

            var payment = new Payment
            {
                OrderId = orderId,
                WalletId = walletId,
                UserId = userId,
                TotalAmount = totalAmount,
                IsPaid = isPaid
            };

            await _appDbContext.Payments.AddAsync(payment);


            object paymentResultEvent;

            if (isPaid)
            {
                paymentResultEvent = new PaymentCompletedEvent
                {
                    OrderId = orderId
                }; 
            }
            else
            {
                paymentResultEvent = new PaymentRejectedEvent
                {
                    OrderId = orderId,
                    Reason = ""
                }; 
            }


            var outboxMessage = new OutboxMessage
            {
                EventPayload = JsonConvert.SerializeObject(paymentResultEvent),
                EventType = paymentResultEvent.GetType().AssemblyQualifiedName
            };

            await _appDbContext.OutboxMessages.AddAsync(outboxMessage); 

            await _appDbContext.SaveChangesAsync();
        }
    }
}