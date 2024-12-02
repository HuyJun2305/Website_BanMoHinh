using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using View.IServices;
using View.Servicecs;
using View.ViewModels;

namespace View.Controllers
{
    public class AddresController : Controller
    {
        private readonly IAddresServices _services;
        private readonly HttpClient _httpClient;
        public AddresController(IAddresServices services, HttpClient httpClient)
        {
            _services = services;
            _httpClient = httpClient;
        }
        // GET: AddresController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AddresController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        

        // GET: AddresController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: AddresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("provinces, District, wards, AddressDetail, Description")] Address address)
        {
            
            if (address.Id != null)
            {
                address.Id = Guid.NewGuid();
                var jwtToken = HttpContext.Session.GetString("jwtToken"); // Lấy JWT từ session

                if (!string.IsNullOrEmpty(jwtToken))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();

                    // Kiểm tra xem token có hợp lệ không
                    if (tokenHandler.CanReadToken(jwtToken))
                    {
                        var jwt = tokenHandler.ReadJwtToken(jwtToken);

                        // Lấy claim chứa userId từ token
                        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                        if (Guid.TryParse(userIdClaim, out Guid userId))
                        {
                            address.AccountId = userId;

                            await _services.Create(address);
                        }
                        return View(address);
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AddresController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddresController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
