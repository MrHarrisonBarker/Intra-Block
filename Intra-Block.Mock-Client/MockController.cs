using System.Text;
using Intra_Block.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Intra_Block.Mock_Client
{
    [ApiController]
    [Route("api/[controller]")]
    public class MockController : ControllerBase
    {
        private readonly ILogger<MockController> Logger;
        private readonly IntraCache Cache;

        public MockController(ILogger<MockController> logger, IntraCache cache)
        {
            Logger = logger;
            Cache = cache;
        }

        [HttpGet]
        public ActionResult Insert(string key, string value)
        {
            Logger.LogInformation($"Inserting \"{key}\"");

            Cache.Set(key, Encoding.UTF8.GetBytes(value));

            return Ok();
        }

        [HttpGet]
        public ActionResult Retrieve(string key)
        {
            Logger.LogInformation($"Retrieving \"{key}\"");

            Cache.Get(key);

            return Ok();
        }
        
        [HttpGet]
        public ActionResult Remove(string key)
        {
            Logger.LogInformation($"Removing \"{key}\"");

            Cache.Remove(key);

            return Ok();
        }
    }
}