using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SmartHotel.Services.Discount.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHotel.Services.Discount.Services
{
    public class LoyaltyService
    {
        private readonly string _url;
        private readonly ILogger _logger;
        public LoyaltyService(string url, ILogger<LoyaltyService> logger)
        {
            _url = url;
            _logger = logger;
        }


        public async Task<Loyalty> GetLoyaltyByCustomer(string userOrAlias)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{_url}profiles/{userOrAlias}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        dynamic profileResult = JObject.Parse(json);
                        int loyaltyAsInteger = profileResult.loyalty;
                        return (Loyalty)loyaltyAsInteger;
                    }
                    else
                    {
                        _logger.LogWarning($"Received {response.StatusCode} ('{response.ReasonPhrase}') when calling profile service for {userOrAlias}");
                        return Loyalty.None;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception {ex.GetType().Name} ('{ex.Message}') when connecting to profile service at {_url}");
                return Loyalty.None;
            }
        }

    }
}
