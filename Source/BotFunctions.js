var Twit = require('./node_modules/twit/lib/twitter');
var txtFileReader = require('./TextReader.js');
var xmlFileAsText = 'parsedXML.txt'

var T = new Twit({
    consumer_key:        'n53YwKakvyVGjDgDqbcxN4AlG'
  , consumer_secret:     'gHaatWUr1WHos8d5ULijiULQrVgFU5PgGagfQqqYm0P06JH34b'
  , access_token:        '3294145216-R0kSnLkFRdi3Y6iPgde9dAFaAMA3zDANHySY8Yd'
  , access_token_secret: 'dvIFu7Ipexcpy6DAIJvI6kfmJ1gLgDtkJzmee8KG0rArS'
})



var allLines =  txtFileReader.data(xmlFileAsText);
console.log(allLines);

//
//  tweet allLines
//

T.post('statuses/update', { status: allLines },function(err, data, response) {
  console.log(data)
})