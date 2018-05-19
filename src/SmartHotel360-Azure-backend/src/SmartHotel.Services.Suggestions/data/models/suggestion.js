'use strict';
module.exports = (sequelize, DataTypes) => {
  var Suggestion = sequelize.define('Suggestion', {
    name: DataTypes.STRING,
    type: DataTypes.STRING,
    latitude: DataTypes.DOUBLE,
    longitude: DataTypes.DOUBLE,
    description: DataTypes.TEXT,
    rating: DataTypes.INTEGER,
    votes: DataTypes.INTEGER,
    picture: DataTypes.STRING
  }, {
    classMethods: {
      associate: function(models) {
        // associations can be defined here
      }
    }
  });
  return Suggestion;
};