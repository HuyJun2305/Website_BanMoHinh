using Data.Models;

namespace View.ViewModel
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RowsPerPage { get; set; }
    }
}
