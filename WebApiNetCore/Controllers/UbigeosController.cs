using Microsoft.AspNetCore.Mvc;
using System;
using SAP_Core.BO;
using SAP_Core.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbigeosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                else
                {
                    UbigeoDAL ubigeoDAL = new();
                    ListaUbigeo ubigeos = ubigeoDAL.getUbigeo(imei);

                    return Ok(ubigeos);
                }
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
    }
}
