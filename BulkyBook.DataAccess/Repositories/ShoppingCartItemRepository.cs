using BulkyBook.Core.Models;
using BulkyBook.Core.Repositories;

namespace BulkyBook.DataAccess.Repositories
{
    public class ShoppingCartItemRepository : Repository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void ModifyQuantity(ShoppingCartItem shoppingCartItem, int quantity)
        {
            shoppingCartItem.Quantity += quantity;
        }
    }
}
