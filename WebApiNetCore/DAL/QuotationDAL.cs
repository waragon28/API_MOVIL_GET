
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Configuration;
using SalesForce.Util;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
using System.Net;
using static SalesForce.BO.Odata;
using System.Text.Json;
using System.Net.Http;
using static WebApiNetCore.Controllers.QuotationController;
using System.Data;
using static WebApiNetCore.Utils.Other;
using Newtonsoft.Json;
using Azure;
using Newtonsoft.Json.Linq;
using GoogleApi.Entities.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SAP_Core.Utils;

namespace SAP_Core.DAL
{
    public class QuotationDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;

        public QuotationDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }

        public static HttpResponseMessage SendLoginRequest(string url, Acceso loginRequest,ref string Token)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => { return true; };

                using (HttpClient client = new HttpClient(handler))
                {
                    string json = JsonConvert.SerializeObject(loginRequest);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    return response;
                }
            }
        }

        public List<Quotation> PostWithToken(string url, string token, string Json)
        {
            ResponseData rs = new ResponseData();
            Quotation quotation = new Quotation();
            List<Quotation> lQuotation = new List<Quotation>();

            string Respuesta = string.Empty;
            var baseAddress = new Uri(url);

            var cookieContainer = new CookieContainer();
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(clientHandler)
            {
                BaseAddress = baseAddress
            };
            client.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=2000");
            cookieContainer.Add(baseAddress, new Cookie("B1SESSION", token));

            StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // Manejo de la respuesta
            if (response.IsSuccessStatusCode)
            {
               
                string responseBody = response.Content.ReadAsStringAsync().Result;
                JObject jsonObject = JObject.Parse(responseBody);
                rs.StatusCode = response.StatusCode;
                int docNum = jsonObject["DocNum"].Value<int>();
                rs.Data = "Se creo la OV Nº " + Convert.ToString(docNum);

                quotation.DocEntry = jsonObject["DocEntry"].ToString();
                quotation.DocNum = jsonObject["DocNum"].ToString();
                // quotation.cardCode = jsonObject["CardCode"].ToString();
                quotation.Message = "Documento " + Convert.ToString(docNum)+ " creado satisfactoriamente";
                quotation.ErrorCode = "0";
                quotation.SalesOrderID = jsonObject["U_VIS_SalesOrderID"].ToString();
              //  quotation.docDate = jsonObject["DocDate"].ToString();
              //  quotation.slpCode = jsonObject["SalesPersonCode"].ToString();
            }
            else
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                // Parse the JSON string
                JObject parsedJson = JObject.Parse(responseBody);

                // Access the "value" field
                string value = (string)parsedJson["error"]["message"]["value"];

                quotation.ErrorCode = "1";
                quotation.DocEntry = "0";
              //  quotation.cardCode ="";
                quotation.Message = value;
                quotation.SalesOrderID = "";
              //  quotation.docDate = "";
                quotation.DocNum = "";
              //  quotation.slpCode = "";

            }
            lQuotation.Add(quotation);

            return lQuotation;
        }
        public ResponseData GetWithToken(string url, string token)
        {
            ResponseData rs = new ResponseData();
            string Respuesta = string.Empty;
            var baseAddress = new Uri(url);

            var cookieContainer = new CookieContainer();
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(clientHandler)
            {
                BaseAddress = baseAddress
            };
            client.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=2000");
            cookieContainer.Add(baseAddress, new Cookie("B1SESSION", token));


            // Enviar la solicitud POST
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Manejo de la respuesta
            if (response.IsSuccessStatusCode)
            {
               
                 string responseBody = response.Content.ReadAsStringAsync().Result;
                 JObject jsonObject = JObject.Parse(responseBody);
                 rs.StatusCode = response.StatusCode;
                 rs.Data = responseBody;
                
            }
            else
            {
                /*
                string responseBody = response.Content.ReadAsStringAsync().Result;
                rs.statusCode = response.StatusCode;
                var jsonResponse = JsonConvert.SerializeObject(responseBody);
                // Deserializar el JSON a un objeto dinámico
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                // Deserializar el JSON a un objeto dinámico
                dynamic jsonObject1 = JsonConvert.DeserializeObject<dynamic>(jsonObject);

                // Acceder al valor de la propiedad "value"
                string value = jsonObject1.error.message.value;

                // Deserializar la respuesta JSON a un objeto dinámico
                rs.statusCode = HttpStatusCode.BadRequest;
                rs.data = value;
                */
            }
            return rs;
        }

        public List<DocumentLine> LineCoti_To_OV(string DocEntry)
        {
            HanaConnection connection = GetConnection();
            List<DocumentLine> lDocumentLine = new List<DocumentLine>();
            DocumentLine ObjDocumentLine = new DocumentLine();
            try
            {
                HanaDataReader reader;
                string strSQL = string.Format("CALL {0}.APP_COTI_CONVERT_OV_LINE('{1}')", DataSource.bd(), DocEntry);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read()) 
                    {
                        ObjDocumentLine = new DocumentLine();

                        ObjDocumentLine.AcctCode = reader["AcctCode"].ToString();
                        ObjDocumentLine.COGSAccountCode = reader["CogsAcct"].ToString();
                        ObjDocumentLine.CostingCode = reader["OcrCode"].ToString();
                        ObjDocumentLine.CostingCode2 = reader["OcrCode2"].ToString();
                        ObjDocumentLine.CostingCode3 = reader["OcrCode3"].ToString();
                        ObjDocumentLine.DiscountPercent = reader["DiscPrcnt"].ToString();
                        ObjDocumentLine.Dscription = reader["Dscription"].ToString();
                        ObjDocumentLine.ItemCode = reader["ItemCode"].ToString();
                        ObjDocumentLine.UnitPrice = reader["Price"].ToString();
                        ObjDocumentLine.Quantity = reader["Quantity"].ToString();
                        ObjDocumentLine.TaxCode = reader["TaxCode"].ToString();
                        ObjDocumentLine.TaxOnly = reader["TaxOnly"].ToString();
                        ObjDocumentLine.WarehouseCode = reader["WhsCode"].ToString();
                        ObjDocumentLine.U_VIS_PromID = reader["U_VIS_PromID"].ToString();
                        ObjDocumentLine.U_VIS_PromLineID = reader["U_VIS_PromLineID"].ToString();
                        ObjDocumentLine.BaseEntry =Convert.ToInt32(DocEntry);
                        ObjDocumentLine.BaseLine = Convert.ToInt32(reader["LineNum"].ToString());
                        ObjDocumentLine.BaseType = 23;
                        lDocumentLine.Add(ObjDocumentLine);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString(); ;
            }
            return lDocumentLine;
        }

        public async Task<QuotationList> toOrder(string DocEntry, RequestQuotation informationClient)
        {
            HanaConnection connection = GetConnection();
            ResponseData responseData = new ResponseData();
            QuotationList quotationList = new QuotationList();
            Coti_To_OV coti_To_OV = new Coti_To_OV();
            try
            {
                HanaDataReader reader;
                string strSQL = string.Format("CALL {0}.APP_COTI_CONVERT_OV('{1}')", DataSource.bd(),DocEntry);

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        coti_To_OV = new Coti_To_OV();

                        coti_To_OV.CardCode=reader["CardCode"].ToString();
                        coti_To_OV.Comments = reader["Comments"].ToString();
                        coti_To_OV.DocCurrency = reader["DocCur"].ToString();
                        coti_To_OV.DocDate = DateTime.Now.ToString("yyyyMMdd").ToString();//FECHA DE SISTEMA
                        coti_To_OV.DocDueDate = reader["DocDueDate"].ToString();
                        coti_To_OV.DocumentsOwner = Convert.ToInt32(reader["OwnerCode"].ToString());
                        coti_To_OV.PayToCode = reader["PayToCode"].ToString();
                        coti_To_OV.PaymentGroupCode = Convert.ToInt32(reader["GroupNum"].ToString());
                        coti_To_OV.SalesPersonCode = Convert.ToInt32(reader["SlpCode"].ToString());
                        coti_To_OV.ShipToCode = reader["ShipToCode"].ToString();
                        coti_To_OV.TaxDate = DateTime.Now.ToString("yyyyMMdd").ToString();//FECHA DE SISTEMA

                        coti_To_OV.U_VIST_SUCUSU = reader["U_VIST_SUCUSU"].ToString();
                        coti_To_OV.U_VIS_AgencyCode = reader["U_VIS_AgencyCode"].ToString();
                        coti_To_OV.U_VIS_AgencyDir = reader["U_VIS_AgencyDir"].ToString();
                        coti_To_OV.U_VIS_AgencyName = reader["U_VIS_AgencyName"].ToString();
                        coti_To_OV.U_VIS_AgencyRUC = reader["U_VIS_AgencyRUC"].ToString();
                        coti_To_OV.U_VIS_SalesOrderID = reader["U_VIS_SalesOrderID"].ToString();
                        coti_To_OV.U_SYP_TVENTA = reader["U_SYP_TVENTA"].ToString();
                        coti_To_OV.U_VIS_Intent =Convert.ToDouble(reader["U_VIS_Intent"].ToString());
                        coti_To_OV.U_VIS_Brand = reader["U_VIS_Brand"].ToString();
                        coti_To_OV.U_VIS_Model = reader["U_VIS_Model"].ToString();
                        coti_To_OV.U_VIS_Version = reader["U_VIS_Version"].ToString();
                        coti_To_OV.DocType = "dDocument_Items";
                        coti_To_OV.DocObjectCode = "oOrders";

                        coti_To_OV.U_CODCTRL = reader["U_CODCTRL"].ToString();
                        coti_To_OV.U_SERIECOD = reader["U_SERIECOD"].ToString();
                        coti_To_OV.U_NROAUTOR = reader["U_NROAUTOR"].ToString();
                        coti_To_OV.U_NIT = reader["U_NIT"].ToString();
                        coti_To_OV.U_LB_WITHCC = reader["U_LB_WITHCC"].ToString();
                        coti_To_OV.U_TIPODOC =Convert.ToInt32(reader["U_TIPODOC"].ToString());
                        coti_To_OV.U_TIPOCOM = Convert.ToInt32(reader["U_TIPOCOM"].ToString());
                        coti_To_OV.U_RAZSOC = reader["U_RAZSOC"].ToString();
                        coti_To_OV.U_CODFORPI = reader["U_CODFORPI"].ToString();
                        coti_To_OV.U_NROTRAM = reader["U_NROTRAM"].ToString();
                        coti_To_OV.U_NROPOL = reader["U_NROPOL"].ToString();
                        coti_To_OV.U_BOLBSP = reader["U_BOLBSP"].ToString();
                        coti_To_OV.U_ICE = Convert.ToDouble(reader["U_ICE"].ToString());
                        coti_To_OV.U_EXENTO = Convert.ToDouble(reader["U_EXENTO"].ToString());
                        coti_To_OV.U_ESTADOFC = reader["U_ESTADOFC"].ToString();
                        coti_To_OV.U_FACANU = reader["U_FACANU"].ToString();
                        coti_To_OV.U_TASACERO = Convert.ToDouble(reader["U_TASACERO"].ToString());
                        coti_To_OV.U_SO1_01RETAILONE = reader["U_SO1_01RETAILONE"].ToString();
                        coti_To_OV.U_ORIGIN = reader["U_ORIGIN"].ToString();
                        coti_To_OV.U_SYP_TPOENTME = reader["U_SYP_TPOENTME"].ToString();
                        coti_To_OV.U_SYP_TPOSALME = reader["U_SYP_TPOSALME"].ToString();
                        coti_To_OV.U_VIS_AppVersion = reader["U_VIS_AppVersion"].ToString();
                        coti_To_OV.U_B_State = reader["U_B_State"].ToString();
                        coti_To_OV.U_B_type = reader["U_B_type"].ToString();
                        coti_To_OV.U_B_invtype = reader["U_B_invtype"].ToString();
                        coti_To_OV.U_B_invalidltn = reader["U_B_invalidltn"].ToString();
                        coti_To_OV.U_TIPOVENTA = reader["U_TIPOVENTA"].ToString();
                        coti_To_OV.U_Status = reader["U_Status"].ToString();
                        coti_To_OV.U_AproCoti = reader["U_AproCoti"].ToString();

                        coti_To_OV.DocumentLines = LineCoti_To_OV(DocEntry);
                    }

                    string url = "https://ecs-dbs-vistony:50000/b1s/v1/Drafts";

                    UsuarioDAL user = new UsuarioDAL();
                    LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();
                    string Json = JsonConvert.SerializeObject(coti_To_OV);


                    quotationList.SalesOrders = PostWithToken(url,sl.token,Json);


                }
                connection.Close();

            }
            catch (Exception ex)
            {
                ex.Message.ToString(); ;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return quotationList;
        }
        
        public ListQuotation GetCotizaciones(string fecha,string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<QuotationBO> listAgencias = new List<QuotationBO>();
            ListQuotation agencias = new ListQuotation();

           
            string strSQL = string.Format("SELECT * FROM \"_SYS_BIC\".\"App.SalesForce.Pe.Prod/LIST_QUOTATION\"('PLACEHOLDER' = ('$$Fecha$$','{0}'),'PLACEHOLDER' = ('$$Imei$$','{1}'))",fecha,imei);

            try
            {

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuotationBO quotation = new QuotationBO();
                        quotation.Autorizado = reader["Autorizado"].ToString().ToUpper();
                        quotation.CardCode = reader["CardCode"].ToString().ToUpper();
                        quotation.DocDate = reader["DocDate"].ToString();
                        quotation.DocEntry = int.Parse(reader["DocEntry"].ToString());
                        quotation.DocNum = reader["DocNum"].ToString().ToUpper();
                        quotation.StatusAproveed = reader["StatusAproveed"].ToString().ToUpper();
                        quotation.ReadyGenerateOv = reader["ReadyGenerateOv"].ToString();
                        quotation.DocumentTotal = Double.Parse(reader["DocumentTotal"].ToString());
                        quotation.CardName = reader["CardName"].ToString().ToUpper();
                        quotation.Object = reader["Object"].ToString();
                        quotation.LicTradNum = reader["LicTradNum"].ToString().ToUpper();
                        quotation.SlpCode = int.Parse(reader["SlpCode"].ToString());
                        quotation.SalesOrder = reader["SalesOrder"].ToString();
                        quotation.ApprovalCommentary = reader["ApprovalCommentary"].ToString();
                        quotation.Mobile_ID = reader["Mobile_ID"].ToString();

                        listAgencias.Add(quotation);
                    }
                }
                agencias.Quotation = listAgencias;
                connection.Close();
            }
            catch (Exception)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return agencias;
        }
        
        #region Disposable

        private bool disposing = false;
        /// <summary>
        /// Método de IDisposable para desechar la clase.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        /// <summary>
        /// Método sobrecargado de Dispose que será el que
        /// libera los recursos, controla que solo se ejecute
        /// dicha lógica una vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name=”b”></param>
        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            {
                if (!disposing)

                    // La marco como desechada ó desechandose,
                    // de forma que no se puede ejecutar este código
                    // dos veces.
                    disposing = true;
                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);
                // … libero los recursos… 
            }
        }




        /// <summary>
        /// Destructor de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta la lógica
        /// anterior para liberar los recursos.
        /// </summary>
        ~QuotationDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    
    }
}
