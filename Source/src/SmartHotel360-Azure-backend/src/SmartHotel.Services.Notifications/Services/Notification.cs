using System;

namespace SmartHotels.Services.Notifications.Services
{
    public enum NotificationType
    {
        BeGreen,
        Room,
        Hotel,
        Other
    }

    public class Notification
    {
        public int Seq { get; set; }
        public DateTime Time { get; set; }
        public NotificationType Type { get; set; }
        public string Text { get; set; }
    }
}