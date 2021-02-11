using HasuraHandling.Data;
using HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HasuraHandling.Controller
{
  public abstract class ActionControllerBase<InputType, OutputType> : HasuraControllerBase
    where InputType : class
    where OutputType : class
  {
    protected readonly IActionHandler<InputType, OutputType> _handler;

    protected ActionControllerBase(
      IActionHandler<InputType, OutputType> handler,
      ILogger logger
    ) : base(logger)
    {
      _handler = handler;
    }

    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Post([FromBody] ActionRequestPayload<InputType> input) => await DoPost(Handle, input.Input);

    protected async Task<IActionResult> DoPost(Func<InputType, Task<OutputType>> handlerCallback, InputType input)
    {
      return await TryToHandle(async () =>
      {
        var result = await handlerCallback(input);
        return Ok(result);
      });
    }

    protected Task<OutputType> Handle(InputType input) => _handler.Handle(input);
  }
}
