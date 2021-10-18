namespace HasuraFunction;

using Newtonsoft.Json;

public class Input
{
    [JsonProperty("value")]
    public int Value { get; set; }
}

