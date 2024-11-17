using Microsoft.AspNetCore.Mvc;
using View.Iservices;

namespace View.Controllers
{
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

    }
}
