using BulkyBook.Core.Models;

namespace BulkyBook.Core.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
        void UpdateStatus(int id, string status, string? paymentStatus = null);
        void UpdateStripeSessionId(int id, string sessionId);
        void UpdateStripePaymentId(int id, string paymentIntentId);
    }
}
