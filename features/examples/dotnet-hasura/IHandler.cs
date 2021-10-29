using System.Threading.Tasks;

namespace HasuraFunction;

public interface IHandler<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    Task<TOutput> Handle(TInput input);
}