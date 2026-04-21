using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Domain.Entities;

namespace ECommerce.Catalog.Application.Services;

public class ProductService : IProductService
{
    private readonly IApplicationDbContext _context;

    public ProductService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Product?> GetProduct(long id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return product;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Product>> GetAllProducts(CancellationToken cancellationToken)
    {
        try
        {
            var products = await _context.Product
                .AsNoTracking()
                .ToListAsync();

            return products;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<long> AddProduct(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Product.AddAsync(product);
            var productId = await _context.SaveChangesAsync(cancellationToken);

            return productId;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
    {
        try
        {
            var productToUpdate = await _context.Product
                .FirstOrDefaultAsync(c => c.Id == product.Id);

            if (productToUpdate is null)
                return;

            _context.Product.Entry(productToUpdate).CurrentValues.SetValues(product);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteProduct(long id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _context.Product
                .FirstOrDefaultAsync(c => c.Id == id);

            if (product is null)
                return;

            _context.Product.Remove(product);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
