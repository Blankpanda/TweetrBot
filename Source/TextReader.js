// Javascript file reader to pass onto BotFunctions.js so the bot tweets the contents of a text file

var fs = require('fs');

exports.data = function(file) {

var lines = fs.readFileSync(file).toString().split("\n");

    return lines.join(" ");
    
};