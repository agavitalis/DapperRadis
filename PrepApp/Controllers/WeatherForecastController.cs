using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache
         )
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            string value = string.Empty;
            _memoryCache.TryGetValue("key", out value);
            Console.WriteLine(value);

            var readFromRedis =  _distributedCache.Get("key");

            var cacheOptions = new DistributedCacheEntryOptions
            {

                AbsoluteExpiration = DateTime.Now.AddMilliseconds(500),
                SlidingExpiration = TimeSpan.FromMinutes(2),

            };
            JsonConvert.SerializeObject("emeka");
            var preparedData = Encoding.UTF8.GetBytes("value");

            _distributedCache.Set("key", preparedData, cacheOptions);



            ///setting cache
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {

                AbsoluteExpiration = DateTime.Now.AddMilliseconds(500),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 1024
            };

            _memoryCache.Set("key", "value", cacheExpiryOptions);

         


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
