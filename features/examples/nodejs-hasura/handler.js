'use strict'

const bodyParser = require('body-parser')

module.exports = ({ app }, wrap) => {
  app.use(bodyParser.json())
  app.post('/', wrap(handler))
}

const handler = async input => {
  if(!('value' in input)) {
    throw 'input has no \'value\' property'
  }
  
  return {
    value: input.value
  }
}
