using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using BuildingBlocks.Behaviors;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Middleware;

public class IpLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IpLoggingMiddleware> _logger;


    public IpLoggingMiddleware(RequestDelegate next, ILogger<IpLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var request = context.Request;
        var method = request.Method;
        var url = request.Path + request.QueryString;
        var ipRequest = request.HttpContext.Connection.RemoteIpAddress;
        var trackId = context.TraceIdentifier;

        string inputParam = "{}";
        if (method == HttpMethod.Get.ToString())
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
            {
                var paramsFromClient = await reader.ReadToEndAsync();
                inputParam = paramsFromClient.Trim();
                request.Body.Position = 0;
            }
        }

        try
        {
            await _next(context);
            stopwatch.Stop();
            _logger.LogInformation(
                "{Timespan} - INFO - {Url} - {TrackId} - IP:{IpAddress} - [{Method}] - {ExecutionTime}ms - InputParams:{InputParams}",
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                url,
                trackId,
                ipRequest,
                method,
                stopwatch.ElapsedMilliseconds,
                inputParam);

        }
        catch (Exception e)
        {
            await _next(context);
            stopwatch.Stop();
            _logger.LogError(
                "{Timespan} - ERROR - {Url} - {TrackId} - IP:{IpAddress} - [{Method}] - {ExecutionTime}ms - Error:{error} - InputParams:{InputParams}",
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                url,
                trackId,
                ipRequest,
                method,
                stopwatch.ElapsedMilliseconds,
                e.Message,
                inputParam);
        }
    }
}