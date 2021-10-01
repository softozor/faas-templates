using Newtonsoft.Json;
using System;

namespace Softozor.HasuraHandling.Data
{
  public class HasuraSessionVariables
  {
    [JsonProperty("x-hasura-user-id")]
    public Guid UserId { get; set; }

    [JsonProperty("x-hasura-role")]
    public string Role { get; set; }

    [JsonProperty("x-hasura-admin-secret")]
    public string AdminSecret { get; set; }
  }
}
