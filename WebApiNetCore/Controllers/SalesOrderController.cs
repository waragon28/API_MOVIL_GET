using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using SalesForce.BO;
using SAP_Core.BO;
using SAP_Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiNetCore.BO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public SalesOrderController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        // GET: api/<SalesOrderController>
        [HttpGet]
        public IActionResult Get(string imei, string fecha)
        {
            try
            {
                if (imei.Length != 15 || !imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListaOrdenVentaC listOrdenVentaC = new ListaOrdenVentaC();
                OrdenVentaCDAL ordenVentaCDAL = new OrdenVentaCDAL();
                listOrdenVentaC = ordenVentaCDAL.GetOrdenVentaC(imei, fecha);

                return Ok(listOrdenVentaC);
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("Validate/")]
        public IActionResult GetValidDeposit(string CardCode, string DocDate, string SalesOrderID, string slpCode)
        {
            try
            {
                SalesOrder rr = new SalesOrder();
                SalesOrderDAL cobranzas = new SalesOrderDAL(_memoryCache);
                rr = cobranzas.validSalesOrder(CardCode, DocDate, SalesOrderID, slpCode);
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
        [HttpGet("Object/")]
        public IActionResult getObject([FromQuery] string imei)
        {
            try
            {
                ListObjects rr = new ListObjects();
                SalesOrderDAL order = new SalesOrderDAL(_memoryCache);
                rr = order.getObjects(imei);
                if (rr != null)
                {
                    return Ok(rr);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda " + e);
            }
        }
        [HttpGet("BusinessLayer/")]
        public IActionResult getBusinessLayer([FromQuery] string imei)
        {
            try
            {
                ListBusinessLayer rr = new ListBusinessLayer();
                SalesOrderDAL order = new SalesOrderDAL(_memoryCache);
                rr = order.getBusinessLayer(imei);
                if (rr != null)
                {
                    return Ok(rr);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda " + e);
            }
        }
        [HttpGet("BusinessLayerDetail/")]
        public IActionResult getBusinessLayerDetail([FromQuery] string imei)
        {
            try
            {
                LisstBusinessLayerSalesDetail rr = new LisstBusinessLayerSalesDetail();
                SalesOrderDAL order = new SalesOrderDAL(_memoryCache);
                rr = order.getBusinessLayerDetail(imei);
                if (rr != null)
                {
                    return Ok(rr);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda " + e);
            }
        }

        [HttpPost]
        public IActionResult insertOv([FromBody] JObject text)
        {
            try
            {
                SalesOrderDAL orderDAL = new(_memoryCache);
                SalesOrdersPE ordersBO = new SalesOrdersPE();
                ordersBO = orderDAL.getSalesOrdersPE(text);

                string json = JsonSerializer.Serialize(ordersBO);
                List<SalesOrderPE> c = ordersBO.SalesOrders;
                int i = 0;
                List<SalesOrder> listOrders = new List<SalesOrder>();
                ListOrders orders = new ListOrders();

                foreach (SalesOrderPE obj in c)
                {
                    SalesOrder tempData = new();

                    if (i >= 10)
                    {
                        tempData.ErrorCode = "1";
                        tempData.Message = "Es necesario reenviar este objeto.";
                        tempData.SalesOrderID = obj.U_VIS_SalesOrderID;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(obj.SalesPersonCode) || String.IsNullOrEmpty(obj.DocumentsOwner))
                        {
                            tempData.ErrorCode = "1";
                            tempData.Message = "El valor del campo 'SalesPersonCode' y 'DocumentsOwner' no deben tener valores vacios o nulos.";
                            tempData.SalesOrderID = obj.U_VIS_SalesOrderID;
                        }
                        else
                        {
                            Task.Delay(1500).Wait();
                            //tempData = orderDAL.validSalesOrder(obj.CardCode, obj.DocDate, obj.U_VIS_SalesOrderID, obj.SalesPersonCode);

                            if (tempData.DocEntry == null)
                            {

                                tempData = new();
                                string endPoint = string.Empty;

                                if ((obj.Draft == null || obj.Draft == "N") && obj.quotation == "N")
                                {
                                    endPoint = "Orders";
                                }
                                else if (obj.quotation == "Y")
                                {
                                    if (obj.Draft == "Y")
                                    {
                                        endPoint = "Drafts";
                                    }
                                    else
                                    {
                                        endPoint = "Quotations";
                                    }
                                }
                                else
                                {
                                    endPoint = "Drafts";
                                }

                                ResponseData apiResponse = orderDAL.insert(endPoint, obj.ToString()).GetAwaiter().GetResult();

                                if (apiResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    tempData.SalesOrderID = apiResponse.Data["U_VIS_SalesOrderID"].ToString();
                                    tempData.DocEntry = apiResponse.Data["DocEntry"].ToString();
                                    tempData.DocNum = apiResponse.Data["DocNum"].ToString();
                                    tempData.ErrorCode = "0";
                                    tempData.Message = "Documento " + apiResponse.Data["DocNum"].ToString() + " creado satisfactoriamente";
                                }
                                else
                                {
                                    ResponseError response = apiResponse.Data;

                                    if (response.error.message.value.ToString() == "No matching records found (ODBC -2028)")
                                    {
                                        tempData.ErrorCode = "0";
                                        tempData.Message = "Este documento necesita ser aprobado";
                                        tempData.SalesOrderID = obj.U_VIS_SalesOrderID;
                                    }
                                    else
                                    {
                                        tempData.ErrorCode = "1";
                                        tempData.Message = response.error.message.value.ToString();
                                        tempData.SalesOrderID = obj.U_VIS_SalesOrderID;
                                    }
                                }
                            }
                        }
                    }

                    listOrders.Add(tempData);

                    i++;
                }
            }
            catch (Exception e)
            {
                List<SalesOrder> listOrders = new List<SalesOrder>();
                SalesOrder s = new SalesOrder();
                s.ErrorCode = "1";
                s.Message = e.Message;

                ListOrders orders = new ListOrders();
                orders.SalesOrders = listOrders;
                return Ok(orders);
            }
            return BadRequest();
        }

    }
}
