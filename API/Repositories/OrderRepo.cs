using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

		public OrderRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public async Task Create(Order order)
        {
            if (await GetOrderById(order.Id) != null) throw new DuplicateWaitObjectException($"Order : {order.Id} is existed!");
            await _context.Orders.AddAsync(order);
        }

		public async Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherId = null)
		{
			var staffAccount = await _context.Accounts.FirstOrDefaultAsync(p => p.Id == staffId);
			if (staffAccount == null)
			{
				throw new Exception($"Staff account with ID {staffId} does not exist.");
			}

			var isStaff = await _context.UserRoles.AnyAsync(ur =>
				ur.UserId == staffId && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Staff"));
			if (!isStaff)
			{
				throw new Exception($"Account with ID {staffId} is not authorized as a Staff.");
			}

			ApplicationUser? customerAccount = null;
			if (customerId.HasValue)
			{
				customerAccount = await _context.Accounts.FirstOrDefaultAsync(p => p.Id == customerId);
				if (customerAccount == null)
				{
					throw new Exception($"Customer account with ID {customerId} does not exist.");
				}

				var isCustomer = await _context.UserRoles.AnyAsync(ur =>
					ur.UserId == customerId && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Customer"));
				if (!isCustomer)
				{
					throw new Exception($"Account with ID {customerId} is not authorized as a Customer.");
				}
			}

			Voucher? voucher = null;
			if (voucherId.HasValue)
			{
				voucher = await _context.Vouchers
									   .Include(v => v.Account)
									   .FirstOrDefaultAsync(v => v.Id == voucherId);

				if (voucher == null)
				{
					throw new Exception($"Voucher with ID {voucherId} does not exist.");
				}

				if (customerId.HasValue && voucher.AccountId != customerId)
				{
					throw new Exception("This voucher does not belong to the specified customer.");
				}
			}

			// Tạo đơn hàng mới
			var newOrder = new Order
			{
				Id = Guid.NewGuid(),
				AccountId = customerId,
				CreateBy = staffId,
				DayCreate = DateTime.Now,
				Price = 0,
				PaymentMethods = PaymentMethod.Cash,
				Status = OrderStatus.TaoDonHang,
				CustomerName = customerAccount?.Name ?? "Guest",
				VoucherId = voucherId
			};

			await _context.Orders.AddAsync(newOrder);
			await _context.SaveChangesAsync();

			return newOrder;
		}



		public async Task Delete(Guid id)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order == null)
			{
				throw new KeyNotFoundException($"Order with ID {id} not found.");
			}

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Orders.Include(p => p.Account).ToListAsync();
        }
        public async Task<Order> GetOrderById(Guid id)
        {
            return await _context.Orders
                                 .Include(p => p.Account)  // Bao gồm thông tin liên quan đến Account
                                 .FirstOrDefaultAsync(o => o.Id == id);  // Lọc theo ID đơn hàng
        }


        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }       

        public async Task Update(Order order)
        {
			var existingOrder = await GetOrderById(order.Id);
			if (existingOrder == null) throw new KeyNotFoundException("Not found this Id!");

			// Chỉ cập nhật các trường cần thiết
			existingOrder.Price = order.Price;
			existingOrder.PaymentMethods = order.PaymentMethods;
			existingOrder.Status = order.Status;
			existingOrder.CustomerName = order.CustomerName;

			// Đánh dấu thực thể đã được thay đổi
			_context.Entry(existingOrder).State = EntityState.Modified;
		}
    }
}
