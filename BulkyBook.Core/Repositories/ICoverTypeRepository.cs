using BulkyBook.Core.Models;

namespace BulkyBook.Core.Repositories
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}
