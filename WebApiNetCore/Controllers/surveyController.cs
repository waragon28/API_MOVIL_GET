using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.DAL;
using Sentry;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using SalesForce.BO;
using WebApiNetCore.BO;
using System.Net;

namespace WebApiNetCore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class surveyController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public surveyController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Get(string IdEncuesta)
        {
            try
            {
                if ((IdEncuesta.Length == 0) || (IdEncuesta == null))
                {
                    return BadRequest("El token es inválido");
                }

                SurveyDAL surveyDAL = new(_memoryCache);
                    //string despachos = surveyDAL.GetEncuesta(IdEncuesta);
                // Convertir la cadena de texto a un objeto JSON
                var objetoJson = JsonConvert.DeserializeObject(surveyDAL.GetEncuesta(IdEncuesta));

                // Convertir el objeto JSON a una cadena JSON con formato
                var jsonResult = JsonConvert.SerializeObject(objetoJson, Formatting.Indented);


                return Ok(jsonResult);
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
            SurveyDAL formDAL = new(_memoryCache);
            return Redirect(formDAL.getUrlImagen(Name));
        }

        [HttpPost]
        public IActionResult Post(Object JSONEncuesta)
        {
                SurveyDAL surveyDAL = new(_memoryCache);
                ResponseSurveyBO tempData = new();
            //string despachos = surveyDAL.GetEncuesta(IdEncuesta);
            // Convertir la cadena de texto a un objeto JSON
            ResponseData apiResponse = surveyDAL.PostEncuesta(JSONEncuesta).GetAwaiter().GetResult();

            ResponseError response=null;// apiResponse.Data;

            if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                tempData.DocEntry = apiResponse.Data["DocEntry"].ToString();
                tempData.DocNum = apiResponse.Data["DocNum"].ToString();
                tempData.ErrorCode = "0";
                tempData.Message = "Documento " + apiResponse.Data["DocNum"].ToString() + " creado satisfactoriamente";
                }
                else
                {
                tempData.ErrorCode = "1";
                tempData.Message = response.error.message.value.ToString();
            
            }
            return Ok(tempData);
        }

        [HttpGet("RangoEncuestas")]
        public IActionResult GetEncuestas(string FechaInicio, string FechaFin,string CodVendedor)
        {
            try
            {
                List<SurveyBO> objListEncuesta = new List<SurveyBO>();
                SurveyDAL ObjSurveyDAL = new(_memoryCache);

                objListEncuesta = ObjSurveyDAL.GetEncuestaFrm(FechaInicio, FechaFin, CodVendedor);
                if (objListEncuesta != null)
                {
                    return Ok(objListEncuesta);
                }
                return BadRequest("No se encontraron datos");
            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda " + e.Message.ToString());
            }
          
        }
   
    }
}
