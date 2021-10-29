using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Softozor.HasuraHandling;

namespace HasuraFunction;

public static class AppSetup
{
    public static void Configure(WebApplication app)
    {
        app.MapPost("/", async (HttpContext http, IHandler<Input, Output> handler) =>
            {
                Func<Input, Task<Output>> handle = handler.Handle;
                await HandlerWrapper.HandleAsyncAction(http, handle);
            }
        );
    }
}