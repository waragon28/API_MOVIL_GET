using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiNetCore.BO;
using WebApiNetCore.DAL;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class InspeccionVehiculoController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public InspeccionVehiculoController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        [HttpPost]
        public IActionResult Post([FromBody] JsonElement isnspeccionVehiculo)
        {

            InspeccionVehiculoDAL inspeccionVehiculoDAL = new(_memoryCache);
            string jsonString = JsonSerializer.Serialize(isnspeccionVehiculo);
            ResponseData response = inspeccionVehiculoDAL.inspeccionVehiculo(jsonString).GetAwaiter().GetResult();

            return Ok(response);

        }



    }
}
