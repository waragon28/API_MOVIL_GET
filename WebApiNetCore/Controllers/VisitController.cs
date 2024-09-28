using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SalesForce.BO;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : Controller
    {
        private IMemoryCache _memoryCache;
        public VisitController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpPost]
        public IActionResult insertVisit([FromBody] JsonElement s, [FromHeader] string Token)
        {
            VisitDAL visitDAL = new(_memoryCache);
            //string jsonString = JsonSerializer.Serialize(s);
            dynamic obj = JsonConvert.DeserializeObject(s.ToString());
            dynamic list = obj["Visits"];
            /*VisitsBO v = new VisitsBO();
            v = JsonSerializer.Deserialize<VisitsBO>(jsonString);
            List<VisitBO> lv = v.Visits;*/

            ResponseData apiResponse = visitDAL.insert_test(list).GetAwaiter().GetResult();
            return Ok(apiResponse.Data);

        }

        [HttpGet("Validate/")]
        public IActionResult GetValidVisit(string slpCode, string fecha, string visitID, string cardCode)
        {
            try
            {
                VisitResponse rr = new VisitResponse();
                VisitDAL cobranzas = new VisitDAL(_memoryCache);
                rr = cobranzas.GetValidateVisit(slpCode, fecha, visitID, cardCode).GetAwaiter().GetResult();

                if (rr != null)
                {
                    return Ok(rr);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda " + e);
            }

        }

    }
}
