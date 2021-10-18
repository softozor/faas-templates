namespace HasuraFunction;

using Newtonsoft.Json;

public class Output
{
    [JsonProperty("value")]
    public int Value { get; set; }
}

