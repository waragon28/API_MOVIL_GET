using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.BO;
using WebApiNetCore.DAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public TestController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        // POST api/<TestController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ListTest value)
        {
            TestDAL tdal = new(_memoryCache);
            ResponseData response = await tdal.getQR(value);
            return Ok(response);
        }
        [HttpPost("BOM")]
        public async Task<IActionResult> Post_BOM([FromBody] ListBoM value)
        {
            TestDAL tdal = new(_memoryCache);
            ResponseData response = await tdal.getBoM(value);
            return Ok(response);
        }

        // PUT api/<TestController>/5
        [HttpGet("GeoLocate")]
        public void getGeoLocate(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
