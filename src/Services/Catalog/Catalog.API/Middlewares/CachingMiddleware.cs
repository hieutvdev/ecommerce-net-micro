using System.Text;
using Catalog.API.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Catalog.API.Middlewares;

public class CachingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IResponseCacheService _cacheService;

    public CachingMiddleware(RequestDelegate next, IResponseCacheService cacheService)
    {
        _next = next;
        _cacheService = cacheService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cacheKey = GenerateCacheKeyFromRequest(context.Request);
        var cachedResponse = await _cacheService.GetCacheResponseAsync(cacheKey);
       
        if (!string.IsNullOrEmpty(cachedResponse))
        {
             

            // Second deserialization to get the object
            var jsonResponse = JsonConvert.DeserializeObject<object>(cachedResponse, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            var result = System.Text.Json.JsonSerializer.Deserialize<object>(jsonResponse.ToString());


            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(result);

            return;
        }

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status200OK)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            await _cacheService.SetCacheResponseAsync(cacheKey, responseBodyText, TimeSpan.FromSeconds(1000 * 10));

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }

        return keyBuilder.ToString();
    }
}
