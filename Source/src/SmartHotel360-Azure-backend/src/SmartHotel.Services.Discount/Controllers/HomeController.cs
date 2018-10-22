using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartHotel.Services.Discount.Controllers
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
