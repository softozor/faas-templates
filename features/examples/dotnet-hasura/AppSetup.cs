using HasuraFunction;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Softozor.HasuraHandling;

public static class AppSetup
{
    public static void Configure(WebApplication app)
    {
        app.MapPost("/", async (HttpContext http, Handler handler) =>
            {
                var input = await InputHandling.ExtractActionRequestPayloadFrom<Input>(http);

                try
                {
                    var output = await handler.Handle(input);
                    await http.Response.WriteAsJsonAsync(output);
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