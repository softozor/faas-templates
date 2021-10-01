using Softozor.HasuraFunction;
using Softozor.HasuraHandling.ConfigurationManagement;
using Softozor.HasuraHandling.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  public void ConfigureServices(IServiceCollection services)
  {
    services
      .AddControllers()
      .AddNewtonsoftJson();

    services
      // this is to allow for local debugging
      .Configure<KestrelServerOptions>(options =>
      {
        options.AllowSynchronousIO = true;
      })
      .AddSingleton<IEnvironmentSetup, EnvironmentSetup>()
      .AddConfigurationManagement();

    ServicesConfig.Configure(Configuration, services);
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}