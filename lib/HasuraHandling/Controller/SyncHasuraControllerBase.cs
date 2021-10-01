using Softozor.HasuraHandling.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Softozor.HasuraHandling.Controller
{
  public abstract class SyncHasuraControllerBase : ControllerBase
  {
    protected readonly ILogger _logger;

    protected SyncHasuraControllerBase(ILogger logger)
    {
      _logger = logger;
    }

    protected IActionResult TryToHandle(Func<IActionResult> callback)
    {
      try
      {
        return callback();
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
