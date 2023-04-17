using BulkyBook.Core.Models;

namespace BulkyBook.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
