using Function.Data;
using HasuraHandling.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HasuraFunction.DemoController
{
  public class DemoHandler : IActionHandler<DemoInput, DemoOutput>
  {
    private readonly ILogger<DemoHandler> _logger;

    public DemoHandler(
      ILogger<DemoHandler> logger
    )
    {
      _logger = logger;
    }

    public async Task<DemoOutput> Handle(DemoInput input)
    {
      _logger.LogInformation($"Calling demo handler");

      return new DemoOutput();
    }
  }
}
