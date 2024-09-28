using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesForce.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.BO;
using WebApiNetCore.DAL;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudTrasladoController : ControllerBase
    {
        // GET: api/<ColorsController>
        [HttpPost]
        public IActionResult Post([FromBody] SoliDevBO soliDevBO)
        {
            try
            {
                SolicitudDevolDAL solicitudDevolDAL = new SolicitudDevolDAL();

                ResponseData apiResponse = solicitudDevolDAL.SolicitudDevolucion(soliDevBO).GetAwaiter().GetResult();
                return Ok(apiResponse);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        
    }


}
