# OpenFaaS Shopozor templates

This repository hosts the openfaas function templates that we use in the shopozor project. All the functions we use are bound to hasura actions, therefore they need special boilerplate. We have serverless functions written in nodejs and .net core.

## Using the templates

### NodeJS

### .Net Core

## Updating the templates

### .Net Core

We build nuget packages that we use both in the template and the function code. 

After you've made some modifications to the code, you need to publish the nuget packages like this:

1. make sure you have a nuget api key (connect to your nuget account [here](https://www.nuget.org/users/account/LogOn) and do [whatever is necessary](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-the-dotnet-cli))

2. in each of the modified libs, update the version numbers:

![nuget package version numbers](/doc/img/nuget-update.png)

3. at the root of this repository, run
```bash
./scripts/publish_dotnet_packages.sh YOUR_API_KEY
```

Make sure both the template and your function code use the same version of those nuget packages, otherwise you will have some conflicts.