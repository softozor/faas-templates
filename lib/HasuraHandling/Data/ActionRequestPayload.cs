using Newtonsoft.Json;

namespace HasuraHandling.Data
{
  public class ActionRequestPayload<InputType> where InputType : class
  {
    [JsonProperty("action")]
    public HasuraAction Action { get; set; }

    [JsonProperty("input")]
    public InputType Input { get; set; }

    [JsonProperty("session_variables")]
    public HasuraSessionVariables SessionVariables { get; set; }
  }

  public class HasuraAction
  {
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}
