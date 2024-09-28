using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesForce.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.DAL;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuboDeudaController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return null;
               // DeudaDAL agencias = new DeudaDAL();

             //  ResponseData apiResponse = agencias.getCuboDeuda().GetAwaiter().GetResult();
             //   return Ok(apiResponse.Data);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
    }
}
