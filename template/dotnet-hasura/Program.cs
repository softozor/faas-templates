using HasuraFunction;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

BuilderSetup.Configure(builder);

await using var app = builder.Build();

AppSetup.Configure(app);

await app.RunAsync();