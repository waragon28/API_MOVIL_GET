using Microsoft.AspNetCore.Mvc;
using SAP_Core.BO;
using SAP_Core.DAL;
using Sentry;
using System;
using System.Linq;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPathController : ControllerBase
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

                ListaRutaTrabajo listRutaTrabajo = new();
                RutaTrabajoDAL rutaTrabajoDAL = new();
                listRutaTrabajo = rutaTrabajoDAL.GetRutaTrabajo(imei);

                return Ok(listRutaTrabajo);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

       
    }
}
