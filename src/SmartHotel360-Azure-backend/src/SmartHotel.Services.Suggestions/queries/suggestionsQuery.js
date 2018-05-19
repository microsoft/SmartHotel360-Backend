const geolib = require('geolib');
const Suggestion = require('../data/models').Suggestion;

var get = function (location, take) {
    take = take || 10;
    return Suggestion.all({ raw: true }).then(function (suggestions) {
        suggestions.forEach(function (suggestion) {
            var distance = geolib.getDistance(location, suggestion);
            suggestion.distance = distance;
        });

        var nearest = suggestions.sort(function (suggestionA, suggestionB) {
            return suggestionA.distance - suggestionB.distance;
        }).slice(0, take);

        return nearest;
    });
}

module.exports = {
    get: get
};                           