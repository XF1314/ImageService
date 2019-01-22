using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ft.ImageServer.Core.Domain;
using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ft.ImageServer.Host.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _clientFactory;
        private readonly List<MongoDBHostConfig> _mongoDBHostConfigs;
        private readonly IImageMetadataRepository _imageMetadataRepository;

        public ValuesController(IImageMetadataRepository imageMetadataRepository, 
            IHttpClientFactory clientFactory, IServiceProvider serviceProvider,
            IOptions<List<MongoDBHostConfig>> mongoDBHostConfigs, ILogger<ValuesController> logger,IImageService imageService)
        {
            _imageMetadataRepository = imageMetadataRepository;
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
