using Microsoft.Extensions.Options;
using SmartHotel.Services.ReviewsNet.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.ReviewsNet
{
    public class FormatDateService
    {
        private DateFormatConfig _cfg;
        public FormatDateService(IOptions<DateFormatConfig> cfg)
        {
            _cfg = cfg.Value;
        }

        public string FormatAsString(DateTime date)
        {
            if (!string.IsNullOrEmpty(_cfg.DateFormat))
            {
                return date.ToString(_cfg.DateFormat);
            }
            else
            {
                return date.ToLongDateString() + " " + date.ToLongTimeString();
            }
        }
    }
}
