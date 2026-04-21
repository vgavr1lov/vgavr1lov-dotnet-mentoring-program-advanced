using ECommerce.Catalog.Domain.Entities;

namespace ECommerce.Catalog.Application.Common.Interfaces
{
    public interface ICategoryService
    {
        Task<long> AddCategory(Category category, CancellationToken cancellationToken);
        Task DeleteCategory(long id, CancellationToken cancellationToken);
        Task<List<Category>> GetAllCategories(CancellationToken cancellationToken);
        Task<Category?> GetCategory(long id, CancellationToken cancellationToken);
        Task UpdateCategory(Category category, CancellationToken cancellationToken);
    }
}