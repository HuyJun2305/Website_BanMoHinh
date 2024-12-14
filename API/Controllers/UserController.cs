
using Data.DTO;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;



        public UserController( UserManager<ApplicationUser> userManager = null)
        {
            
            _userManager = userManager;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApplicationUser>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ApplicationUser>> Get()
        {
            return Ok(_userManager.Users);
            //return Ok(/*_reviewRepo.Users*/_dbContext.Users.ToList());
        } 
        [HttpGet("GetRoles/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IdentityRole<Guid>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  async Task<ActionResult<List<string>>> GetRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return Ok(await _userManager.GetRolesAsync(user));
            //return Ok(/*_reviewRepo.Users*/_dbContext.Users.ToList());
        } 
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApplicationUser>> Get(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            else
                return Ok(result);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] UserData data)
        {
            

                var passwordHasher = new PasswordHasher<ApplicationUser>();

            ApplicationUser user = new ApplicationUser()
                {
                    UserName = data.Username,
                    Email = data.Email,
                    Name = data.Name,
                    PhoneNumber = data.PhoneNumber,
                    ImgUrl = data.ImgUrl,
                };
            user.PasswordHash = passwordHasher.HashPassword(user, data.Password);
            IdentityResult result = await _userManager.CreateAsync(user);

                IdentityResult resultRole = await _userManager.AddToRolesAsync(user, data.role);
            if (result.Succeeded)
            {
                if (resultRole.Succeeded)
                    return CreatedAtAction(nameof(Get), new { id = user.Id }, user.Id);
                else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa tạo thành công userRole:{string.Join(" ", resultRole.Errors.Select(e => e.Description))}");
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa tạo thành công user:{string.Join(" ", result.Errors.Select(e => e.Description))}");



        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Edit([FromBody] UserData value, string id)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var user1 = await _userManager.FindByIdAsync(id);
            if (user1 == null) { return NotFound(); }
            else
            {
                
                user1.UserName = value.Username;
                user1.PasswordHash = passwordHasher.HashPassword(user1, value.Password) ;
                user1.Email = value.Email;
                user1.PhoneNumber = value.PhoneNumber;
                user1.Name = value.Name;
                user1.ImgUrl = value.ImgUrl;
                var result = await _userManager.UpdateAsync(user1);
                IdentityResult addRoleResultStaff;
                IdentityResult addRoleResultAdmin;
                IdentityResult removeRoleResultAdmin;
                if (result.Succeeded)
                {
                    
                    if (value.role.Contains("Admin"))
                    {
                        addRoleResultStaff = await  _userManager.AddToRoleAsync(user1, "Staff");
                        addRoleResultAdmin = await  _userManager.AddToRoleAsync(user1, "Admin");
                        
                            if (addRoleResultAdmin.Succeeded)
                            {
                                if (addRoleResultStaff.Succeeded)
                                {
                                    return Ok("Sửa thành công admin nhưng chưa thành công thêm staff");
                                }

                                return Ok("Sửa thành công Admin"); 

                            }
                            else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa sửa thành công userRole:{string.Join(" ", addRoleResultAdmin.Errors.Select(e => e.Description))}");
                        }
                        else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa sửa thành công user:{string.Join(" ", result.Errors.Select(e => e.Description))}");

                    }
                    else
                    {
                        removeRoleResultAdmin = await _userManager.RemoveFromRoleAsync(user1, "Admin");
                        if (removeRoleResultAdmin.Succeeded)
                        {
                            return Ok("Đã Xóa Khỏi Admin");
                        }
                        if (!await _userManager.IsInRoleAsync(user1, "Staff"))
                        {
                            addRoleResultStaff = await _userManager.AddToRoleAsync(user1, "Staff");
                            if (addRoleResultStaff.Succeeded)
                            {
                                return Ok("Sửa thành công Staff");
                            }
                                else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa tạo thành công userRole:{string.Join(" ", addRoleResultStaff.Errors.Select(e => e.Description))}");

                            }



                        }
                }

                

                return Ok("Sửa thành công");
                
            
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null) { return NotFound(); }
            else
            {
                var result = await  _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok("Xóa thành công");
                }
                return BadRequest("Xóa thất bại");
            }

        }
    }
}
