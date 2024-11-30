using Data.Models;

namespace View.IServices
{
	public interface ICategoryServices
	{
		Task<IEnumerable<Category>> GetAllCategories();
		Task<Category> GetCategoryById(Guid id);
		Task Create(Category category);
		Task Update(Category category);
		Task Delete(Guid id);
	}
}
