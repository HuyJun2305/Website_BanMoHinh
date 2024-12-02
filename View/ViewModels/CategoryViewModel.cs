using Data.Models;

namespace View.ViewModels
{
	public class CategoryViewModel
	{
		public Category? NewCategories { get; set; }
		public IEnumerable<Category>? Categories { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int RowsPerPage { get; set; }
	}
}
