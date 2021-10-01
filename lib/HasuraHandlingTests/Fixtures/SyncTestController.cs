using Softozor.HasuraHandling.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Softozor.HasuraHandlingTests.Fixtures
{
  public class SyncTestController : SyncHasuraControllerBase
  {
    public SyncTestController(ILogger logger) : base(logger)
    {
    }

    public IActionResult TestPost(Func<IActionResult> callback)
    {
      return TryToHandle(() =>
      {
        var result = callback();
        return Ok(result);
      });
    }
  }
}
