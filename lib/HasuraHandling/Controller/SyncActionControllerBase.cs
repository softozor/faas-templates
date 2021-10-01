using Softozor.HasuraHandling.Data;
using Softozor.HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Softozor.HasuraHandling.Controller
{
  public abstract class SyncActionControllerBase<InputType, OutputType> : SyncHasuraControllerBase
    where InputType : class
    where OutputType : class
  {
    protected readonly ISyncActionHandler<InputType, OutputType> _handler;

    protected SyncActionControllerBase(
      ISyncActionHandler<InputType, OutputType> handler,
      ILogger logger
    ) : base(logger)
    {
      _handler = handler;
    }

    [HttpPost]
    [Consumes("application/json")]
    public IActionResult Post([FromBody] ActionRequestPayload<InputType> input) => DoPost(Handle, input.Input);

    protected IActionResult DoPost(Func<InputType, OutputType> handlerCallback, InputType input)
    {
      return TryToHandle(() =>
      {
        var result = handlerCallback(input);
        return Ok(result);
      });
    }

    protected OutputType Handle(InputType input) => _handler.Handle(input);
  }
}
