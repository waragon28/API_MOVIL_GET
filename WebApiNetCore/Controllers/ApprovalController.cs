using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAP_Core.BO;
using SAP_Core.DAL;
using Sentry;
using SalesForce.BO;
using SAP_Core.Utils;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public ApprovalController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]string user, [FromQuery] string status)
        {
            try
            {
               // Get_Documents

                ApprovalDAL approvalDAL = new(_memoryCache);
                ListApprovalBo approvalList = approvalDAL.Get_Documents(user, status);

                return Ok(approvalList);
            }
            catch (Exception e)
            {

                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }

        [HttpGet("Deuda")]
        public IActionResult GetDeuda([FromQuery]string cardCode)
        {
            try
            {
               // Get_Documents

                ApprovalDAL approvalDAL = new(_memoryCache);
                ListDeudaBo listDeudaBo = approvalDAL.Get_Deuda(cardCode);

                return Ok(listDeudaBo);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }
        
        [HttpGet("Linea")]
        public IActionResult GetLinea([FromQuery]string cardCode)
        {
            try
            {
                ApprovalDAL approvalDAL = new(_memoryCache);
                ListLineaBo listDeudaBo = approvalDAL.Get_Linea(cardCode);

                return Ok(listDeudaBo);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }

        [HttpGet("Customer")]
        public IActionResult GetCustomer([FromQuery]string cardCode, [FromQuery]string user)
        {
            try
            {
                ApprovalDAL approvalDAL = new(_memoryCache);
                ListClienteBo listDeudaBo = approvalDAL.Get_Cliente(cardCode,user);

                return Ok(listDeudaBo);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }

        [HttpGet("Pedidos/Detalle")]
        public IActionResult GetPedidosDetalle([FromQuery]string docEntry,string Tipo="")
        {
            try
            {
                ApprovalDAL approvalDAL = new(_memoryCache);
                ListPedidoDetalleBo listDeudaBo = approvalDAL.Get_PedidosDetalle(docEntry,Tipo);

                return Ok(listDeudaBo);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }

         [HttpGet("Pedidos/Rules")]
        public IActionResult GetRules([FromQuery]string docEntry, string Tipo = "")
        {
            try
            {
                ApprovalDAL approvalDAL = new(_memoryCache);
                ListAprovacionBo listDeudaBo = approvalDAL.Get_Rules(docEntry, Tipo);

                return Ok(listDeudaBo);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }
        }

        [HttpGet("Pedidos/PriceHistory")]
        public IActionResult Get_PriceHistory(string ItemCode)
        {
            try
            {
                ApprovalDAL approvalDAL = new(_memoryCache);
                List<PriceHistoy> listPriceHistoy = approvalDAL.Get_PriceHistory(ItemCode);
                OPriceHistoy obj = new OPriceHistoy();
                obj.Data = listPriceHistoy;
                return Ok(obj);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        [HttpPost]
        public IActionResult Post([FromQuery] string docEntry, [FromBody] JsonElement Payload, [FromHeader] string SessionId,string Tipo = "")
        {
            
            string jsonString = JsonSerializer.Serialize(Payload);

            ApprovalDAL approvalDAL = new(_memoryCache);
            ResponseData response = approvalDAL.decicion(jsonString, docEntry, SessionId, Tipo).GetAwaiter().GetResult();

            return Ok(response);
            
        }

        [HttpPost("Documentos")]
        public IActionResult PostDocumentos(Filtro filtro)
        {
            ApprovalDAL approvalDAL = new(_memoryCache);
            ListDocumentosBO response = approvalDAL.Documentos(filtro.Id,filtro.User,filtro.Status);

            return Ok(response);

        }

        [HttpGet("Pedidos/Anexos")]
        public IActionResult Get_Anexos(string DocEntry)
        {
            try
            {
                ApprovalDAL approvalDAL = new(_memoryCache);
                LstAnexo listLstAnexo = approvalDAL.Get_Anexos(DocEntry);

                return Ok(listLstAnexo);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

    }
}
