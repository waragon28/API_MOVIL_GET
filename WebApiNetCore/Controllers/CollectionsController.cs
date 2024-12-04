using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SalesForce.BO;
using SAP_Core.BO;
using SAP_Core.DAL;
using SAP_Core.Utils;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiNetCore.BO;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public CollectionsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("Date/")]
        public IActionResult Get(string imei, string fecha)
        {
            try
            {
                if (imei.Length != 15 || fecha.Length < 8)
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListaCobranzaD listCobranzaC = new ListaCobranzaD();
                CobranzaDDAL cobranzaC = new CobranzaDDAL(_memoryCache);
                listCobranzaC = cobranzaC.GetCollections(imei, fecha);
                return Ok(listCobranzaC);
            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda");
            }


        }
        [HttpGet("Receip/")]
        public IActionResult GetCobranzaDetail(string imei, string recibo)
        {
            try
            {
                if (imei.Length != 15 || string.IsNullOrWhiteSpace(imei) || string.IsNullOrEmpty(recibo))
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit) || !recibo.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                CobranzaDBO cobranza = new CobranzaDBO();
                CobranzaDDAL cobranzaC = new CobranzaDDAL(_memoryCache);
                cobranza = cobranzaC.GetCollectionDetail(imei, recibo);

                if (cobranza != null)
                {
                    return Ok(cobranza);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("Document/")]
        public IActionResult GetCobranzaDocument(string imei, string docentry)
        {
            try
            {
                if (imei.Length != 15 || string.IsNullOrWhiteSpace(imei) || string.IsNullOrEmpty(docentry))
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit) || !docentry.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                List<CobranzaDBO> cobranza = new List<CobranzaDBO>();
                CobranzaDDAL cobranzaC = new CobranzaDDAL(_memoryCache);
                cobranza = cobranzaC.GetCollectionDocument(imei, docentry);

                if (cobranza != null)
                {
                    return Ok(cobranza);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("Status/")]
        public IActionResult GetCobranzaStatus(string imei, string status)
        {
            try
            {
                if (imei.Length != 15 || string.IsNullOrWhiteSpace(imei) || string.IsNullOrEmpty(status))
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListaCobranzaD listCobranzaC = new ListaCobranzaD();
                CobranzaDDAL cobranzaC = new CobranzaDDAL(_memoryCache);
                listCobranzaC = cobranzaC.GetCollectionStatus(imei, status);


                if (listCobranzaC != null)
                {
                    return Ok(listCobranzaC);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("Status2/")]
        public IActionResult GetCobranzaStatus2(string imei, string status, string user)
        {
            try
            {
                if (imei.Length != 15 || string.IsNullOrWhiteSpace(imei) || string.IsNullOrEmpty(status))
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListaCobranzaD listCobranzaC = new ListaCobranzaD();
                CobranzaDDAL cobranzaC = new CobranzaDDAL(_memoryCache);
                listCobranzaC = cobranzaC.GetCollectionStatus2(imei, status, user);


                if (listCobranzaC != null)
                {
                    return Ok(listCobranzaC);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                return BadRequest("No se pudo concluir la busqueda");
            }

        }
        [HttpGet("Deposit/")]
        public IActionResult GetCobranzaDeposit(string imei, string deposit)
        {
            try
            {
                if (imei.Length != 15 || string.IsNullOrWhiteSpace(imei) || string.IsNullOrEmpty(deposit))
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListaCobranzaD listCobranzaC = new ListaCobranzaD();
                CobranzaDDAL cobranzaC = new CobranzaDDAL(_memoryCache);
                listCobranzaC = cobranzaC.GetCollectionDeposit(imei, deposit);


                if (listCobranzaC != null)
                {
                    return Ok(listCobranzaC);
                }
                return BadRequest("No se encontraron datos");

            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        [HttpGet("Deposit_Status/")]
        public IActionResult GetCobranzaDepositStatus(string imei, string Status)
        {
            try
            {
                if (imei.Length != 15 || string.IsNullOrWhiteSpace(imei) || string.IsNullOrEmpty(Status))
                {
                    return BadRequest("El token es inválido");
                }
                if (!imei.All(char.IsDigit))
                {
                    return BadRequest("El token es inválido");
                }
                ListaDepositsStatus2 ObListaDepositsStatus2 = new ListaDepositsStatus2();

               List<DepositsStatus2> ListDepositsStatus = new List<DepositsStatus2>();
                CobranzaDDAL cobranzaC = new CobranzaDDAL(_memoryCache);
                ObListaDepositsStatus2.Deposits = cobranzaC.GetCollectionDepositStatus(imei, Status);


                if (ListDepositsStatus != null)
                {
                    return Ok(ObListaDepositsStatus2);
                }

                return BadRequest("No se encontraron datos");
              
            }
            catch (Exception e)
            {
                //Log.save(this, e.Message);
                return BadRequest("No se pudo concluir la busqueda");
            }

        }

        private bool isString(string text)
        {
            return text.Any(ch => Char.IsLetterOrDigit(ch));
        }

        [HttpGet("Validate/")]
        public IActionResult GetValidCollection(string IncomeDate, string CardCode, string DocEntry, string Receip, string SlpCode)
        {
            if (isString(CardCode) && isString(DocEntry) && isString(Receip) && isString(SlpCode))
            {
                try
                {
                    ReceipResponse rr = new ReceipResponse();
                    //CobranzaDDAL cobranzas = new CobranzaDDAL();
                    rr = new CobranzaDDAL(_memoryCache).GetValidCollections(IncomeDate, CardCode, DocEntry, Receip, SlpCode);

                    if (rr != null)
                    {
                        return Ok(rr);
                    }
                    return BadRequest("No se encontraron datos");

                }
                catch (Exception)
                {
                    return BadRequest("No se pudo concluir la busqueda ");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult insertCollection([FromBody] JsonElement json)
        {
            try
            {
                dynamic objs = JsonConvert.DeserializeObject(json.ToString());
                dynamic c = objs["Collections"];
                List<ReceipResponse> ListReceip = new List<ReceipResponse>();
                ListReceipResponse receips = new ListReceipResponse();
                CobranzaDDAL co = new CobranzaDDAL(_memoryCache);

                int i = 0;

                foreach (dynamic obj in c)
                {
                    ReceipResponse tempData = new();

                    if (i >= 10)
                    {
                        tempData.ErrorCode = "1";
                        tempData.Message = "Es necesario reenviar este objeto.";
                        tempData.ItemDetail = obj["ItemDetail"].ToString();
                        tempData.Receip = obj["Receip"].ToString();
                        tempData.Code = "S/N Code";
                    }
                    else
                    {
                        Task.Delay(1500).Wait();

                        //tempData=co.GetValidCollections(obj["IncomeDate"].ToString(), obj["CardCode"].ToString(), obj["DocEntryFT"].ToString(), obj["Receip"].ToString(), obj["SlpCode"].ToString()).GetAwaiter().GetResult();

                        if (tempData.Receip == null)
                        {
                            tempData = new();

                            ReciboSAP rs = co.SetCollection(obj);
                            dynamic jsonData = JsonConvert.SerializeObject(rs);

                            ResponseData apiResponse = co.insert(jsonData.ToString()).GetAwaiter().GetResult();

                            if (apiResponse.StatusCode == HttpStatusCode.OK)
                            {
                                tempData.ErrorCode = "0";
                                tempData.Receip = obj["Receip"].ToString();
                                tempData.Message = "Cobranza " + obj["ItemDetail"].ToString() + " registrada satisfactoriamente";
                                tempData.ItemDetail = obj["ItemDetail"].ToString();
                                tempData.Code = apiResponse.Data.ToString();
                            }
                            else
                            {
                                ResponseError response = apiResponse.Data;
                                if (response.error.message.value.ToString() == "This entry already exists in the following tables (ODBC -2035)" || response.error.message.value.ToString() == "Esta entrada ya existe en las tablas siguientes (ODBC -2035)")
                                {
                                    tempData.ErrorCode = "0";
                                    tempData.Message = "Cobranza " + obj["ItemDetail"].ToString() + " registrada satisfactoriamente";
                                    tempData.ItemDetail = obj["ItemDetail"].ToString();
                                    tempData.Receip = obj["Receip"].ToString();
                                    tempData.Code = rs.Code;
                                }
                                else
                                {
                                    tempData.ErrorCode = response.error.code.ToString();
                                    tempData.Message = response.error.message.value.ToString();
                                    tempData.ItemDetail = obj["ItemDetail"].ToString();
                                    tempData.Receip = obj["Receip"].ToString();
                                    tempData.Code = rs.Code;
                                }

                            }
                        }
                    }

                    ListReceip.Add(tempData);

                    i++;
                }

                receips.Collections = ListReceip;

                return Ok(receips);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
