using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServicesCollection.Tool.Cache;
using System.Collections.Generic;
using System.Text;

namespace ServicesCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TestController> _logger;

        public TestController(ICache cache,
            IConfiguration configuration,
            ILogger<TestController> logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }

        [Route("GetRedisValues")]
        [HttpGet]
        public string GetRedisValues()
        {
            var result = _cache.GetAllHashKeysAndValues("MyNovel:Author:GetIdByName:");
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append($"key:{item.Key},value:{item.Value}");
            }
            _logger.LogInformation(sb.ToString());
            return sb.ToString();
        }
    }
}
