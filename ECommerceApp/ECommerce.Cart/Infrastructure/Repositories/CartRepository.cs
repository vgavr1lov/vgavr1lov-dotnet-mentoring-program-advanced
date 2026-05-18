using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Domain.Entities;
using LiteDB;

namespace ECommerce.Cart.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private const string ShoppingCartLiteDb = "ShoppingCartLiteDb/cart.db";
    private static string CartsCollection = "carts";
    private readonly string? _dbPath;

    public CartRepository(string? dbPath = ShoppingCartLiteDb)
    {
        _dbPath = dbPath;

        var directory = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);
    }

    public async Task<ShoppingCart?> GetCartByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase($"Filename={_dbPath}");
            var col = db.GetCollection<ShoppingCart>(CartsCollection);

            cancellationToken.ThrowIfCancellationRequested();

            return col.Query()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }, cancellationToken);
    }

    public async Task SaveCartAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase($"Filename={_dbPath}");
            var col = db.GetCollection<ShoppingCart>(CartsCollection);

            cancellationToken.ThrowIfCancellationRequested();

            col.Upsert(cart);
        }, cancellationToken);
    }

    public async Task SaveCartsAsync(List<ShoppingCart> carts, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase($"Filename={_dbPath}");
            var col = db.GetCollection<ShoppingCart>(CartsCollection);

            cancellationToken.ThrowIfCancellationRequested();

            col.Upsert(carts);
        }, cancellationToken);
    }

    public async Task<List<ShoppingCart>> GetCartsByItemId(long id, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase($"Filename={_dbPath}");
            var col = db.GetCollection<ShoppingCart>(CartsCollection);

            cancellationToken.ThrowIfCancellationRequested();

            return col.FindAll()
                .Where(cart => cart.Items.Any(item => item.Id == id))
                .ToList();
        }, cancellationToken);
    }
}
