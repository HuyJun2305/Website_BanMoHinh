using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Data.Models;
using API.IRepositories;
using API.Repositories;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repo;

		public CategoryController(ICategoryRepo repo)
		{
			_repo = repo;
		}

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
			try
			{
				return await _repo.GetAllCategories();

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(Guid id)
        {
			try
			{
				return await _repo.GetCategoryById(id);

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory( Category category)
        {
            try
            {
                await _repo.Update(category);
                await _repo.SaveChanges();
            }
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}

			return Content("Success!");
		}

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
			try
			{
				await _repo.Create(category);
				await _repo.SaveChanges();

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}

			return Content("Success!");
		}

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
			try
			{
				await _repo.Delete(id);
				await _repo.SaveChanges();

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}

			return Content("Success!");
		}

     
    }
}
