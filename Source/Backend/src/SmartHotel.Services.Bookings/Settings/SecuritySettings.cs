using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Bookings.Settings
{
    public class SecuritySettings
    {
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string Authority => $"https://login.microsoftonline.com/tfp/{Tenant}/{Policy}/v2.0/";
        public string Policy { get; set; }
        public string Audiencies { get; set; }
        public IEnumerable<string> GetAudiences() => !string.IsNullOrEmpty(Audiencies) ? Audiencies.Split(',') : Enumerable.Empty<string>();
    }
}
