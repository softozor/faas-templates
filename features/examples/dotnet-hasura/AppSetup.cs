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
        app.MapPost("/", async (HttpContext http, Handler handler) =>
            {
                var input = await InputHandling.ExtractActionRequestPayloadFrom<SetPasswordInput>(http);

                try
                {
                    await handler.Handle(input);
                }
                catch (HasuraFunctionException ex)
                {
                    await ErrorReporter.ReportError(http, ex);
                }
                catch (Exception ex)
                {
                    await ErrorReporter.ReportUnexpectedError(http, ex);
                }
            }
        );
    }
}