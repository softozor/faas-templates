using System.Threading.Tasks;

namespace Softozor.HasuraHandling.Interfaces
{
  public interface IActionHandler<InputType, OutputType>
  {
    Task<OutputType> Handle(InputType input);
  }
}
