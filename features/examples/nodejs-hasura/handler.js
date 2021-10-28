'use strict'

const bodyParser = require('body-parser')

module.exports = ({ app }, wrap) => {
  app.use(bodyParser.json())
  app.post('/', wrap(handler))
}

const handler = async input => {
  if(input.value > 100) {
    throw '\'value\' is too high'
  }
  
  return {
    value: input.value
  }
}
