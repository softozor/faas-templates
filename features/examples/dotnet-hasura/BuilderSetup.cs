using HasuraFunction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Softozor.HasuraHandling.ConfigurationManagement;

public static class BuilderSetup
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services.AddConfigurationManagement();
        builder.Services.AddTransient<Handler, Handler>();
    }
}
