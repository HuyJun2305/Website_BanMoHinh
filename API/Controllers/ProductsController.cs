﻿using API.IRepositories;
using Data.DTO;
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

        [HttpGet("ProductSize")]
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetProductSize()
        {
            try
            {
                return await _productRepo.GetAllProductSizes();
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
        public async Task<IActionResult> PutProduct(Guid id,[FromBody] ProductDto productUpdateDto)
        {
            if (id != productUpdateDto.Product.Id)
            {
                return BadRequest("Id sản phẩm không khớp.");
            }
            await _productRepo.Update(productUpdateDto);
            return Ok();
        }
        //Delete Product
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(Guid productId, Guid? sizeId)
        {
            try
            {
                await _productRepo.Delete( productId,sizeId);
                await _productRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return NoContent();
        }

        [HttpPost("AddSize")]
        public async Task<IActionResult> AddSize(Guid productId, Guid sizeId)
        {
            try
            {
                await _productRepo.AddSize(productId, sizeId);
                return Ok(new { success = true, message = "Size đã được thêm vào sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { success = false, message = errorMessage });
            }
        }

        [HttpPost("DistributeStock")]
        public async Task<IActionResult> DistributeStock(Guid productId, int totalStock, Dictionary<Guid, int> productSizesStock)
        {
            try
            {
                await _productRepo.DistributeStockToProductSizesAsync(productId, totalStock, productSizesStock);
                return Ok();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { success = false, message = errorMessage });
            }
        }


        [HttpGet("GetProductSizes/{productId}")]
        public async Task<IActionResult> GetProductSizes(Guid productId)
        {
            try
            {
                var product = await _productRepo.GetProductById(productId);
                if (product == null)
                    return NotFound("Product not found.");

                var productSizes = product.ProductSizes
                    .Select(ps => new
                    {
                        SizeId = ps.SizeId,
                        Value = ps.Size.Value, // Giả sử Size có thuộc tính Value
                        Stock = ps.Stock
                    }).ToList();

                return Ok(productSizes);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
