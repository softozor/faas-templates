using Function.Data;
using HasuraHandling.Controller;
using HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HasuraFunction.DemoController
{
  [Route("[controller]")]
  [ApiController]
  public class DemoController : ActionControllerBase<DemoInput, DemoOutput>
  {
    public DemoController(
      IActionHandler<DemoInput, DemoOutput> handler,
      ILogger<DemoHandler> logger
    ) : base(handler, logger)
    {
    }
  }
}
