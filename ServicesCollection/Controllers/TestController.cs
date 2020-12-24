using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServicesCollection.Tool.Cache;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TestController> _logger;


        /// <summary>
        /// ConfigureServices方法内需要把HttpClient服务添加到依赖注入容器当中:services.AddHttpClient();
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;

        public TestController(ICache cache,
            IConfiguration configuration,
            ILogger<TestController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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

        /// <summary>
        /// IHttpClientFactory的运用
        /// </summary>
        /// <returns></returns>
        [Route("HttpClientFactoryTest")]
        [HttpGet]
        public async Task<string> HttpClientFactoryTest()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient.GetStringAsync("https://www.baidu.com/");
            return result;
        }
    }
}
