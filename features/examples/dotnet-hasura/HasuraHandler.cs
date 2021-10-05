namespace HasuraFunction;

using HasuraHandling.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class HasuraHandler : IActionHandler<Input, Output>
{
    private readonly ILogger<HasuraHandler> _logger;
    
    public HasuraHandler(
      ILogger<HasuraHandler> logger
    )
    {
      _logger = logger;
    }
    
    public Task<Output> Handle(Input input)
    {
      _logger.LogInformation($"Calling hasura handler");
    
      return Task.FromResult<Output>(new Output());
    }
}