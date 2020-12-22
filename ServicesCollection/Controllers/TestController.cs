using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesCollection.Tool.Cache;

namespace ServicesCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICache _cache;

        public TestController(ICache cache)
        {
            _cache = cache;
        }

        [Route("GetRedisValues")]
        [HttpGet]
        public string GetRedisValues()
        {
            var result = _cache.Get().ToString();
            return result;
        }
    }
}
