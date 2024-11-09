using API.IRepositories;
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
        [HttpGet("{id}")]
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
        //Add product
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            try
            {
                Cart ct = new Cart()
                {
                    Id = cart.Id,
                    Price = cart.Price,
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
                    Price = cart.Price,
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
        //Delete Product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            try
            {
                await _cartRepo.Delete(id);
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
