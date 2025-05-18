using Azure.Core;
using CPMethod.WebAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;

public class SvgService : ISvgService
{
    private readonly IMemoryCache _memoryCache;

    public SvgService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public string SaveSvgToFile(string svgContent)
    {
        var id = Guid.NewGuid().ToString();

        var fileName = $"{Guid.NewGuid()}.svg";

        _memoryCache.Set(id, svgContent, TimeSpan.FromMinutes(20));

        var url = $"https://cpmethodwebapi.lemonpebble-4008942a.polandcentral.azurecontainerapps.io/cpm/svg/{id}";

        return url;
    }
}
