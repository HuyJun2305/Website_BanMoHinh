using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepos _voucherRepos;

        public VoucherController(IVoucherRepos voucherRepos)
        {
            _voucherRepos = voucherRepos;
        }
       

    }
}
