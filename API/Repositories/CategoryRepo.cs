using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
	public class CategoryRepo : ICategoryRepo
	{
		private readonly ApplicationDbContext _context;

		public CategoryRepo(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Create(Category category)
		{
			if (await GetCategoryById(category.Id) != null) throw new DuplicateWaitObjectException($"Category : {category.Id} is existed!");
			await _context.Categories.AddAsync(category);
		}

		public async Task Delete(Guid id)
		{
			var data = await GetCategoryById(id);
			if (data == null) throw new KeyNotFoundException("Not found this brand!");
			if (_context.Products.Where(p => p.BrandId == data.Id).Any()) throw new Exception("This brand has used for some product!");
			_context.Categories.Remove(data);
		}

		public async Task<List<Category>> GetAllCategories()
		{
			return await _context.Categories.ToListAsync();
		}

		public async Task<Category> GetCategoryById(Guid id)
		{
			return await _context.Categories.FindAsync(id);
		}

		public async Task SaveChanges()
		{
			await _context.SaveChangesAsync();
		}

		public async Task Update(Category category)
		{
			if (await GetCategoryById(category.Id) == null) throw new KeyNotFoundException("Not found this brand!");
			_context.Entry(category).State = EntityState.Modified;
		}
	}
}
