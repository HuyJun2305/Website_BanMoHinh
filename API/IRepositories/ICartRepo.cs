﻿using Data.Models;

namespace API.IRepositories
{
    public interface ICartRepo
    {
        Task<List<Cart>> GetAllCart();
        Task<Cart> GetCartById(Guid id);
        Task Create(Cart cart);
        Task Update(Cart cart);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}