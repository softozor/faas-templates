# Hasura serverless templates

This repository hosts the [openfaas](https://www.openfaas.com/) function templates that can bind
with [hasura](https://hasura.io/). When we bind hasura actions / events to openfaas functions, some annoying boilerplate
is required, which is made available in this repo for .NET and nodejs.

The templates contained in this repository make wrappers available that make sure the function responses comply
to [the hasura directives](https://hasura.io/docs/2.0/graphql/core/actions/action-handlers.html).

Have a look at the [living documentation](/features) (the `*.feature` files) that relates to
the [examples](/features/examples).
