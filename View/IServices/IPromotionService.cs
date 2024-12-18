﻿using Data.Models;

namespace View.IServices
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetAllPromotion();
        Task<Promotion> GetPromotionById(Guid? id);
        Task Create(Promotion promotion);
        Task Update(Promotion promotion);
        Task Delete(Guid id);
    }
}
