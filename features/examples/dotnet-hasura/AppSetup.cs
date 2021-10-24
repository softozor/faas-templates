using Microsoft.AspNetCore.Builder;
using Softozor.HasuraHandling.Data;

namespace HasuraFunction;

public static class AppSetup
{
    public static void Configure(WebApplication app)
    {
        app.MapPost("/", (ActionRequestPayload<Input> payload) => new Output
        {
            Value = payload.Input.Value
        });
    }
}