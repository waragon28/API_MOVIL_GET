using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesForce.BO;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KardexController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public KardexController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Get(string CardCode)
        {
            try
            {
                ListaKardex kardex = new ListaKardex();
                KardexDAL kardexDAL = new(_memoryCache);
                kardex = kardexDAL.getKardex(CardCode);
                return Ok(kardex);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        /*[HttpPost("Validate/")]
        public IActionResult Post([FromBody] JObject s)
        {
            dynamic obj = JsonConvert.DeserializeObject(s.ToString());
            string imei = obj["Imei"];
            string dia = obj["Dia"];

            KardexDAL kardexDAL = new (_memoryCache);
            ResponseData apiResponse= kardexDAL.getValidationCliente(imei, dia).GetAwaiter().GetResult();

            if (apiResponse.StatusCode==HttpStatusCode.OK)
            {

            }
            else
            {
                return StatusCode((int)apiResponse.StatusCode);
            }            

        }*/
    }
}
