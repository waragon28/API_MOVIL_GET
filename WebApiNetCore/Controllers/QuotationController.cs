using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {

        private IMemoryCache _memoryCache;
        public QuotationController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public class RequestBody
        {
            [Required]
            [StringLength(100)]
            public string  Fecha { get; set; }

            [Required]
            [StringLength(15)]
            public string Imei { get; set; } = null!;

        }

        public class RequestQuotation
        {
            [Required]
            [StringLength(100)]
            public string OsVersion { get; set; }

            [Required]
            [StringLength(100)]
            public string AppVersion { get; set; }

            [Required]
            [StringLength(100)]
            public string ModelDevice { get; set; }
            
            [Required]
            [StringLength(100)]
            public string Brand { get; set; }

        }

        [HttpPost]
        public IActionResult Index([FromBody] RequestBody json)
        {
            if (ModelState.IsValid)
            {

                  QuotationDAL quotationDAL = new QuotationDAL(_memoryCache);
                ListQuotation list = quotationDAL.GetCotizaciones(json.Fecha, json.Imei);
                if (list.Quotation == null || list.Quotation.Count == 0)
                {
                    return BadRequest("No hay datos para mostrar");
                }
                else
                {
                    return Ok(list);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("({docEntry})/CreateSalesOrder")]
        public IActionResult ConvertOrder(string docEntry,[FromBody] RequestQuotation requestJson)
        {
            if (ModelState.IsValid)
            {
                QuotationDAL quotationDAL = new(_memoryCache);
                QuotationList apiResponse = quotationDAL.toOrder(docEntry, requestJson).GetAwaiter().GetResult();

                return Ok(apiResponse);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
