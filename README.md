# OpenFaaS Shopozor templates

This repository hosts the openfaas function templates that we use in the shopozor project. All the functions we use are bound to hasura actions / events, therefore they need special boilerplate. We have serverless functions written in nodejs and .net core. Our templates support the shopozor architecture. You can find [examples here](https://gitlab.hidora.com/softozor/shopozor/services/-/tree/master/backend/functions).

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

### .Net Core

**CAUTION** This template deploys a docker container with root user. This is something we need to fix some time later (cf. [this issue](https://gitlab.hidora.com/softozor/shopozor/services/-/issues/324)).

The shopozor needs a lot of functions. Of course, we don't write one .net project for each function, because it would be far too heavy. However, we pack functions that belong together within the same project, so that we comply to the single responsibility principle. To support multiple function projects, we need to diverge a bit from the usual faas template code. That is because otherwise it is not trivial to have a simple `Dockerfile`. To keep things simple, we don't include handler function code and handler function project definition in our template. We therefore have neither `FunctionHandler.cs` nor `FunctionHandler.csproj`. However we have `EnvironmentSetup.cs` and `ServicesConfig.cs` classes in our `function` folder. The former is just there to make some environment data available (like the `IsDevelopment` flag). The latter provides a way to fill the DI container with your function-specific services. It is used in the `Startup` class located at the root of the `dotnet-hasura` template.

Let's see how our template can be used (cf. [our shopping functions project](https://gitlab.hidora.com/softozor/shopozor/services/-/tree/master/backend/functions/Shopping) for a more elaborate example):

1. In your openfaas stack file, declare your function project like this:
```yaml
version: 1.0
provider:
  name: openfaas
  gateway: ${OPENFAAS_GATEWAY_URL:-http://localhost:31112}
functions:
  # this example features the shopozor admin function project which we wrote in .Net Core
  admin:
    namespace: ${OPENFAAS_NAMESPACE:-dev}
    lang: dotnet-hasura # this is the template name
    handler: ./Admin
    image: shopozor/admin-fn:${IMG_TAG:-latest}
    build_args:
      FUNCTION_NAME: Admin # this name is used in the Dockerfile
configuration:
  templates: # this section is critical if you want openfaas to access our templates
    - name: dotnet-hasura
      source: https://github.com/shopozor/faas-templates
```
In the shopozor visual studio solution, there are a few function projects like that one. In order to avoid name conflicts, we need to name them differently. We could have multiple `Function.csproj` in that solution, but they would then all be named the same, which is not very handy. For that reason, we name those function projects differently, which is why we need the above `FUNCTION_NAME` variable passed as docker build argument.

2. Write your handler: in the above example, you could add the following code to the `./Admin` folder:
```cs
// ./Admin/Data/DemoInput.cs
namespace Function.Data
{
  public class DemoInput
  {
  }
}

// ./Admin/Data/DemoOutput.cs
namespace Function.Data
{
  public class DemoOutput
  {
  }
}

// ./Admin/DemoController/DemoController.cs
using Function.Data;
using HasuraHandling.Controller;
using HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HasuraFunction.DemoController
{
  [Route("[controller]")]
  [ApiController]
  public class DemoController : ActionControllerBase<DemoInput, DemoOutput>
  {
    public DemoController(
      IActionHandler<DemoInput, DemoOutput> handler,
      ILogger<DemoHandler> logger
    ) : base(handler, logger)
    {
    }
  }
}

// ./Admin/DemoController/DemoHandler.cs
using Function.Data;
using HasuraHandling.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HasuraFunction.DemoController
{
  public class DemoHandler : IActionHandler<DemoInput, DemoOutput>
  {
    private readonly ILogger<DemoHandler> _logger;

    public DemoHandler(
      ILogger<DemoHandler> logger
    )
    {
      _logger = logger;
    }

    public async Task<DemoOutput> Handle(DemoInput input)
    {
      _logger.LogInformation($"Calling demo handler");

      return new DemoOutput();
    }
  }
}

// ./Admin/DemoController/DemoServiceCollectionExtensions.cs
using Function.Data;
using HasuraHandling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HasuraFunction.DemoController
{
  public static class DemoServiceCollectionExtensions
  {
    public static IServiceCollection AddDemoHandling(this IServiceCollection services)
    {
      return services
        .AddSingleton<IActionHandler<DemoInput, DemoOutput>, DemoHandler>();
    }
  }
}
```

3. Connect your action handler with a hasura action on the url 
```
http://{{OPENFAAS_GATEWAY_URL}}/function/admin.{{FUNCTION_NAMESPACE}}/demo
```

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