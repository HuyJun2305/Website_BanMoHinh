﻿using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepos _productRepo;
        public ProductsController(IProductRepos productRepos)
        {
            _productRepo = productRepos;
        }
        //Get all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            try
            {
                return await _productRepo.GetAllProduct();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetByIdProduct(Guid id)
        {
            try
            {
                return await _productRepo.GetProductById(id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        //Add product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                Product sp = new Product()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    BrandId = product.BrandId,
                    MaterialId = product.MaterialId,
                    SizeId = product.SizeId
                    
                };
                await _productRepo.Create(sp);
                await _productRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Content("Success!");
        }
        //
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Product product)
        {
            try
            {
                Product sp = new Product()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    BrandId = product.BrandId,
                    MaterialId = product.MaterialId,
                    SizeId = product.SizeId
                };
                await _productRepo.Update(product);
                await _productRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return NoContent();
        }
        //Delete Product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productRepo.Delete(id);
                await _productRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return NoContent();
        }
    }
}
