using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Settings
{
    public class AppSettings
    {
        public Connectionstrings ConnectionStrings { get; set; }
    }

    public class Connectionstrings
    {
        public string DefaultConnection { get; set; }
    }
}
