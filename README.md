# Softozor hasura serverless templates

This repository hosts the openfaas function templates that we use in softozor projects. All the functions we use are bound to hasura actions / events, therefore they need special boilerplate.

## Using the templates

The templates contained in this repository make wrappers available that make sure the function responses comply to [the hasura directives](https://hasura.io/docs/1.0/graphql/core/actions/action-handlers.html).

### NodeJS

Our NodeJS template is very easy to use (cf. [our mailhog client functions project](https://gitlab.hidora.com/softozor/shopozor/services/-/tree/master/backend/functions/MailhogClient) for a more elaborate example):

1. In your openfaas stack file, declare your function project like this:
```yaml
version: 1.0
provider:
  name: openfaas
  gateway: ${OPENFAAS_GATEWAY_URL:-http://localhost:31112}
functions:
  # this example features the shopozor mailhog-client function which we wrote in javascript
  mailhog-client:
    namespace: ${OPENFAAS_NAMESPACE:-dev}
    lang: node-hasura
    handler: ./MailhogClient
    image: shopozor/mailhog-client-fn:${IMG_TAG:-latest}
    environment:
      MAIL_SERVER_API_PORT: ${MAIL_SERVER_API_PORT:-8025}
      MAIL_SERVER_HOST: ${MAIL_SERVER_HOST:-mailhog.dev}
configuration:
  templates: # this section is critical if you want openfaas to access our templates
    - name: node-hasura
      source: https://github.com/shopozor/faas-templates
```

2. Use the built-in handler function packed in this template's `handler.js` source.

3. Bind your hasura action with the following url (cf. [the default handler](/template/node-hasura/function/handler.js)):
```
http://{{OPENFAAS_GATEWAY_URL}}/function/mailhog-client.{{FUNCTION_NAMESPACE}}/
```
In the case of the default handler, the `/` is the route to the handler, therefore the action url above features that `/` route.