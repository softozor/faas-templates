using HasuraHandling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HasuraHandling.ConfigurationManagement
{
  public static class ConfigurationManagementServiceCollectionExtensions
  {
    public static IServiceCollection AddConfigurationManagement(this IServiceCollection services)
    {
      return services
        .AddSingleton<ISecretReader, SecretReader>();
    }
  }
}
