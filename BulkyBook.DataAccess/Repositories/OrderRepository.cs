using BulkyBook.Core.Models;
using BulkyBook.Core.Repositories;

namespace BulkyBook.DataAccess.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public void UpdateStatus(int id, string status, string? paymentStatus = null)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == id);

            if (order != null)
            {
                order.Status = status;

                if (paymentStatus != null)
                    order.PaymentStatus = paymentStatus;
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == id);

            if (order != null)
            {
                order.SessionId = sessionId;
                order.PaymentIntentId = paymentIntentId;
            }
        }
    }
}
