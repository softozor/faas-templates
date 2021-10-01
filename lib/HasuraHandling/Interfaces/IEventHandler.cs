using System.Threading.Tasks;

namespace Softozor.HasuraHandling.Interfaces
{
  public interface IEventHandler<InputType, OutputType>
  {
    Task<OutputType> Handle(InputType oldRow, InputType newRow);
  }
}
