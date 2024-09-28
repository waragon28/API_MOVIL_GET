using Microsoft.AspNetCore.Mvc;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraceabilitiesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string imei, string fecha)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListTraceability traceability = new ListTraceability();
                TraceabilityDAL traceabilityDAL = new TraceabilityDAL();
                traceability = traceabilityDAL.GetTraceability(imei, fecha);
                return Ok(traceability);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpGet("route")]
        public IActionResult GetRoute(string imei, string fecha)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListSalesRoute traceability = new ListSalesRoute();
                TraceabilityDAL traceabilityDAL = new TraceabilityDAL();
                traceability = traceabilityDAL.GetSalesRoute(imei, fecha);
                return Ok(traceability);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
    }
}
