namespace Softozor.HasuraHandling.Interfaces
{
  using System.Threading.Tasks;
  
  public interface IEventHandler<InputType, OutputType>
  {
    Task<OutputType> Handle(InputType oldRow, InputType newRow);
  }
}
