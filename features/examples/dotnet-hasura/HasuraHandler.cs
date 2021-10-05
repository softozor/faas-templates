namespace HasuraFunction;

using HasuraHandling.Interfaces;
using System.Threading.Tasks;

public class HasuraHandler : IActionHandler<Input, Output>
{
    private readonly ILogger<HasuraHandler> _logger;
    
    public HasuraHandler(
      ILogger<HasuraHandler> logger
    )
    {
      _logger = logger;
    }
    
    public async Task<DemoOutput> Handle(Input input)
    {
      _logger.LogInformation($"Calling hasura handler");
    
      return new Output();
    }
}