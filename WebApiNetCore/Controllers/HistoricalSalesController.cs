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
    public class HistoricalSalesController : ControllerBase
    {
        // GET: api/<HistoricalSalesController>
        [HttpGet("Type/")]
        public IActionResult Get(string imei, string type, string cardCode, string fecini, string fecfin)
        {
            try
            {
                ListHistoricalSalesBO records = new ListHistoricalSalesBO();
                HistoricalSalesDAL record = new HistoricalSalesDAL();
                records = record.GetHistoricalType(imei, type, cardCode, fecini, fecfin);
                return Ok(records);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpGet("Variable/")]
        public IActionResult Get1(string imei, string fecini, string fecfin)
        {
            try
            {
                ListHistoricalSalesBO records = new ListHistoricalSalesBO();
                HistoricalSalesDAL record = new HistoricalSalesDAL();
                records = record.GetHistoricalVariable(imei, fecini, fecfin);
                return Ok(records);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
    }
}
