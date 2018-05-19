using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHotels.Services.Notifications.Services;

namespace SmartHotels.Services.Notifications.Controllers
{
    [Route("notifications")]
    public class NotificationsController : Controller
    {
        private readonly NotificationsService _notifSvc;
        public NotificationsController(NotificationsService notifSvc)
        {
            _notifSvc = notifSvc;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetByUser(int seq = -1)
        {
            var now = DateTime.Now;
            var userid = User.Claims.First(c => c.Type == "emails").Value;
            var data = _notifSvc.GetNotificationsForUser(userid)
                .Where(n => n.Seq > seq)
                .Where(n => n.Time <= now)  
                .Take(3);

            return Ok(data);
        }
    }
}
