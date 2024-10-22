using Microsoft.AspNetCore.Mvc;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.DAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        CorreoAlert correoAlert = new CorreoAlert();

        [HttpGet]
        public IActionResult Get(string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListAgencia agencias = new ListAgencia();
                AgenciaDAL agencia = new AgenciaDAL();
                agencias = agencia.GetAgencias(imei);
                return Ok(agencias);
            }
            catch (Exception ex)
            {
                //Log.save(this, e.Message);
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Agencies Controller Vistony", ex.Message.ToString());
                return BadRequest("No se pudo concluir la busqueda " + ex.Message.ToString());
            }
            finally
            {

            }
        }
    }
}
