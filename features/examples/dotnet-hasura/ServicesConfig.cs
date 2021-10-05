namespace HasuraFunction;

using Microsoft.Extensions.DependencyInjection;

public static class ServicesConfig
{
    public static void Configure(IServiceCollection services)
    {
      services.AddHasuraHandling();
    }
}
