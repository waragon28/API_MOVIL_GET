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
    public class PromotionDetailController : ControllerBase
    {
        // GET: api/<PromotionDetailController>
        [HttpGet]
        public IActionResult Get(string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListarPromoD listPromocionD = new ListarPromoD();
                ListaPromoDDAL listaPromoDDAL = new ListaPromoDDAL();
                listPromocionD = listaPromoDDAL.GetListaPromoD(imei);

                return Ok(listPromocionD);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }


        [HttpGet("PromDetailWareHouse")]
        public IActionResult PromDetailWareHouse(string imei,string WhsCode)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListarPromoD listPromocionD = new ListarPromoD();
                ListaPromoDDAL listaPromoDDAL = new ListaPromoDDAL();
                listPromocionD = listaPromoDDAL.GetListaPromoD(imei, WhsCode);

                return Ok(listPromocionD);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
    }
}
