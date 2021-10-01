using Softozor.HasuraHandling.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Softozor.HasuraHandling.Controller
{
  public abstract class HasuraControllerBase : ControllerBase
  {
    protected readonly ILogger _logger;

    protected HasuraControllerBase(ILogger logger)
    {
      _logger = logger;
    }

    protected async Task<IActionResult> TryToHandle(Func<Task<IActionResult>> callback)
    {
      try
      {
        return await callback();
      }
      catch (UnableToHandleException ex)
      {
        _logger.LogWarning($"Caught UnableToLoginException: {ex}");

        return Unauthorized(new ActionErrorResponse
        {
          Code = StatusCodes.Status401Unauthorized.ToString(),
          Message = "Unauthorized access"
        });
      }
      catch (GraphqlException ex)
      {
        _logger.LogWarning($"Caught GraphqlException: {ex}");

        return StatusCode(StatusCodes.Status500InternalServerError, new ActionErrorResponse
        {
          Code = StatusCodes.Status500InternalServerError.ToString(),
          Message = ex.Message
        });
      }
      catch (FormatException ex)
      {
        _logger.LogWarning($"Caught FormatException: {ex}");

        return BadRequest(new ActionErrorResponse
        {
          Code = StatusCodes.Status400BadRequest.ToString(),
          Message = "Unauthorized access"
        });
      }
      catch (Exception ex)
      {
        _logger.LogError($"Caught generic Exception: {ex}");

        return StatusCode(StatusCodes.Status500InternalServerError, new ActionErrorResponse
        {
          Code = StatusCodes.Status500InternalServerError.ToString(),
          Message = ex.Message
        });
      }
    }
  }
}
