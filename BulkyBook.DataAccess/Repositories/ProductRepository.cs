using BulkyBook.Core.Models;
using BulkyBook.Core.Repositories;
using BulkyBookWeb.Data;


namespace BulkyBook.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var productFromDb = _context.Products.SingleOrDefault(p => p.Id == product.Id);

            if (productFromDb != null) 
            {
                productFromDb.ISBN = product.ISBN;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Price = product.Price;
                productFromDb.Price100 = product.Price100;  
                productFromDb.Price50 = product.Price50;
                productFromDb.Author = product.Author;
                productFromDb.Description = product.Description;
                productFromDb.CategoryId = product.CategoryId;
                productFromDb.CoverTypeId = product.CoverTypeId;
                productFromDb.Title = product.Title;

                if (product.ImageUrl != null) 
                    productFromDb.ImageUrl = product.ImageUrl;
            }
        }
    }
}
