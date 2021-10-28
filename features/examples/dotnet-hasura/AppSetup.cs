using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Softozor.HasuraHandling.Data;

namespace HasuraFunction;

public static class AppSetup
{
    public static void Configure(WebApplication app)
    {
        app.MapPost("/", async http => 
        {
            var payload = await http.Request.ReadFromJsonAsync<ActionRequestPayload<Input>>();

            var value = payload.Input.Value;
        
            if(value > 100)
            {
                http.Response.StatusCode = 400;
                return;
            }
        
            var output = new Output
            {
                Value = value
            };
            
            await http.Response.WriteAsJsonAsync(output);
        });
    }
}