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
    public class QuotaController : ControllerBase
    {
       
        // GET: api/<QuotaController>
        [HttpGet("Header/")]
        public IActionResult Get(string CardCode, string SlpCode)
        {
            try
            {
                ListQuotaBO quotas = new ListQuotaBO();
                QuotaDAL quota = new QuotaDAL();
                quotas = quota.GetQuotas(CardCode, SlpCode);
                return Ok(quotas);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpGet("Detail/")]
        public IActionResult GetDetail(string SlpCode, string CardCode)
        {
            try
            {
                ListQuotaDetailBO quotas = new ListQuotaDetailBO();
                QuotaDAL quota = new QuotaDAL();
                quotas = quota.GetQuotaDetail(SlpCode, CardCode);
                return Ok(quotas);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
        [HttpGet("Invoice/")]
        public IActionResult GetInvoice(string SlpCode, string CardCode)
        {
            try
            {
                ListQuotaInvoiceBO quotas = new ListQuotaInvoiceBO();
                QuotaDAL quota = new QuotaDAL();
                quotas = quota.GetQuotaInvoice(SlpCode, CardCode);
                return Ok(quotas);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");

            }
        }
    }
}
