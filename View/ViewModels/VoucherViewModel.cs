using Data.Models;

namespace View.ViewModels
{
    public class VoucherViewModel
    {
        public Voucher Voucher { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public int Percent { get; set; }
        public int Stock { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
        public bool Status { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
