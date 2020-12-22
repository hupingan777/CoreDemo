using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServicesCollection.Tool.Cache;

namespace ServicesCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICache _cache;
        private IConfiguration _configuration;

        public TestController(ICache cache,
            IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        [Route("GetRedisValues")]
        [HttpGet]
        public string GetRedisValues()
        {
            var result = _cache.Get().ToString();
            var configurationResult = _configuration["Logging:LogLevel:Default"].ToString();
            return result;
        }
    }
}
