namespace HasuraFunction;

using HasuraHandling.Controller;
using HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[Route("[controller]")]
[ApiController]
public class Controller : ActionControllerBase<Input, Output>
{
    public Controller(
      IActionHandler<Input, Output> handler,
      ILogger<Controller> logger
    ) : base(handler, logger)
    {
    }
}
