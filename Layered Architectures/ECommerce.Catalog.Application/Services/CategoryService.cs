using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Domain.Entities;

namespace ECommerce.Catalog.Application.Services;

public class CategoryService: ICategoryService
{
    private readonly IApplicationDbContext _context;

    public CategoryService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Category?> GetCategory(long id, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _context.Category
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Category>> GetAllCategories(CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _context.Category
                .AsNoTracking()
                .ToListAsync();

            return categories;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<long> AddCategory(Category category, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Category.AddAsync(category);
            var categoryId = await _context.SaveChangesAsync(cancellationToken);

            return categoryId;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateCategory(Category category, CancellationToken cancellationToken)
    {
        try
        {
            var categoryToUpdate = await _context.Category
                .FirstOrDefaultAsync(c => c.Id == category.Id);

            if (categoryToUpdate is null)
                return;

            _context.Category.Entry(categoryToUpdate).CurrentValues.SetValues(category);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteCategory(long id, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _context.Category
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                return;

            _context.Category.Remove(category);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}