namespace HasuraFunction;

using HasuraHandling.Controller;
using HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[Route("[controller]")]
[ApiController]
public class HasuraController : ActionControllerBase<Input, Output>
{
    public HasuraController(
      IActionHandler<Input, Output> handler,
      ILogger<HasuraController> logger
    ) : base(handler, logger)
    {
    }
}
