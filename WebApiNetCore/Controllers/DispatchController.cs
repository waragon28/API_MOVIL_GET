using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using SalesForce.BO;
using SAP_Core.BO;
using SAP_Core.DAL;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {

        private IMemoryCache _memoryCache;
        public DispatchController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Get(string imei, string fecha)
        {
            try
            {
                if ((imei.Length != 15 || !imei.All(char.IsDigit)) && (fecha==null))
                {
                    return BadRequest("El token es inválido");
                }
                
                DespachoDAL despachoDAL = new (_memoryCache);
                ListaDespachoHeaderBO despachos = despachoDAL.GetDespachoHeader(imei, fecha);
                return Ok(despachos);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }

        [HttpGet("Photo")]
        public RedirectResult GetPhoto(string Name)
        {
           DespachoDAL despachoDAL = new (_memoryCache);
           return Redirect(despachoDAL.getUrlImagen(Name));
        }

        [HttpGet("Customer/")]
        public IActionResult Get2(string imei, string fecha)
        {
            try
            {
                if ((imei.Length != 15 || !imei.All(char.IsDigit)) && fecha==null)
                {
                    return BadRequest("El token es inválido");
                }
                ListaClientes2 clientes = new ListaClientes2();
                ClienteDAL clienteDAL = new ClienteDAL(_memoryCache);
                clientes = clienteDAL.GetClientesDespacho(imei, fecha);
                return Ok(clientes);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        [HttpGet("Type/")]
        public IActionResult Get([FromQuery] string imei)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                
                TipoDespachoDAL despacho = new();
                ListTipoDespacho despachos = despacho.GetOcurrencias(imei);
                return Ok(despachos);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        [HttpGet("List/")]
        public IActionResult ListDespachosWMS(string imei, string fecha,string flag)
        {
            try
            {
                 if ((imei.Length != 15 || !imei.All(char.IsDigit)) && fecha == null)
                 {
                    return BadRequest("El token es inválido");
                 }

                DespachoDAL despachoDAL = new(_memoryCache);
                string despachosJson = "{\"Dispatch\":" + despachoDAL.GetListaDespachosWms(imei, fecha,flag) + "}";
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
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }
    
        [HttpPatch]
        public IActionResult updateLineDispatch([FromBody] InDispatchList dispatchList)
        {
            if (ModelState.IsValid)
             {
                 DespachoDAL despachoDAL = new(_memoryCache);
                 ResponseData apiResponse = despachoDAL.update(dispatchList).GetAwaiter().GetResult();

                 if (apiResponse.StatusCode == HttpStatusCode.OK)
                 {
                     return Ok(apiResponse.Data);
                 }
                 else
                 {
                     return StatusCode((int)apiResponse.StatusCode);
                 }
             }
             else
             {
                 return BadRequest();
             }
           // return BadRequest();
        }

        //changeStatus


        //Dispatch
        [HttpGet("abs")]
        public IActionResult GetLocation( string id,int line,string status)
        {
            DespachoDAL despachoDAL = new(_memoryCache);
            ResponseData apiResponse= despachoDAL.changeStatus(id, line, status).GetAwaiter().GetResult();

            return Ok(apiResponse.Data);
        }



        [HttpPost("masive")]
        public IActionResult GetLocation([FromBody] JObject payload)
        {
            DespachoDAL despachoDAL = new(_memoryCache);
            ResponseData apiResponse = despachoDAL.execute( payload).GetAwaiter().GetResult();

            return Ok(apiResponse.Data);
        }


    }
}
