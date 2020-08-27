using System.Threading.Tasks;

namespace HasuraHandling.Interfaces
{
  public interface IEventHandler<InputType, OutputType>
  {
    Task<OutputType> Handle(InputType oldRow, InputType newRow);
  }
}
