﻿using API.IRepositories;
using Data.Models;
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
        //Tìm kiếm sản phẩm 
        [HttpGet("filterAndsearch")]
        public async Task<ActionResult<List<Product>>> FilterProduct(string? searchQuery = null, Guid? brandId = null, Guid? materialId = null, Guid? categoryId = null)
        {
            var product = await _productRepo.GetFilteredProduct(searchQuery,  brandId, materialId, categoryId);
            if (product == null || product.Count == 0)
            {
                return NotFound();
            }
            return Ok(product);
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
                await _productRepo.Create(product);
                await _productRepo.SaveChanges();
                
            }
            catch (Exception ex)
            {
                return Problem(ex.InnerException.Message);
            }
            return Content("Success!");
        }
        //
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Product product)
        {
            await _productRepo.Update(product);
            return Ok();
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
