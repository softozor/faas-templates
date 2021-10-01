namespace Softozor.HasuraHandling.Data
{
  using Newtonsoft.Json;
  
  public class ActionErrorResponse
  {
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }
  }
}
