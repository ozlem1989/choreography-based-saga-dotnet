using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using PaymentService.Services;
using Shared.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.Consumers
{
    public class StocksReservedEventConsumer : IConsumeAsync<StocksReservedEvent>
    {
        private readonly IPaymentService _paymentService;

        public StocksReservedEventConsumer(IPaymentService paymentService, IBus bus)
        {
            _paymentService = paymentService;
        }

        public async Task ConsumeAsync(StocksReservedEvent message, CancellationToken cancellationToken = default)
        {
            await _paymentService.DoPaymentAsync(message.OrderId, message.WalletId, message.UserId, message.TotalAmount);
        }
    }
}