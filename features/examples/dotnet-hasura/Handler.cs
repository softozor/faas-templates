using System.Globalization;
using System.Threading.Tasks;
using Softozor.HasuraHandling.Exceptions;
using Softozor.HasuraHandling.Interfaces;

namespace HasuraFunction;

public class Handler : IActionHandler<Input, Output>
{
    public async Task<Output> Handle(Input input)
    {
        var value = input.Value;

        if(value > 100)
        {
            throw new HasuraFunctionException($"too high value {value.ToString(CultureInfo.InvariantCulture)}", 400);
        }

        return await Task.FromResult(new Output
        {
            Value = value
        });
    }
}