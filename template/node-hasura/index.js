'use strict'

const express = require('express')
const app = express()
const handler = require('./function/handler')
const hasuraWrapper = require('./hasura')

async function init() {
  await handler({ app }, hasuraWrapper)

  const port = process.env.http_port || 3000
  app.disable('x-powered-by')

  app.listen(port, () => {
    console.log(`listening on port: ${port}`)
  })
}

// do not await this call
init()
