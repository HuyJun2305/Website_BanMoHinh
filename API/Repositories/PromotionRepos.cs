﻿using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class PromotionRepos : IPromotionRepos
    {
        private readonly ApplicationDbContext _context;

        public PromotionRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Promotion promotion)
        {

            if (await GetPromotionById(promotion.Id) != null)
            {
                throw new DuplicateWaitObjectException($"Promotion : {promotion.Id} is existed!");
            }
            await _context.Promotions.AddAsync(promotion);
        }
        public async Task applyToProduct(List<Guid> pros,Guid promId)
        {
            
            var promotion = await GetPromotionById(promId);
            if(promotion == null)
            {
                throw new KeyNotFoundException("Not found this Id!");
            }
            foreach (Guid id in pros)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if(promotion.PriceReduced != null)
                {
                    product.Price -=  promotion.PriceReduced.Value;
                }else
                {
                    product.Price -= product.Price * (promotion.PercentReduced.Value)/100;
                }
                _context.Entry(product).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }
        public async Task Delete(Guid id)
        {
            var deleteItem = await GetPromotionById(id);
            if (deleteItem == null)
            {
                throw new KeyNotFoundException("Not found this Id!");
            }
            _context.Promotions.Remove(deleteItem);
        }

        public async Task<List<Promotion>> GetAllPromotion()
        {
            return await _context.Promotions.ToListAsync();
        }

        public async Task<Promotion> GetPromotionById(Guid id)
        {
            return await _context.Promotions.FindAsync(id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Promotion promotion)
        {
            var item = await GetPromotionById(promotion.Id);
            if (item == null)
            {
                throw new KeyNotFoundException("Not found this Id!");
            }
            _context.Entry(promotion).State = EntityState.Modified;
        }
    }
}
