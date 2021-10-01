using Softozor.HasuraHandling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Softozor.HasuraHandling.ConfigurationManagement
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddConfigurationManagement(this IServiceCollection services)
    {
      return services
        .AddSingleton<ISecretReader, SecretReader>();
    }
  }
}
