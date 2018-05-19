using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Discount.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SmartHotel.Services.Discount.Controllers
{
    [Route("[controller]")]
    public class DiscountsController : Controller
    {
        private readonly LoyaltyService _loyaltyService;
        public DiscountsController(LoyaltyService loyaltyService) => _loyaltyService = loyaltyService;
        [Route("{userid}")]
        [HttpGet()]
        public async Task<IActionResult> GetDiscountPerUser(string userid)
        {
            // Get the customers loyalty level
            Loyalty loyaltyLevel = await _loyaltyService.GetLoyaltyByCustomer(userid);

            double discount = 0.0d;
            switch (loyaltyLevel)
            {
                case Loyalty.Silver:
                    discount = 0.05d;
                    break;
                case Loyalty.Platnum:
                    discount = 0.10d;
                    break;
                case Loyalty.Latinum:
                    discount = 0.20d;
                    break;
                default:
                    discount = 0.0d;
                    break;
            }
            // index in on the loyalty
            var discountResult = new {
                Discount = discount
            };
            return Ok(discountResult);
        }
    }
}
