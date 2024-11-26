using Data.Models;

namespace View.ViewModels
{
    public class PromotionViewModel
    {
        public Promotion Promotion { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
        public bool Status { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
