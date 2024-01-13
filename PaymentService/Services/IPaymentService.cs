using System;
using System.Threading.Tasks;

namespace PaymentService.Services
{
    public interface IPaymentService
    {
        Task DoPaymentAsync(int orderId, int walletId, int userId, decimal totalAmount);
    }
}