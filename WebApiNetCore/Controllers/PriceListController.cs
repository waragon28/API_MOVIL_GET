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
    public class PriceListController : ControllerBase
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

                ListaPrecios listaPrecios = new ListaPrecios();
                ListaPreciosDAL listaPrecioDAL = new ListaPreciosDAL();
                listaPrecios = listaPrecioDAL.GetListaPrecios(imei);
                return Ok(listaPrecios);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("Warehouse")]
        public IActionResult GetWarehouse([FromQuery] string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }

                LPListWarehouse listaPrecios = new LPListWarehouse();
                ListaPreciosDAL listaPrecioDAL = new ListaPreciosDAL();
                listaPrecios = listaPrecioDAL.GetListWarehouse(imei);
                return Ok(listaPrecios);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("PriceListWarehouse")]
        public IActionResult GetPLWarehouse([FromQuery] string imei, string whsCode, string pl1, string pl2)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }

                ListaPreciosWhs listaPrecios = new ListaPreciosWhs();
                ListaPreciosDAL listaPrecioDAL = new ListaPreciosDAL();
                listaPrecios = listaPrecioDAL.GetPriceListWarehouse(imei, whsCode, pl1, pl2);
                return Ok(listaPrecios);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        [HttpGet("Header")]
        public IActionResult GetHeader([FromQuery] string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }

                ListaPreciosHeader listaPrecios = new ListaPreciosHeader();
                ListaPreciosDAL listaPrecioDAL = new ListaPreciosDAL();
                listaPrecios = listaPrecioDAL.GetListaPreciosHeader(imei);
                return Ok(listaPrecios);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
    }
}
