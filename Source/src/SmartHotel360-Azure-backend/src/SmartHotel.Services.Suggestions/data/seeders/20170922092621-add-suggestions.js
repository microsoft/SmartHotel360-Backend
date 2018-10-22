'use strict';

var getRandom = function (min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min; //The maximum is inclusive and the minimum is inclusive 
};

var getCities = function () {
    var cities = [
        { name: "Seattle", latitude: 47.608013, longitude: -122.335167 },
        { name: "New York", latitude: 40.712784, longitude: -74.005941 },
        { name: "Barcelona", latitude: 41.385064, longitude: 2.173403 },
        { name: "Welligton", latitude: -41.286460, longitude: -174.776236 }
    ];

    return cities;
};

var getRestaurants = function () {
    var restaurants = [
        { name: 'Black Fox Coffee Co.', city: 'New York', coordinate: { latitude: 40.70629, longitude: -74.00786 }, description: 'Here\'s what you should know about Black Fox on Pine Street: The shop has best bean selection day to day of any shop in the city. As a result, it\'s packing in downtown workers looking for a coffee fix and above-average coffee shop food, like kale and farro salad with pesto, a roast chicken and avocado sandwich, and a smoked salmon bagel.', picture: 'suggestions/nyc_1.jpg' },
        { name: 'Voyager Espresso', city: 'New York', coordinate: { latitude: 40.70874938095975, longitude: -74.0070341029628 }, description: 'Named for the Voyager spacecraft, this shop goes for the sci-fi vibe with beakers and scientific items as part of the experience here. Coffee fans take note of the offbeat featured roaster, often one not based in the U.S. For eating, there’s fancy toasts like avocado and one topped with Nutella.', picture: 'suggestions/nyc_2.jpg' },
        { name: 'Everyman Espresso', city: 'New York', coordinate: { latitude: 40.72137, longitude: -74.004377 }, description: 'The West Broadway location is the star of the three locations. The move here is to get one of the signature drinks like the panacea or espresso old-fashioned.', picture: 'suggestions/nyc_3.jpg' },
        { name: 'Blue Bottle Coffee', city: 'New York', coordinate: { latitude: 40.719227, longitude: -73.985203 }, description: 'There are many locations of Blue Bottle in the city, but this one is an oasis in the neighborhood for pour-overs and cold brew.', picture: 'suggestions/nyc_4.jpg' },
        { name: 'Third Rail Coffee', city: 'New York', coordinate: { latitude: 40.72982797782921, longitude: -73.99982929229736 }, description: 'A small and rustic-chic venue that\'s packed into the evening with NYU students, this is a go- to spot for a coffee before a movie at the IFC Center.A menu of classics is complemented by Doughnut Plant doughnuts and numerous other baked goods.Just keep in mind it\'s cash only.', picture: 'suggestions/nyc_5.jpg' },
        { name: 'Abraço', city: 'New York', coordinate: { latitude: 40.727190300039474, longitude: -73.98609917877279 }, description: 'Even though what had been a tiny shop has grown larger in an across-the-street move, Abraco remains a destination for quality coffee and the baked goods. AN espresso is especially fantastic when owner Jamie McCormick pours. Take note of the tunes, start-to-finish vinyl records.', picture: 'suggestions/nyc_6.jpg' },
        { name: 'Hi-Collar', city: 'New York', coordinate: { latitude: 40.72944395157202, longitude: -73.98588683367826 }, description: '"Choose your bean" among several on the menu. And consider the array of different brewing methods.The experience is a draw as much as anything here.', picture: 'suggestions/nyc_7.jpg' },
        { name: 'The ELK', city: 'New York', coordinate: { latitude: 40.73424147849653, longitude: -74.00747596129659 }, description: 'Look for a Vancouver vibe from this Canadian native, Claire Chan has opened a destination for a fine cortado and a single-origin pour-over. The general store slash coffee shop with a light menu includes bruleed grapefruit and baked egg toast.', picture: 'suggestions/nyc_8.jpg' },
        { name: 'Toby\'s Estate Coffee', city: 'New York', coordinate: { latitude: 40.73493358250772, longitude: -74.00211989879607 }, description: 'The third addition to the Toby\'s Estate family in NYC, and also the most comprehensive. The large space is outfitted with all sorts of brewing devices, and the menu includes a range of single-origin coffees. It\'s also one of the best bets for the serious coffee nerd because it offers regular classes on everything from latte art to coffee tasting.', picture: 'suggestions/nyc_9.jpg' },
        { name: 'Intelligentsia Coffee', city: 'New York', coordinate: { latitude: 40.745911003619725, longitude: -74.00536421354879 }, description: 'This Chicago import is one of the best options in a neighborhood lacking in good coffee. Drinks come beautifully prepared and is served in standout glassware. Plus there are some prepared salads and pre-packaged sandwiches for the lunch crowds, along with pastries from Bien Cuit and Mah Ze Dahr.', picture: 'suggestions/nyc_10.jpg' },
        { name: 'Stumptown Coffee Roasters', city: 'New York', coordinate: { latitude: 40.745901560214506, longitude: -73.98814227092512 }, description: 'The Ace Hotel staff keeps the ever present line moving at a good clip. The cappuccino is especially stellar: perfect milk to espresso ratio, and milk temperature. It\'s ground zero for nitro in the city.', picture: 'suggestions/nyc_11.jpg' },
        { name: 'Culture Espresso', city: 'New York', coordinate: { latitude: 40.752285553402615, longitude: -73.98580074776224 }, description: 'A solid cortado and drip coffee anchor the menu of this stylish coffee bar near Bryant Park. It\'s a much stronger choice for coffee than the several chain stores closer to the park and it serves a superb chocolate chip cookie', picture: 'suggestions/nyc_12.jpg' },
        { name: 'Little Collins', city: 'New York', coordinate: { latitude: 40.75990312387116, longitude: -73.96976709365845 }, description: 'The Aussies running this Midtown shop have provided a getaway for a neighborhood otherwise filled with Starbucks and Le Pains. Alongside flat whites and pour overs the shop serves sandwiches, salads, and a behemoth of toasted banana bread with ricotta, berries, honey, and almond brittle. Get the schnitzel sandwich or the always-solid avocado smash.', picture: 'suggestions/nyc_13.jpg' },
        { name: 'Padoca', city: 'New York', coordinate: { latitude: 40.76519004067687, longitude: -73.95801020048518 }, description: 'Billing itself as a "creative bakery," this shop can feel a little too much like a Panera bakery, with Oasis playing on the speakers and a too-corporate vibe. The staff is still figuring out the menu somewhat, but all is forgiven with an espresso dispensed from the stylish Modbar sheer aluminum fittings. The coffee beans are sourced from Brooklyn’s Nobletree, which is a roaster that also oversees the growing of beans from its own fields in Brazil. The end result at Padoca is notably good. And there is free Wifi available.', picture: 'suggestions/nyc_14.jpg' },
        { name: 'Flora Coffee', city: 'New York', coordinate: { latitude: 40.773377, longitude: -73.963821 }, description: 'Just before Flora Bar is Flora Coffee, a lower-key extension of the UES stunner serving coffee, pastries, and takeaway sandwiches. It’s smaller than Flora’s main dining room, but there’s plenty of room for an expert cup of coffee, provided by Counter Culture. It’s also where pastry chef Natasha Pickowicz is selling out of her can’t-be-missed sticky buns.', picture: 'suggestions/nyc_15.jpg' }
    ];

    var cities = getCities();

    var suggestions = [];

    cities.forEach(function (city) {
        restaurants.filter(r => r.city === city.name).forEach(function (restaurant) {
            var suggestion = {};
            suggestion.name = restaurant.name;
            suggestion.type = 'restaurant';
            suggestion.description = restaurant.description;
            suggestion.latitude = restaurant.coordinate.latitude;
            suggestion.longitude = restaurant.coordinate.longitude;
            suggestion.rating = getRandom(4, 5);
            suggestion.votes = getRandom(2500, 3000);
            suggestion.picture = restaurant.picture;
            suggestion.createdAt = new Date();
            suggestion.updatedAt = new Date();
            suggestions.push(suggestion);
        });
    });

    return suggestions;
};



var getSuggestions = function () {
    var suggestions = getRestaurants();
    var id = 1;
    suggestions.forEach(function (suggestion) {
        suggestion.id = id++;
    });
    return suggestions;
};

var deleteSuggestion = function (queryInterface) {
    return queryInterface.bulkDelete('Suggestions');
};

var addSuggestions = function (queryInterface) {
    var suggestions = getSuggestions();
    return queryInterface.bulkInsert('Suggestions', suggestions, {});
};

module.exports = {
    up: (queryInterface, Sequelize) => {
        return deleteSuggestion(queryInterface)
            .then(function () {
                return addSuggestions(queryInterface);
            });
    },

    down: (queryInterface, Sequelize) => {
        /*
          Add reverting commands here.
          Return a promise to correctly handle asynchronicity.
    
          Example:
          return queryInterface.bulkDelete('Person', null, {});
        */
    }
};
