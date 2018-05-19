using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOFile = System.IO.File;

namespace SmartHotel.Services.Configuration.Controllers
{
    [Route("cfg", Name = "MainConfigRoute")]
    public class ConfigController : Controller
    {
        [HttpGet("{file}")]
        public async Task<IActionResult> GetConfig(string file)
        {


            var fname = $@"cfg{Path.DirectorySeparatorChar}{file}.json";

            if (IOFile.Exists(fname))
            {
                var data = await IOFile.ReadAllBytesAsync(fname);
                return File(data, "application/json");
            }

            return NotFound($"Not found config file for '{file}'");
        }

        [HttpGet("")]
        public IActionResult GetAllConfigNames()
        {
            var path = "cfg";
            var files = Directory.EnumerateFiles(path, "*.json", SearchOption.TopDirectoryOnly)
                .Select(f => Path.GetFileNameWithoutExtension(f));

            return Ok(files);
        }
    }
}