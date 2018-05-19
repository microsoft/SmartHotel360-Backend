'use strict';
var express = require('express');
var suggestionsQuery = require('../queries/suggestionsQuery');

var router = express.Router();

router.get('/', function (req, res) {
    var latitude = req.query.latitude;
    var longitude = req.query.longitude;

    if (!latitude || !longitude) {
        return res.status(400).send('Latitude and longitude required');
    }

    suggestionsQuery.get({
        latitude: latitude,
        longitude: longitude
    }).then(function (suggestions) {
        res.send(suggestions);
    });
});

module.exports = router;
