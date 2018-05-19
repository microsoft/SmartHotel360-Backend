using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHotel.Services.Hotels.Domain.Hotel;
using SmartHotel.Services.Hotels.Queries;

namespace SmartHotel.Services.Hotels.Controllers
{
    [Route("[controller]")]
    public class CitiesController : Controller
    {
        private readonly CitiesQuery _citiesQueries;
        private readonly ILogger _logger;

        public CitiesController(CitiesQuery citiesQueries, ILogger<CitiesController> logger)
        {
            _citiesQueries = citiesQueries;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string name = "")
        {
            var cities = string.IsNullOrEmpty(name) ? 
                await _citiesQueries.GetDefaultCities() :
                _citiesQueries.GetDefaultCities().Result
                    .Where(city => city.Name.StartsWith(name));

            if (cities.Count() == 0)
                _logger.LogWarning("City search {0} returned 0 results.", name);

            return Ok(cities);
        }
    }
}
