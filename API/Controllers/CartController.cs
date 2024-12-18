﻿using API.IRepositories;
using API.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
           _cartRepo = cartRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCart()
        {
            try
            {
                return await _cartRepo.GetAllCart();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        //
        [HttpGet("GetCartById")]
        public async Task<ActionResult<Cart>> GetByIdCart(Guid id)
        {
            try
            {
                return await _cartRepo.GetCartById(id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet("GetCartByUserId")]
        public async Task<ActionResult<Cart>> GetCartByUserId(Guid userId)
        {
            try
            {
                return await _cartRepo.GetCartByUserId(userId);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        //Add product
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            try
            {
                Cart ct = new Cart()
                {
                    Id = cart.Id,
                    Account = cart.Account,
                    AccountId = cart.AccountId

                };
                await _cartRepo.Create(ct);
                await _cartRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Content("Success!");
        }
        //
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(Cart cart)
        {
            try
            {
                Cart ct = new Cart()
                {
                    Id = cart.Id,
                    Account = cart.Account,
                    AccountId = cart.AccountId

                };
                await _cartRepo.Create(ct);
                await _cartRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return NoContent();
        }
        
    }
}
