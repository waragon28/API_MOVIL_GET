using Microsoft.AspNetCore.Mvc;
using SAP_Core.BO;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        // GET: api/<VersionController>
        [HttpGet]
        public IActionResult Get(string imei, string token)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                VersionBO versions = new VersionBO();
                VersionDAL version = new VersionDAL();
                versions = version.GetVersion(imei, token);
                return Ok(versions);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir con la búsqueda");
            }
        }
        /*[HttpGet]
        public HttpResponseMessage Get([FromQuery] string v)
        {
            //HttpResponse response;

            string url = "";
            url = S3_Imagen.DownloadApk(v);
            Uri u = new Uri(url);
            //var response = Request.CreateResponse(HttpStatusCode.Moved);
            HttpRequestMessage request = new HttpRequestMessage();
            HttpResponseMessage response2 = request.CreateResponse(HttpStatusCode.Moved);
            var response = response2;
            response.Headers.Location = u;
            return response;

        }*/
    }
}
