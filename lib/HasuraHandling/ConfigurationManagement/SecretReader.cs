using Softozor.HasuraHandling.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Softozor.HasuraHandling.ConfigurationManagement
{
  public class SecretReader : ISecretReader
  {
    private readonly ILogger<SecretReader> _logger;

    public SecretReader(ILogger<SecretReader> logger)
    {
      _logger = logger;
    }

    public string GetSecret(string secretName)
    {
      try
      {
        return File.ReadAllText($"/var/openfaas/secrets/{secretName}").Trim();
      }
      catch (Exception)
      {
        _logger.LogCritical($"Secret {secretName} could not be read");
        return string.Empty;
      }
    }
  }
}
