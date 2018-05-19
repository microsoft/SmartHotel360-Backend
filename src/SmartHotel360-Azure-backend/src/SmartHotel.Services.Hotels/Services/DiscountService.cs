using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Services
{
    public class DiscountService
    {
        private readonly string _url;
        private readonly ILogger _logger;
        public DiscountService(string url, ILogger<DiscountService> logger)
        {
            _url = url;
            _logger = logger;
        }


        public async Task<double> GetDiscountByCustomer(string customerid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{_url}discounts/{customerid}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        dynamic discountResult = JObject.Parse(json);
                        double dvalue = discountResult.discount;
                        return dvalue;
                    }
                    else
                    {
                        _logger.LogWarning($"Received {response.StatusCode} ('{response.ReasonPhrase}') when calling discount service for {customerid}");
                        return 0.0d;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception {ex.GetType().Name} ('{ex.Message}') when connecting to discount service at {_url}");
                return 0.0d;
            }
        }
    }
}
