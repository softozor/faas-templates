namespace Softozor.HasuraHandling.Controller
{
  using Softozor.HasuraHandling.Data;
  using Softozor.HasuraHandling.Interfaces;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using System;
  using System.Threading.Tasks;
  
  public abstract class EventControllerBase<InputType, OutputType> : HasuraControllerBase
    where InputType : class
    where OutputType : class
  {
    protected readonly IEventHandler<InputType, OutputType> _handler;

    protected EventControllerBase(
      IEventHandler<InputType, OutputType> handler,
      ILogger logger
    ) : base(logger)
    {
      _handler = handler;
    }

    // here we await because the controller is synchronous
    // when we know how to do asynchronous hasura actions, then this might change
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Post([FromBody] EventRequestPayload<InputType> input) => await DoPost(Handle, input.Event.Data.Old, input.Event.Data.New);

    protected async Task<IActionResult> DoPost(Func<InputType, InputType, Task<OutputType>> handlerCallback, InputType oldRow, InputType newRow)
    {
      return await TryToHandle(async () =>
      {
        var result = await handlerCallback(oldRow, newRow);
        return Ok(result);
      });
    }

    protected Task<OutputType> Handle(InputType oldRow, InputType newRow) => _handler.Handle(oldRow, newRow);
  }
}
