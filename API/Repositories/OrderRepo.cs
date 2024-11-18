using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Order order)
        {
            if (await GetOrderById(order.Id) != null) throw new DuplicateWaitObjectException($"Brand : {order.Id} is existed!");
            await _context.Orders.AddAsync(order);
        }

        public async Task Delete(Guid id)
        {
            var order = await GetOrderById(id);
            if (order == null) throw new KeyNotFoundException("Not found this brand!");
            _context.Orders.Remove(order);
        }

        public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Orders.Include(p => p.Account).ToListAsync();
        }
        public async Task<Order> GetOrderById(Guid id)
        {
            return await _context.Orders.Include(p => p.Account).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }       

        public async Task Update(Order order)
        {
            if (await GetOrderById(order.Id) == null) throw new KeyNotFoundException("Not found this Id!");
            _context.Entry(order).State = EntityState.Modified;
        }
    }
}
