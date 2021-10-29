using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HasuraFunction;

public static class BuilderSetup
{
    public static void Configure(WebApplicationBuilder builder)
    {
        // here you add your services through builder.Services
        builder.Services.AddTransient<IHandler<Input, Output>, Handler>();
    }
}
