using BulkyBook.Core.Models;

namespace BulkyBook.Core.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
