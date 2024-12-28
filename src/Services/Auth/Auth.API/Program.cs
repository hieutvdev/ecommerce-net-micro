using Auth.API;
using Auth.Application;
using Auth.Infrastructure;


var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Add Layer Services
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration)
    .AddApplicationAuthentication(builder.Configuration);
    

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseApiServices();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
