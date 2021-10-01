namespace Softozor.HasuraHandlingTests.Fixtures
{
  using Softozor.HasuraHandling.Controller;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using System;
  using System.Threading.Tasks;
  
  public class TestController : HasuraControllerBase
  {
    public TestController(ILogger logger) : base(logger)
    {
    }

    public async Task<IActionResult> TestPost(Func<Task<IActionResult>> callback)
    {
      return await TryToHandle(async () =>
      {
        var result = await callback();
        return Ok(result);
      });
    }
  }
}
