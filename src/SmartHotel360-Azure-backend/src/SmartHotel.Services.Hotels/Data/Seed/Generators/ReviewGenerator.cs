using SmartHotel.Services.Hotels.Domain.Hotel;
using SmartHotel.Services.Hotels.Domain.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Data.Seed.Generators
{
    public class ReviewGenerator
    {
        public List<Review> GetReviews(List<Hotel> hotels, int reviewPerhotel = 25)
        {
            var userNameGenerator = new UserNameGenerator();
            var reviews = new List<Review>();
            var random = new Random();
            var comments = GetComments();

            hotels.ForEach(hotel =>
            {
                for (int i = 0; i < reviewPerhotel; i++)
                {
                    var message = comments[random.Next(0, comments.Count())];
                    var roomType = hotel.RoomTypes.ElementAt(random.Next(0, hotel.RoomTypes.Count()));

                    reviews.Add(new Review
                    {
                        HotelId = hotel.Id,
                        Date = GetRandomDate(),
                        Message = message,
                        RoomType = roomType.Name,
                        UserName = userNameGenerator.GetName(),
                        Rating = random.Next(3, 5)
                    });
                }
            });

            return reviews;
        }

        private DateTime GetRandomDate()
        {
            Random random = new Random();
            DateTime minDate = new DateTime(2005, 1, 1);

            int maxDays = (DateTime.Today - minDate).Days;
            return minDate.AddDays(random.Next(maxDays));
        }

        private IList<string> GetComments()
        {
            return new List<string> {
                "Excellent hotel. Lobby and restaurant are impressive. Breakfast and dinner are really good. Service is great and one of the best in London. Try also de club downstairs and the Punch Room",
                "The restaurant and bar is wonderful! Lively enough, and excellent for business lunches as well. Try the Ham and Cheese toastie- delicious! We did not care for our room; however, too spartan.",
                "I really like this place with it's clean and tablet-controlled rooms, with a great movie selection (perfect after a long day in the office) and most importantly their always friendly staff...",
                "Hotel located just behind the Tate, with great breakfast spots and Borough Market nearby. Two people stay for the same price as one, so bring a friend. Whom you would share a double bed with.",
                "love this hotel: great bar downstairs, the rooms were a wee bit bigger than the ones at schiphol, and it's right around the corner from the gate modern...will definitely stay here again",
                "Trendy hotel with the best lounge area I've ever seen. Cozy place with comfy beds. Rooms are small but all right. Funny high-tech comfort.",
                "Great concept. Modern without being too clean. Super comfy beds. Samsung tablet which controls everything in your room is more of a gimmick than being a handy tool.",
                "From the moment you step in, you can perfectly understand why this hotel has been short listed for the annual World Architecture Festival Awards...",
                "Unbelievable everything! Mere words can not describe the staff grounds or rooms. The Spa is top notch the decor refined elegant and as beautiful as everyone who works their.",
                "antastic Hotel great service lovely environment upmarket but not pretentious at all superb food rooms are amongst the best I have stayed in. Well recommended folks",
                "Great cocktail bar with live music. Staff are amazing and attentive without being annoying.",
                "Have some work to do? Grab a good Full English and sit on one of their comfortable sofas with your laptop. Great wifi signal and plenty of outlets available.",
                "Brings together the integrity and character of a historic building with a simple, sophisticated design. Boasts 173 rooms, a restaurant, two bars, an event space, meeting rooms and an inviting lobby.",
            };
        }
    }
}
