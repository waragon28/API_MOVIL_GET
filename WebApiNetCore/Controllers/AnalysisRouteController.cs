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
    public class AnalysisRouteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string imei, string dia)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }

                ListAnalysisRoute colores = new ListAnalysisRoute();
                AnalysisRouteDAL color = new AnalysisRouteDAL();
                colores = color.GetAnalysis(imei, dia);
                return Ok(colores);

            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda");
            }
        }
        
       
    }



}
