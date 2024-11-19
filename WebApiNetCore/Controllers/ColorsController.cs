using Microsoft.AspNetCore.Mvc;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        // GET: api/<ColorsController>
        [HttpGet]
        public IActionResult Get(string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListaColores colores = new ListaColores();
                ColoresDAL color = new ColoresDAL();
                colores = color.GetColores(imei);
                return Ok(colores);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }

        [HttpGet("legendColor")]
        public IActionResult GetlegendColor(string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListColorRange colores = new ListColorRange();
                ColoresDAL color = new ColoresDAL();
                colores = color.GetlegendColor(imei);
                return Ok(colores);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpPost("ColorApproval")]
        public IActionResult PostOrderVentaColores(ColorListSalesOrder colorListSalesOrder)
        {
            try
            {
                ColorListSalesOrder coloresD = new ColorListSalesOrder();
                ColoresDAL color = new ColoresDAL();
                coloresD = color.PostOrderVentaColores(colorListSalesOrder);
                return Ok(coloresD);

            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }

    }
}
