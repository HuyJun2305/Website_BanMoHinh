﻿using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class HomeCustomer : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
