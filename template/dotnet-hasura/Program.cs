using System;
using HasuraFunction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

BuilderSetup.Configure(builder);

await using var app = builder.Build();

AppSetup.Configure(app);

await app.RunAsync();