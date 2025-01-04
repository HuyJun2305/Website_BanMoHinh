using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public int Percent { get; set; }
        public int Stock { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
        public VoucherStatus Status { get; set; }
        public Guid AccountId { get; set; }

        public virtual ApplicationUser? Account { get; set; }

        // Kiểm tra tính hợp lệ của voucher
        public bool IsValid()
        {
            return Status == VoucherStatus.Active && DayStart <= DateTime.Now && DayEnd >= DateTime.Now;
        }

        // Giảm số lượng voucher khi được sử dụng
        public bool UseVoucher()
        {
            if (Stock > 0)
            {
                Stock--;
                return true;
            }
            return false;
        }
    }

    public enum VoucherStatus
    {
        Active,
        Expired,
        Used
    }

}
