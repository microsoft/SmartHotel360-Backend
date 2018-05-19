package com.smarthotel.reviews.seeders;


import com.smarthotel.reviews.models.Review;
import com.smarthotel.reviews.repositories.ReviewRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.event.ContextRefreshedEvent;
import org.springframework.context.event.EventListener;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.text.SimpleDateFormat;
import java.util.*;
import java.util.concurrent.ThreadLocalRandom;

@Component
public class ReviewSeeder {

    private JdbcTemplate jdbcTemplate;
    private ReviewRepository reviewRepository;
    private final int MAX_DAY_OF_MONTH = 15;
    private  final int MAX_HOTEL_ID = 42;
    private List<String> reviews;
    private List<String> names;
    private List<String> familyNames;

    @Autowired
    public ReviewSeeder(ReviewRepository reviewRepository,
                      JdbcTemplate jdbcTemplate) {
        this.reviewRepository = reviewRepository;
        this.jdbcTemplate = jdbcTemplate;
        this.reviews = new ArrayList<>();
        this.names = new ArrayList<>();
        this.familyNames = new ArrayList();
        this.reviews.add("Great location, really pleasant and clean rooms, but the thing that makes this such a good place to stay are the staff. All of the people are incredibly helpful and generous with their time and advice. We travelled with two six year olds and lots of luggage and this was one of the nicest places we stayed.");
        this.reviews.add("Great location. Nice rooms, elevator, good breakfast, clean. Staff were extremely helpful, courteous and knowledgeable. The hotel arranged our tours of the principal museums. Only a five-minute walk to the train station. The hotel arranged to have us picked up at the airport.");
        this.reviews.add("Most friendly and helpful receptionist ever, so lovely and great first impression of hotel. Couldn't have been more sweet, giving me directions to a function I was attending along with a helpful map, ensuring parking in car park and facilitating an early check in. If nothing else had gone right with hotel this lady ensured I would have left with a good impression. Fortunately everything about the hotel was exceptional, and I don't give praise lightly. It was clean, stylish, roomy with excellent service in both bar where we had lunch and restaurant where we had dinner. Food was beyond good and great value for money and service in both places attentive and efficient. Room itself was well equipped and comfortable. I could go on but suffice it to say I was very pleased with my stay, and although short and sweet this time, I hope to be back for a longer visit in the future.");
        this.reviews.add("Great staff that know the true value of customer service special mention for reception and the restaurant first class service, the evening menu was varied and the food exceptional the coffee creme brulee was divine");
        this.reviews.add("A very beautiful hotel walking distance from the railway station . The interiors are beautiful and breakfast is amazing. The front office has a beautiful piano placed and a lovely sit out area. In the evening they serve free snacks . Overall a great place to stay in");
        this.reviews.add("The place was clean and beautiful. The staff service was excellent. The room and bathroom was very spacious and lovely. The breakfast during morning was excellent. When ever we need something and request for it, it's done right away. The staff were always asking about our needs. Would definitely stay here again and would recommend this place too. ");
        this.reviews.add("The hotel is very well maintained, rooms are very clean and comfortable, staff are very professional and friendly, always proactively offering their help to make my stay comfortable to the maximum. It is a great value for money. Strongly recommended to everyone.");
        this.reviews.add("Excellent service, really good breakfast and room amenities. Staff is really courteous and property is really peaceful inspite of being right in the city centre. Will definitely go back :)");
        this.reviews.add("Excellent service, very friendly staff, very clean place and good variety of restaurants. Spa service was very good. Loved the food in The Lion King place hotel and burgers from The Burger Paradise.");
        this.reviews.add("Excellent hotel, very friendly and helpful staff, great location, prompt room service, very clean rooms. I would highly recommend this hotel to anyone.");
        this.reviews.add("Overall can't think of one thing that wasn't anything to think negative of this hotel.. 10 out of 10 for me..! Great ! Will definitely come back again.");
        this.reviews.add("Fantastic hotel, with an exceptional and calm garden and beautiful swimming pool! Remarkable staff, service and attention to details! very good deals. An exceptional experience.");
        this.reviews.add("The service was impeccable throughout and we were also afforded early check-in with a room upgrade. The interiors are beautiful and the room and bed were comfortable.");
        this.reviews.add("Loved the experience truly. Friendly and helpful staff. Lovely automation in the bedrooms. Everything was just excellent about this place. I would highly recommend this hotel.");
        this.reviews.add("This hotel is simply outstanding. The staff are wonderful, everyone. I didn't come across one staff who was just an average. Everyone tried to give 100% and very helpful. The bar and the restaurants are simply outstanding in food quality and service. The Garden is very beautiful and well maintained. It is so nice to see eagles in some of the big trees nesting. The lobby is a wonderful place to sit and chat with friends. The fragrance in the Lobby is incredible. Daily Room service is perfect.");
        this.reviews.add("An amazing experience in an amazing hotel belonging to an amazing group. I couldn't have been happier. Wonderful service, super food, lovely peaceful pool, terrific gym, beautiful rooms, surrounded by greenery");
        this.reviews.add("This must truly be one of the very best hotels in World. The professionalism of the staff and the level of the facilities are just unparalleled. I had a great stay and I look forward to returning");
        this.reviews.add("The staff was extraordinarily selected and well-trained. The breakfast is superb in quality and breadth of offering. Nothing was left to chance, the facility  is beautiful and an oasis in the center of Bangalore. The value position is excellent - this hotel is well worth a stay.");
        this.reviews.add("Loved everything :) Great staff, fabulous rooms and latest facilities, expected it to great but it was awesome");
        this.reviews.add("This hotel is a gem, basically because of the kindness of the staff and the personalized attention that they give to the guests. The beds are super comfortable, the breakfast delicious, the garden well kept. Room is serviced twice a day and they constantly offer you little gifts, fruit, infusions. They pamper you to no end.");
        this.reviews.add("The staff was genuinely friendly, excellent service, beautiful room and garden. Very good value! The food was excellent and expensive compare to outside restaurant...and was worthwhile to spend more.");
        this.reviews.add("The breakfast was superb with all kinds of food. Could not have been with more variety! Also very clean rooms and very kindly staff");
        this.reviews.add("I travelled with my mum and she loved absolutely everything. The location is perfect as everything is close by. The staff are extremely friendly and polite you will be treated like royalty.");
        this.reviews.add("Nothing not to like. We came here for our wedding anniversary and they left no detail unattended. We felt like royalty and yet like family. It was supremely relaxing yet elegant. We can't wait to return. T");
        this.reviews.add("Everything was great. The staff, service, cleanliness, breakfast, value for money .... We never heard the word 'no' from their staff. They accomodated all our requests as soon as they could. Sure we will return!");
        this.reviews.add("Excellent place to stay and fantastic staff ready to help at any time. very prompt with their services.");
        this.reviews.add("Room was very comfortable and clean. Room service was excellent. They even sent us a cake for our anniversary. Washrooms had a steam chamber. Garden and pool area were nice. Overall was an amazing experience.");
        this.reviews.add("This is a 5 Star Hotel with a 6 Star Service. Excellent rooms, friendly staff, very attentive, top quality foods which was well presented. The premises, dining and lounge was well maintained. After staying in three other hotels in the same city this is my choice and is highly recommended. Very impressive management.");
        this.reviews.add("There was nothing to dislike- the staff were so kind and made us feel very special in a five star setting. One of the best hotels I ever was.");
        this.reviews.add("SmartHotel360 is a class apart. all their hotels and the service are A++. 100% recommended.");
        this.reviews.add("The staff was wonderful. Security was pretty awesome and the food - yummy! The room was clean. Well designed, clean and very comfortable!");
        this.reviews.add("The service is absolutely outstanding. Every detail was immediately taken care of.");
        this.reviews.add("The breakfast was superb. And the extra attentiveness and care taken by the staff made it all the more interesting.");
        this.reviews.add("I was shocked by the advanced room automation and it is connectivity and control using my own mobile! Amazing!");
        this.reviews.add("The best hotel experience I ever had! Exceptional people serving you in restaurants and housekeeping");
        this.reviews.add("Quit hotel, offering great benefits. The staff is friendly and very professional. Breakfast of quality and very diversified. Came with our dog!");
        this.reviews.add("I travel extensively for business and honestly this hotel is very nice and way above average. The staff are super friendly, the hotel room is big (we had a family room with one double bed and a sofa bed), beautiful and very modern. Breakfast is top with great choice.");
        this.reviews.add("Great hotel in the centre and just in front of the main station. Clean, very quiet with friendly staff. Great room with a good bed and a perfect bathroom. Excellent breakfast!");
        this.reviews.add("Everything is just amazing about this place. You do not want to go home after coming here. Service, ambiance, people, food you name it and it is all perfect! Amazing experience.");


        this.names.add("Erika");
        this.names.add("Scott");
        this.names.add("James");
        this.names.add("Paul");
        this.names.add("Lisa");
        this.names.add("Steve");
        this.names.add("Beth");
        this.names.add("Ingrid");
        this.names.add("Joe");
        this.names.add("John");
        this.names.add("Rachel");
        this.names.add("Craig");
        this.names.add("Clarke");
        this.names.add("Chun");
        this.names.add("Raymond");
        this.names.add("An");
        this.names.add("Hans");
        this.names.add("Brady");
        this.names.add("Donovan");

        this.familyNames.add("Smith");
        this.familyNames.add("Thomas");
        this.familyNames.add("Gaster");
        this.familyNames.add("Anderson");
        this.familyNames.add("Massi");
        this.familyNames.add("Chen");
        this.familyNames.add("Gu");
        this.familyNames.add("Erhli");
        this.familyNames.add("Lasker");
        this.familyNames.add("Stern");
        this.familyNames.add("Li");
        this.familyNames.add("Bell");
        this.familyNames.add("Murphy");
        this.familyNames.add("Brooks");
        this.familyNames.add("Kelly");
        this.familyNames.add("Hughes");
        this.familyNames.add("Brown");
        this.familyNames.add("Wallace");
        this.familyNames.add("Payne");
        this.familyNames.add("Hudson");
        this.familyNames.add("Perkins");
    }

    @EventListener
    public void seed(ContextRefreshedEvent event) {
        boolean alreadySeeded = reviewRepository.count() > 0;


        if (alreadySeeded) {
            return;
        }

        Random rnd = new Random();
        int nameidx =0;
        int surnameidx = 0;
        for (int i = 0; i < 500; i++) {
            String name = this.names.get(nameidx);
            String familyName = this.familyNames.get(surnameidx);
            nameidx++;
            if (nameidx >= this.names.size()) {
                surnameidx++;
                nameidx=0;
                if (surnameidx >= this.familyNames.size()) {
                    surnameidx = 0;
                }
            }
            Review review = new Review();
            review.setUserId((name.charAt(0) + familyName).toLowerCase() + "@outlook.com");
            review.setUserName(name + " " + familyName);
            review.setSubmitted(new GregorianCalendar(2017, Calendar.NOVEMBER, (i%MAX_DAY_OF_MONTH) + 1).getTime());
            review.setHotelId((i%MAX_HOTEL_ID) +1);
            review.setDescription(reviews.get(i%reviews.size()));
            reviewRepository.save(review);
        }
    }
}
