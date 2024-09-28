using Microsoft.AspNetCore.Mvc;
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
    public class EffectivenessController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string imei, string startDate, string endDate)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListEffectiveness colores = new ListEffectiveness();
                EffectivenessDAL color = new EffectivenessDAL();
                colores = color.GetEffectiveness(imei, startDate, endDate);
                return Ok(colores);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");


            }
        }

        [HttpGet("Quote/")]
        public IActionResult EfectividadPorCuotaDiaria(string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }

              
                EffectivenessDAL color = new EffectivenessDAL();
                string despachosJson = "{\"Quotes\":" + color.GetEffectivenessQuote(imei) + "}";
                
                return Ok(despachosJson);

                /*
                 * 
                 * 

                RootDispatch cotizacion = JsonSerializer.Deserialize<RootDispatch>(despachosJson);

                foreach (Dispatch_SAP temp in cotizacion.Dispatch)
                {
                    if (Convert.ToString(temp.Details) != null)
                    {
                        List<Detail> addresses = JsonSerializer.Deserialize<List<Detail>>(temp.Details.ToString());
                        temp.Details = addresses;
                    }
                }



                return Ok(cotizacion);
                 * 
                 * 
                 * 
                 */
            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

    }

}
