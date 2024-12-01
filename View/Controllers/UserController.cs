using Data.DTO;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using View.Database;
using View.IServices;
using View.ViewModel;

namespace View.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController

        private readonly IUserServices _userServices;
        private readonly List<string> roles = new List<string>() { "Admin", "Staff", "Customer" };
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("username");
            ViewData["RolesName"] = new SelectList(roles);

            var all = await _userServices.GetAllUser();
            //
            if (all == null || !all.Any())
            {
                ViewBag.Message = "No product details found.";
                return View(new List<ApplicationUser>());
            }

            var data = new List<UserData>(); 

            foreach (var item in all)
            {
                data.Add(new UserData { 
                    Id = item.Id,
                Username = item.UserName,
                Name = item.Name,
                PhoneNumber = item.PhoneNumber,
                Email = item.Email,
                ImgUrl = item.ImgUrl,
                Password = item.PasswordHash,
                role = await _userServices.GetAllRoles(item.UserName)
                
                });
            }
            return View(data);
        }

        // GET: UserController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (_userServices.GetAllUser() == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var data = new UserData
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ImgUrl = user.ImgUrl,
                Password = user.PasswordHash,
                role = await _userServices.GetAllRoles(user.UserName)

            };

            return View(data);
        }

        // GET: UserController/Create
        public  async Task<ActionResult> Create()
        {
            var username = HttpContext.Session.GetString("username");
            ViewData["RolesName"] = new SelectList(roles);
            return  View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Username, Name, PhoneNumber, Email, ImgUrl, Password, role")] UserData data)
        {
            try
            {
                await _userServices.Create(data);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (_userServices.GetAllUser()== null)
            {
                return NotFound();
            }
            var user = await _userServices.GetUserById(id);
            if(user == null) return NotFound();

            UserData data = new UserData
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ImgUrl = user.ImgUrl,
                Password = user.PasswordHash,
                role = await _userServices.GetAllRoles(user.UserName)

            };


            var username = HttpContext.Session.GetString("username");
            ViewData["RolesName"] = new SelectList(roles);

            return View(data);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserData data)
        {
            var user = await _userServices.GetUserById(id);
            if (user == null) return NotFound();
            await _userServices.Update(data, id);
            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (_userServices.GetAllUser() == null)
            {
                return NotFound();
            }
            var user = await _userServices.GetUserById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (_userServices.GetAllUser() == null)
            {
                return Problem("Entity set 'ProductDetail'  is null.");
            }
            await _userServices.Delete(id);
            return RedirectToAction(nameof(Delete));
        }
    }
}
