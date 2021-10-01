using Newtonsoft.Json;
using System;

namespace Softozor.HasuraHandling.Data
{
  public class EventRequestPayload<InputType> where InputType : class
  {
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("trigger")]
    public Trigger Trigger { get; set; }

    [JsonProperty("table")]
    public Table Table { get; set; }

    [JsonProperty("event")]
    public HasuraEvent<InputType> Event { get; set; }
  }

  public class Trigger
  {
    [JsonProperty("name")]
    public string Name { get; set; }
  }

  public class Table
  {
    [JsonProperty("schema")]
    public string Schema { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
  }

  public class HasuraEvent<InputType> where InputType : class
  {
    [JsonProperty("session_variables")]
    public HasuraSessionVariables SessionVariables { get; set; }

    private Op _op;
    [JsonProperty("op")]
    public string Op
    {
      get => _op.ToString();
      set
      {
        _op = (Op)Enum.Parse(typeof(Op), value);
      }
    }

    [JsonProperty("data")]
    public EventData<InputType> Data { get; set; }
  }

  public enum Op
  {
    INSERT,
    UPDATE,
    DELETE,
    MANUAL
  }

  public class EventData<InputType> where InputType : class
  {
    [JsonProperty("old")]
    public InputType Old { get; set; }

    [JsonProperty("new")]
    public InputType New { get; set; }
  }
}
