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

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            CoverTypes = new CoverTypeRepository(_context);
            Products = new ProductRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
