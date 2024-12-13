using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderAddressRepo : IOrderAddressRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderAddressRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(OrderAddress orderAddress)
        {
            await _context.AddAsync(orderAddress);
        }

        public async Task Delete(Guid id)
        {
            var odid =await GetOrderAddressById(id);
            _context.Remove(odid);
        }

        public async Task<List<OrderAddress>> GetAllOrderAddress()
        {
            return await _context.OrderAddresses.ToListAsync();
        }

        public async Task<OrderAddress> GetOrderAddressById(Guid id)
        {
            return await _context.OrderAddresses.FindAsync(id);
        }

        public async Task<OrderAddress> GetOrderAddressByOrderId(Guid orderId)
        {
            return await _context.OrderAddresses.Include(od => od.Order).FirstOrDefaultAsync(od => od.OrderId == orderId);
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(OrderAddress orderAddress)
        {
            await GetOrderAddressById(orderAddress.Id);
            _context.Entry(orderAddress).State = EntityState.Modified;
        }
    }
}
