using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Profiles.Data
{
    public class Profile
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Alias { get; set; }

        public Loyalty Loyalty { get; set; }
    }
}
