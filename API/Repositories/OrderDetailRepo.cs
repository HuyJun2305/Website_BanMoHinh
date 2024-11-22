using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(OrderDetail orderDetail)
        {
            if (await GetOrderDetailById(orderDetail.Id) is not null) throw new DuplicateWaitObjectException("This orderDetail is existed!");
            await _context.OrderDetails.AddAsync(orderDetail);
        }

        public async Task Delete(Guid id)
        {
            var data = await GetOrderDetailById(id);
            if (data is null) throw new KeyNotFoundException("Not found this orderDetail");
            _context.OrderDetails.Remove(data);
        }

        public async Task<List<OrderDetail>?> GetAllOrderDetails()
        {
            return await _context.OrderDetails.Include(od => od.Order).ToListAsync();
        }

        public async Task<OrderDetail?> GetOrderDetailById(Guid id)
        {
            return await _context.OrderDetails.Where(od => od.Id == id) .Include(od => od.Order) .FirstOrDefaultAsync();
        }

        public  async Task<List<OrderDetail>?> GetOrderDetailsByOrderId(Guid id)
        {
            return await _context.OrderDetails.Where(od => od.OrderId == id).Include(od => od.Order).ToListAsync();
        }

       

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(OrderDetail orderDetail)
        {
            var data = await GetOrderDetailById(orderDetail.Id);
            if (data is null) throw new KeyNotFoundException("Not found this orderDetail");
            _context.Entry(orderDetail).State = EntityState.Modified;
        }
    }
}
