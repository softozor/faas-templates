using Function.Data;
using HasuraHandling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HasuraFunction.DemoController
{
  public static class DemoServiceCollectionExtensions
  {
    public static IServiceCollection AddDemoHandling(this IServiceCollection services)
    {
      return services
        .AddSingleton<IActionHandler<DemoInput, DemoOutput>, DemoHandler>();
    }
  }
}
