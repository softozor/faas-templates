using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Softozor.HasuraHandling;
using Softozor.HasuraHandling.Interfaces;

namespace HasuraFunction;

public static class AppSetup
{
    public static void Configure(WebApplication app)
    {
        app.MapPost("/", async (HttpContext http, IActionHandler<Input, Output> handler) =>
            {
                Func<Input, Task<Output>> handle = handler.Handle;
                await ActionHandlerWrapper.HandleAsync(http, handle);
            }
        );
    }
}