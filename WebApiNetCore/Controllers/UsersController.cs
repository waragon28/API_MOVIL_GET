using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAP_Core.BO;
using SAP_Core.DAL;
using Sentry;
using SalesForce.BO;
using SAP_Core.Utils;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult Get([FromQuery]string imei)
        {
            try
            {
                if (imei == null || imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }

                UsuarioDAL usuarioDAL = new();
                ListUsers user = usuarioDAL.getUserCL(imei);

                if (user.Users.Count() == 0)
                    return BadRequest("No hay datos para mostrar");
                return Ok(user);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda "+e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Acceso acceso)
        {

            UsuarioDAL usuarioDAL = new();
            ResponseLoginV2 responseLogin =usuarioDAL.login(acceso).GetAwaiter().GetResult();

            return new JsonResult(responseLogin)
            {
                StatusCode = (int)responseLogin.StatusCode
            };
        }


    }
}
