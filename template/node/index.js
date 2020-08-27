'use strict'

const express = require('express')
const app = express()
const handler = require('./function/handler');
const hasura = require('./hasura')

async function init() {
  await handler({ 'app': app }, hasura.handler);

  const port = process.env.http_port || 3000;
  app.disable('x-powered-by');

  app.listen(port, () => {
    console.log(`listening on port: ${port}`)
  });
}

init();