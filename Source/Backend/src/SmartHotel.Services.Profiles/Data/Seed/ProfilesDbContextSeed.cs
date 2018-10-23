using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Profiles.Data.Seed
{
    public class ProfilesDbContextSeed
    {
        public static void Seed(ProfilesDbContext db)
        {
            if (db.Profiles.Any())
            {
                return;
            }

            db.Profiles.Add(new Profile()
            {
                UserId = "shanselman@outlook.com",
                Alias = "ALFKI",
                Loyalty = Loyalty.Platnum
            });

            db.SaveChanges();
        }
    }
}
