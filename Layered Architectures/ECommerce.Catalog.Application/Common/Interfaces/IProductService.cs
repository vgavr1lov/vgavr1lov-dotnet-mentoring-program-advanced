using ECommerce.Catalog.Domain.Entities;

namespace ECommerce.Catalog.Application.Common.Interfaces
{
    public interface IProductService
    {
        Task<long> AddProduct(Product product, CancellationToken cancellationToken);
        Task DeleteProduct(long id, CancellationToken cancellationToken);
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken);
        Task<Product?> GetProduct(long id, CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
    }
}