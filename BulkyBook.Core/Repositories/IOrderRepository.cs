using BulkyBook.Core.Models;

namespace BulkyBook.Core.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order orderDb);
        void UpdateStatus(int id, string status, string? paymentStatus = null);
        void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
    }
}
