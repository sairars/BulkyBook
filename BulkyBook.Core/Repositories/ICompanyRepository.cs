using BulkyBook.Core.Models;

namespace BulkyBook.Core.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company company);
    }
}
