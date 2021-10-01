namespace Softozor.HasuraHandling.Interfaces
{
  using System.Threading.Tasks;
  
  public interface IActionHandler<InputType, OutputType>
  {
    Task<OutputType> Handle(InputType input);
  }
}
