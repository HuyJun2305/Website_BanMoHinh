using Data.Models;

namespace API.IRepositories
{
	 public interface ICategoryRepo
	{
		Task<List<Category>> GetAllCategories();
		Task<Category> GetCategoryById(Guid id);
		Task Create(Category category);
		Task Update(Category category);
		Task Delete(Guid id);
		Task SaveChanges();

	}
}
