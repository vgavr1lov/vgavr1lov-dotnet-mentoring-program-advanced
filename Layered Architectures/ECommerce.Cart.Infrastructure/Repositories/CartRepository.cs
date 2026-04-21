using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Domain.Entities;
using LiteDB;

namespace ECommerce.Cart.Infrastructure.Repositories;

public class CartRepository(string dbPath = CartRepository.ShoppingCartLiteDb) : ICartRepository
{
    private const string ShoppingCartLiteDb = "ShoppingCartLiteDb\\";
    private static string CartsCollection = "carts";

    public ShoppingCart GetCartById(long id)
    {
        using (var db = new LiteDatabase($"Filename={dbPath};Connection=shared"))
        {
            var col = db.GetCollection<ShoppingCart>(CartsCollection);

            var results = col.Query()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return results;
        }
    }
    public void SaveCart(ShoppingCart cart)
    {
        using (var db = new LiteDatabase($"Filename={dbPath};Connection=shared"))
        {
            var col = db.GetCollection<ShoppingCart>(CartsCollection);
            col.Upsert(cart);
        }
    }
}
