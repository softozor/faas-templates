using Microsoft.AspNetCore.Builder;
using HasuraFunction;
using Microsoft.Extensions.DependencyInjection;
using Softozor.HasuraHandling.ConfigurationManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddConfigurationManagement();

BuilderSetup.Configure(builder);

await using var app = builder.Build();

AppSetup.Configure(app);

await app.RunAsync();