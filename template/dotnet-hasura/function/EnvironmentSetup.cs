using HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HasuraFunction
{
  public class EnvironmentSetup : IEnvironmentSetup
  {
    private readonly IWebHostEnvironment _env;

    public EnvironmentSetup(IWebHostEnvironment env)
    {
      _env = env;
    }

    public bool IsDevelopment()
    {
      return _env.IsDevelopment();
    }
  }
}
