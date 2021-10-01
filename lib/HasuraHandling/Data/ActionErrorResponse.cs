using Newtonsoft.Json;

namespace Softozor.HasuraHandling.Data
{
  public class ActionErrorResponse
  {
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }
  }
}
