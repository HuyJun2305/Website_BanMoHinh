﻿using Data.Models;

namespace View.IServices
{
    public interface ISizeServices
    {
        Task<IEnumerable<Size>?> GetAllSizes();
        Task<IEnumerable<Size>?> GetSizeByStatus();
        Task<Size?> GetSizeById(Guid id);
        Task Create(Size size);
        Task Update(Size size);
        Task Delete(Guid id);
    }
}
