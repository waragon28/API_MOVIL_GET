using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    public class DepositsController : ControllerBase
    {

        private IMemoryCache _memoryCache;
        public DepositsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }



        [HttpGet]
        public IActionResult Get(string imei, string fecIni, string fecFin)
        {
            try
            {
                if (imei.Length != 15 || fecIni.Length < 8 || fecFin.Length < 8)
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListDeposit listCobranzaC = new ListDeposit();
                DepositDAL cobranzaC = new DepositDAL();
                listCobranzaC = cobranzaC.GetCobranzaC(imei, fecIni, fecFin);
                return Ok(listCobranzaC);

            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpGet("BankValidation")]
        public IActionResult GetBankValidation(string imei)
        {
            try
            {
                if (imei.Length != 15)
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListDeposit listCobranzaC = new ListDeposit();
                DepositDAL cobranzaC = new DepositDAL();
                listCobranzaC = cobranzaC.bankValidation(imei);
                return Ok(listCobranzaC);

            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("Photo")]
        public RedirectResult GetPhoto(string Name)
        {
            DespachoDAL despachoDAL = new(_memoryCache);
            return Redirect(despachoDAL.getUrlImagen2(Name));
        }

        [HttpGet("Validate/")]
        public IActionResult GetValidDeposit(string bank, string deposit, decimal ammount, string date, string slpCode)
        {
            try
            {
                DepositResponse rr = new DepositResponse();
                DepositDAL cobranzas = new DepositDAL();
                rr = cobranzas.GetValidDeposit(bank, deposit, ammount, date, slpCode);

                if (rr != null)
                {
                    return Ok(rr);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda " + e);
            }

        }
    }

}
