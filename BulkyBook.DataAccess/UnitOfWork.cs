using BulkyBook.Core;
using BulkyBook.Core.Repositories;
using BulkyBook.DataAccess.Repositories;
using BulkyBook.DataAccess;

namespace BulkyBook.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Categories { get; }
        public ICoverTypeRepository CoverTypes { get; }
        public IProductRepository Products { get; set; }
        public ICompanyRepository Companies { get; set; }
        public IShoppingCartItemRepository ShoppingCartItems { get; set; }
        public IOrderRepository Orders { get; set; }
        public IOrderDetailRepository OrderDetails { get; set; }
        public IUserRepository Users { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            CoverTypes = new CoverTypeRepository(_context);
            Products = new ProductRepository(_context);
            Companies = new CompanyRepository(_context);
            ShoppingCartItems = new ShoppingCartItemRepository(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
            Users = new UserRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
