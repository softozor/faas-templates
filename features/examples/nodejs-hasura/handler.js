'use strict'

const bodyParser = require('body-parser')

module.exports = ({ app }, wrap) => {
  app.use(bodyParser.json())
  app.post('/', wrap(handler))
}

const handler = async input => {
  if(input.value > 100) {
    throw `too high value ${input.value}`
  }
  
  return {
    value: input.value
  }
}
