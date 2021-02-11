namespace HasuraHandling.Interfaces
{
  public interface ISyncActionHandler<InputType, OutputType>
  {
    OutputType Handle(InputType input);
  }
}