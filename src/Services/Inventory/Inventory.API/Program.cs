using BuildingBlocks.Middleware;
using Inventory.API.DependencyInjection.Extensions;
using Inventory.Application.DependencyInjection.Extensions;
using Inventory.Infrastructure.DependencyInjection.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilogConfiguration(builder.Configuration);

builder.Services
    .AddApplicationService(builder.Configuration)
    .AddInfrastructureService(builder.Configuration)
    .AddApiService(builder.Configuration)
    .RegisterRepository()
    .RegisterService();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseMiddleware<IpLoggingMiddleware>();
app.UseApiService();
app.UseApplicationService();
app.UseInfrastructureService();



try
{
    Log.Information("Application Starting");
    await app.RunAsync();
}catch(Exception e)
{
    Log.Fatal(e, "Application start failed");
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}
