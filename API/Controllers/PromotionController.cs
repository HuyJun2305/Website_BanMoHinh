using API.IRepositories;
using API.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionRepos _promotionRepos;

        public PromotionController(IPromotionRepos promotionRepos)
        {
            _promotionRepos = promotionRepos;
        }
        // GET: api/Promotions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Promotion>> GetPromotion(Guid id)
        {
            try
            {
                var promotion = await _promotionRepos.GetPromotionById(id);
                if (promotion == null)
                {
                    return NotFound();
                }
                return Ok(promotion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Promotions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotion(Promotion promotion)
        {

            try
            {
                var promotionupdate = await _promotionRepos.GetPromotionById(promotion.Id);
                promotionupdate.Name = promotion.Name;
                promotionupdate.DayStart = promotion.DayStart;
                promotionupdate.DayEnd = promotion.DayEnd;
                promotionupdate.Description = promotion.Description;
                promotionupdate.Status = promotion.Status;

                await _promotionRepos.Update(promotionupdate);
                await _promotionRepos.SaveChanges();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        // POST: api/Promotions
        [HttpPost]
        public async Task<ActionResult<Promotion>> PostPromotion(Promotion promotion)
        {
            try
            {
                await _promotionRepos.Create(promotion);
                await _promotionRepos.SaveChanges();
                return Ok(promotion);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Promotions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            try
            {
                await _promotionRepos.Delete(id);
                await _promotionRepos.SaveChanges();
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
