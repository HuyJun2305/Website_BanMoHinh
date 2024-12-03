using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
	public class OrdersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
