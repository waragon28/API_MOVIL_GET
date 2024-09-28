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
    public class PromotionHeaderController : ControllerBase
    {
        // GET: api/<PromotionHeaderController>
        [HttpGet]
        public IActionResult Get(string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListarPromoC listPromocionC = new ListarPromoC();
                ListaPromoCDAL listaPromoCDAL = new ListaPromoCDAL();
                listPromocionC = listaPromoCDAL.GetListaPromoC(imei);

                return Ok(listPromocionC);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        
        [HttpGet("PromWareHouse")]
        public IActionResult PromWareHouse(string imei,string WhsCode)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListarPromoC listPromocionC = new ListarPromoC();
                ListaPromoCDAL listaPromoCDAL = new ListaPromoCDAL();
                listPromocionC = listaPromoCDAL.GetListaPromoCWhsCode(imei, WhsCode);

                return Ok(listPromocionC);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

    }
}
