using System.Globalization;
using System.Threading.Tasks;
using Softozor.HasuraHandling.Exceptions;

namespace HasuraFunction;

public class Handler : IHandler<Input, Output>
{
    public async Task<Output> Handle(Input input)
    {
        var value = input.Value;
        
        if(value > 100)
        {
            throw new HasuraFunctionException($"too high value {value.ToString(CultureInfo.InvariantCulture)}");
        }
        
        return await Task.FromResult(new Output
        {
            Value = value
        });
    }
}