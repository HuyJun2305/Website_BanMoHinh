﻿using Data.DTO;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace View.IServices
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetAllProduct();
        Task<IEnumerable<ProductSize>> GetAllProductSizes();
        Task<Product> GetProductById(Guid id);
        Task Create(Product product);
        Task Update(ProductDto productUpdateDto);
        Task Delete(Guid id);
        Task<List<Product>> GetFilteredProduct(string? searchQuery = null,
            Guid? sizeId = null,
               Guid? imageId = null,
               Guid? brandId = null,Guid? categoryId = null,
               Guid? materialId = null);
    }
}
