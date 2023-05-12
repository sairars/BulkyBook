using BulkyBook.Core.Models;

namespace BulkyBook.Core.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        void UpdateStatus(int id, string status, string? paymentStatus = null);
    }
}
