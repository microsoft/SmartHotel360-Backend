const urlBases = ["http://sh360.<guid>.<region>.aksapp.io/"]

const requestify = require("requestify");

function iterateCities(urlBase) 
{
    for(let i=0; i<30; i++)
    {
        requestify
            .get(urlBase + "hotels-api/Hotels/search?cityId=" + i)
            .then((response) => {
                console.log(response.getBody());
            });
    }
}

function searchForCities(urlBase) {
    var cities = ['Seattle', 'Sea', 'seattle', 'sea'];

    for (let i = 0; i < cities.length; i++) 
    {
        const city = cities[i];
        requestify
            .get(urlBase + "hotels-api/Cities?name=" + city)
            .then((response) => {
                console.log(response.getBody());
            });
    }
}

function getReviews(urlBase) 
{
    for(i=1; i<20; i++) 
    {
        requestify
            .get(urlBase + "hotels-api/Reviews/" + i)
            .then((response) => {
                console.log(response.getBody());
            });
    }
}

function repeatMe() {
    setTimeout(() => {
        for (let i = 0; i < urlBases.length; i++) {
            iterateCities(urlBases[i]);
            searchForCities(urlBases[i]);
            getReviews(urlBases[i]);
        }
        repeatMe();
    }, 10000);
}

repeatMe();