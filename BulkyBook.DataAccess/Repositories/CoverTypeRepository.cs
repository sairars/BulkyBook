using BulkyBook.Core.Models;
using BulkyBook.Core.Repositories;
using BulkyBookWeb.Data;

namespace BulkyBook.DataAccess.Repositories
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public CoverTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CoverType coverType)
        {
            _context.Update(coverType);
        }
    }
}
