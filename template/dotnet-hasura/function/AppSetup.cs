using Microsoft.AspNetCore.Builder;

namespace HasuraFunction;

public static class AppSetup
{
    public static void Configure(WebApplication app)
    {
        // here you register your endpoints with app.MapPost(...)
    }
}