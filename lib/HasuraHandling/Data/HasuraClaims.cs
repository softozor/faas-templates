namespace Softozor.HasuraHandling.Data
{
  using Newtonsoft.Json;
  using System;
  
  public class HasuraClaims
  {
    [JsonProperty("x-hasura-allowed-roles")]
    public string[] Roles { get; set; }

    [JsonProperty("x-hasura-default-role")]
    public string DefaultRole { get; set; }

    [JsonProperty("x-hasura-user-id")]
    public Guid UserId { get; set; }
  }
}
