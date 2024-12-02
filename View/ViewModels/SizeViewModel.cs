using Data.Models;

namespace View.ViewModels
{
    public class SizeViewModel
    {
        public Size? NewSize { get; set; }
        public IEnumerable<Size>? Sizes { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RowsPerPage { get; set; }
    }
}
