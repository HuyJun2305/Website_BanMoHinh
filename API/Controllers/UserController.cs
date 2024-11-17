
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using XuongTT_API.Model;

namespace XuongTT_API.Controllers
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
        public async Task<IActionResult> Create([FromBody] ApplicationUser value,string roleName, string pass)
        {
            

                var passwordHasher = new PasswordHasher<ApplicationUser>();

            ApplicationUser user = new ApplicationUser()
                {
                    UserName = value.UserName,
                    Email = value.Email,
                    Name = value.Name,
                    PhoneNumber = value.PhoneNumber,
                    ImgUrl = value.ImgUrl,
                };
            user.PasswordHash = passwordHasher.HashPassword(user, pass);
            IdentityResult result = await _userManager.CreateAsync(user);

                IdentityResult resultRole = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                if (resultRole.Succeeded)
                    return CreatedAtAction(nameof(Get), new { id = user.Id }, user.Id);
                else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa tạo thành công userRole:{string.Join(" ", resultRole.Errors.Select(e => e.Description))}");
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa tạo thành công user:{string.Join(" ", result.Errors.Select(e => e.Description))}");



        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Edit([FromBody] ApplicationUser value,string roleName, string pass)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var user1 = await _userManager.FindByNameAsync(value.UserName);
            if (user1 == null) { return NotFound(); }
            else
            {
                var id = user1.Id;
                user1.UserName = value.UserName;
                user1.PasswordHash = passwordHasher.HashPassword(user1, pass) ;
                user1.Email = value.Email;
                user1.PhoneNumber = value.PhoneNumber;
                user1.Name = value.Name;
                user1.PhoneNumber = value.PhoneNumber;
                user1.ImgUrl = value.ImgUrl;
                var result = await _userManager.UpdateAsync(user1);
                IdentityResult addRoleResultStaff;
                IdentityResult addRoleResultAdmin;
                IdentityResult removeRoleResultAdmin;
                if (!await _userManager.IsInRoleAsync(user1,roleName))
                {
                    if (roleName == "Admin")
                    {
                        addRoleResultStaff = await  _userManager.AddToRoleAsync(user1, "Staff");
                        addRoleResultAdmin = await  _userManager.AddToRoleAsync(user1, "Admin");
                        if (result.Succeeded)
                        {
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
                        addRoleResultStaff = await _userManager.AddToRoleAsync(user1, "Staff");
                        if (result.Succeeded)
                        {
                            if (addRoleResultStaff.Succeeded)
                            {
                                if (removeRoleResultAdmin.Succeeded)
                                {
                                    return Ok("Đã Xóa Khỏi Admin");
                                }
                                return Ok("Sửa thành công Staff");
                            }
                            else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa tạo thành công userRole:{string.Join(" ", addRoleResultStaff.Errors.Select(e => e.Description))}");
                        }
                        else return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa Sửa thành công user:{string.Join(" ", result.Errors.Select(e => e.Description))}");

                    }
                }
                return Ok("Sửa thành công");
                
            }
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
