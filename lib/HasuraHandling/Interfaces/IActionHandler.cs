using System.Threading.Tasks;

namespace HasuraHandling.Interfaces
{
  public interface IActionHandler<InputType, OutputType>
  {
    Task<OutputType> Handle(InputType input);
  }
}
