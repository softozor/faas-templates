namespace HasuraFunction;

using HasuraHandling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddHasuraHandling(this IServiceCollection services)
  {
    // TODO: do we really need a singleton here?
    return services
      .AddSingleton<IActionHandler<Input, Output>, HasuraHandler>();
  }
}