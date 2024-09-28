using Amazon.S3;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
using SalesForce.Util;
using Sap.Data.Hana;
using SAP_Core.BO;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApiNetCore;
using WebApiNetCore.Utils;
using static SalesForce.BO.Odata;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Sentry;
using static SAP_Core.Utils.DataSource;
using System.Reflection.Metadata;
using static WebApiNetCore.Utils.Other;
using Amazon.S3.Model;

namespace SAP_Core.DAL
{
    public class DespachoDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;
        private static readonly string _awsAccessKey = Startup.Configuration.GetValue<string>("S3:AWSAccessKey");
        private static readonly string _awsSecretKey = Startup.Configuration.GetValue<string>("S3:AWSSecretKey");
        private static readonly string _endpoingURL = Startup.Configuration.GetValue<string>("S3:EndpoingURL");
        private static readonly string _bucketName = Startup.Configuration.GetValue<string>("S3:Bucketname");

        public DespachoDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }
        public string getUrlImagen2(string fileNameAndExtension)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
            fileNameAndExtension = @"SalesForceApp/Deposito/" + fileNameAndExtension;
            string text = S3_Imagen.getUrlImage2(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }
        public ListaDespachoHeaderBO GetDespachoHeader(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaDespachoHeaderBO despachos = new ListaDespachoHeaderBO();
            List<DespachoHeaderBO> listaDespacho = new List<DespachoHeaderBO>();
            DespachoHeaderBO despacho = new DespachoHeaderBO();
            string strSQL = string.Format("CALL {0}.APP_DESPACHO_HEADER ('{1}','{2}')", DataSource.bd(), imei, fecha);

            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        despacho = new DespachoHeaderBO();
                        despacho.ControlCode = reader["ControlCode"].ToString();
                        despacho.Assistant = reader["Assistant"].ToString().ToUpper();
                        despacho.Brand = reader["Brand"].ToString();
                        despacho.OverallWeight = Convert.ToDecimal(reader["OverallWeight"]);
                        despacho.LicensePlate = reader["LicensePlate"].ToString();
                        despacho.Detail = reader["Detail"] == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<DetailDespacho>>(reader["Detail"].ToString());
                        listaDespacho.Add(despacho);
                    }
                }
                despachos.Obtener_DespachoCResult = listaDespacho;
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetDespachoHeader - " + ex.Message+" "+imei+" "+fecha);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();

                SentrySdk.CaptureException(ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return despachos;

        }

        private async Task<string> getListClientApi(string fecha, string imei)
        {
            string value = "";
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            string strSQL = string.Format("CALL {0}.APP_DESPACHO_LIST_CLIENT('{1}','{2}')", DataSource.bd(), fecha, imei);
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                value = Other.sqlDatoToJson(reader);
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_getListClientApi - " + ex.Message + " - " + imei);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
                value = ex.Message;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return value;
        }

        public string GetListaDespachosWms(string imei, string fecha, string flag)
        {
            string json = "";

            if (flag != null && flag == "Seller")
            {
                HanaDataReader reader;
                HanaConnection connection = GetConnection();
                string strSQL = string.Format("CALL {0}.APP_DESPACHO_HEADER_SALESPERSON('{1}', '{2}')", DataSource.bd(), imei, fecha);

                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    HanaCommand command = new HanaCommand(strSQL, connection);
                    reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    json = Other.sqlDatoToJson(reader);
                    connection.Close();
                }
                catch (Exception ex){

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetListaDespachosWms- " + ex.Message+" "+
                        imei+" "+fecha+" "+flag);
                    HanaCommand command = new HanaCommand(strSQL, connection);

                    reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    connection.Close();

                    SentrySdk.CaptureException(ex);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                json=getListClientApi(fecha, imei).GetAwaiter().GetResult();
            }

            return json;
        }

        public string getUrlImagen(string fileNameAndExtension)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
            fileNameAndExtension = @"SalesForceApp/Distribucion/" + fileNameAndExtension;
            string text = S3_Imagen.getUrlImage(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }

        private async Task recountDeliveredSucess(int docEntry)
        {
            string json = "";
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            string strSQL = string.Format("CALL {0}.APP_DESPACHO_DELIVERED_DOCUMENT_COUNT('{1}')", DataSource.bd(), docEntry);
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                Dictionary<string, int> data = new Dictionary<string, int>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.Add("CountSuccess", Convert.ToInt32(reader["CountSuccess"]));
                        data.Add("CountFailed", Convert.ToInt32(reader["CountFailed"]));
                    }
                    string jsonString = "{\"U_SuccessQuantity\":" + data["CountSuccess"] + ",\"U_FailedQuantity\":" + data["CountFailed"] + "}";
                    await serviceLayer.Request("/b1s/v1/VIS_DIS_ODRT(" + docEntry + ")", Method.PATCH, jsonString);
                }
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_recountDeliveredSucess - " + ex.Message + " - " + docEntry);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
        }

        private async Task updateStatusDeliveryNotesOrInvoices(string docEntry,string status,string returnReasonText, string sessionId)
        {
            /*
             * COMENTADO WARAGON
             * string jsonString = "{\"U_SYP_DT_ESTDES\":\""+ status + "\"}";
            if (status!="E" && returnReasonText != null)
            {
                jsonString = "{\"U_SYP_DT_ESTDES\":\"" + status + "\",\"U_SYP_DT_OCUR\":\""+ returnReasonText + "\"}"; 
            }

            string endPoint = "DeliveryNotes";

            if (DataSource.bd() == Company.B1H_VIST_BO.ToString() || DataSource.bd() == Company.B1H_VIST_CL.ToString())
            {
                endPoint = "Invoices";
            }
            */
            //  ResponseData response = await serviceLayer.Request( String.Format("/b1s/v1/{0}({1})", endPoint, docEntry), Method.PATCH, jsonString, sessionId);

            string endPoint = "DeliveryNotes";

            if (DataSource.bd() == Company.B1H_VIST_BO.ToString() || DataSource.bd() == Company.B1H_VIST_CL.ToString())
            {
                endPoint = "Invoices";
            }

            var response = new
            {
                StatusCode = HttpStatusCode.NoContent,
                Data = "OK"
            };

            if (response.StatusCode==HttpStatusCode.NoContent)
            {
                HanaDataReader reader;
                HanaConnection connection = GetConnection();
                string strSQL = string.Format("CALL {0}.APP_VALIDATE_SMS()", DataSource.bd());
                string resultValidate = "N";
                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    HanaCommand command = new HanaCommand(strSQL, connection);
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            resultValidate = reader["SendSms"].ToString();
                        }
                    }
                    if (resultValidate == "Y")
                    {
                        INotificationService notificationService = new Notificacion(serviceLayer);
                        notificationService.SendSms(endPoint, docEntry, status, returnReasonText).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", 
                        "Despacho_updateStatusDeliveryNotesOrInvoices - " + ex.Message+" "+docEntry+" "+status+" "+returnReasonText);
                    HanaCommand command = new HanaCommand(strSQL, connection);

                    reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    connection.Close();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                
            }
        }

        public async Task<ResponseData> changeStatus(string id, int line,string status)
        {
            string jsonString = "{\"Code\":\"" + id + "\",\"VIS_INC1Collection\":[{\"Code\":\""+id+"\",\"LineId\":"+ line + ",\"U_Inactive\":\""+status+"\"}]}";

            ResponseData response=await serviceLayer.Request( String.Format("/b1s/v1/VIS_OINC('{0}')", id), Method.PATCH, jsonString);

            return new ResponseData()
            {
                Data = response.Data,
                StatusCode = response.StatusCode
            };
        }

        public async Task<ResponseData> execute(JObject payload)
        {
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(payload.ToString());
            List<dynamic> listRespondeData = new List<dynamic>();

            foreach (dynamic subData in data["Data"])
            {
                if (subData["SAP"] == "") {
                    string jsonString = "{\"Warehouse\":\""+ subData["Warehouse"] + "\",\"Sublevel1\":\""+ subData["Sublevel1"] + "\",\"Sublevel2\":\""+ subData["Sublevel2"] + "\",\"Sublevel3\":\""+ subData["SubLevel3"] + "\",\"BinCode\":\""+ subData["BinCode"] + "\",\"Inactive\":\"tNO\",\"Description\":\"Ubicación  " + subData["BinCode"] + "\"}";

                    ResponseData response = await serviceLayer.Request(String.Format("/b1s/v1/BinLocations"),Method.POST, jsonString);
                    listRespondeData.Add(response.Data);
                }
            }
            
            return new ResponseData()
            {
                Data = listRespondeData,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseData> update(InDispatchList dispatchList)
        {

            List<DispatchResponse> dispatchResponseList = new();

            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);

            foreach (InDispatch dispatch in dispatchList.Dispatch)
            {

                UsuarioDAL user = new UsuarioDAL();
                LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();
                VIS_DIS_Drt1 listTemp = new();

                listTemp.VIS_DIS_DRT1Collection = dispatch.Details.Select(d => new VIS_DIS_Drt1collection
                {
                    DocEntry = dispatch.DocEntry,
                    LineId = d.LineId,
                    U_Comments = d.Comments,
                    U_CheckInTime = d.CheckInTime,
                    U_CheckOutTime = d.CheckOutTime,

                    U_PhotoDocument = S3_Imagen.Upload(s3Client, _bucketName, d.PhotoDocument, dispatch.DocEntry + "_" + d.LineId + "_Document.png", IMAGENES.DISTRIBUCIÓN).GetAwaiter().GetResult(),
                    U_PhotoStore = S3_Imagen.Upload(s3Client, _bucketName, d.PhotoStore, dispatch.DocEntry + "_" + d.LineId + "_Store.png", IMAGENES.DISTRIBUCIÓN).GetAwaiter().GetResult(),

                    U_PersonContact = d.PersonContact,
                    U_ReturnReason = d.ReturnReason,
                    U_ReturnReasonText = d.ReturnReasonText,
                    U_Delivered = d.Delivered,
                    U_UserName = d.UserName,
                    U_UserCode = d.UserCode,
                    U_Latitude = d.Latitude,
                    U_Longitude = d.Longitude,
                    U_DocEntry = d.DeliveryNotes
                }).ToList();

                List<DispatchResponseDetalle> dispatchResponseListDetalle = new();
                DispatchResponse dispatchResponse = new();
                dispatchResponse.DocEntry = dispatch.DocEntry;
                
                
                foreach (VIS_DIS_Drt1collection temp in listTemp.VIS_DIS_DRT1Collection)
                { 
                    string deliveryNote = temp.U_DocEntry;
                    string returnReasonText = temp.U_ReturnReasonText;

                    temp.U_DocEntry = null;
                    temp.U_ReturnReasonText = null;

                    string jsonString = "{\"VIS_DIS_DRT1Collection\":["+ JsonSerializer.Serialize(temp) + "]}";
                    ResponseData response = await serviceLayer.Request( "/b1s/v1/VIS_DIS_ODRT(" + dispatch.DocEntry + ")", Method.PATCH, jsonString, sl.token);
                    
                    DispatchResponseDetalle dispatchResponseDetalle = new();

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        dispatchResponseDetalle.LineId = temp.LineId;
                        dispatchResponseDetalle.Message = "Documento actualizado correctamente";
                        dispatchResponseDetalle.ErrorCode = "N";

                        if (temp.U_Delivered!=null && returnReasonText!=null)
                        {

                            HanaConnection connection = GetConnection();
                            //COMENTADO WARAGON (DEMORA EN ACTUALIZAR ENTREGA) new Task(async () => await updateStatusDeliveryNotesOrInvoices( deliveryNote, temp.U_Delivered, returnReasonText, sl.token)).Start();
                            string strSQL = string.Format("UPDATE {0}.\"ODLN\" SET \"U_SYP_DT_ESTDES\"='{2}', \"U_SYP_DT_OCUR\"='{3}' WHERE \"DocEntry\"='{1}'", 
                                DataSource.bd(), deliveryNote, temp.U_Delivered, returnReasonText);
                            connection.Open();
                            HanaCommand command = new HanaCommand(strSQL, connection);
                            HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                            connection.Close();
                           // WARAGON new Task(async () => await updateStatusDeliveryNotesOrInvoices(deliveryNote, temp.U_Delivered, returnReasonText, sl.token)).Start();
                        }
                    }
                    else
                    {
                        var responseBody = await response.Data.Content.ReadAsStringAsync();

                        dispatchResponseDetalle.LineId = temp.LineId;
                        dispatchResponseDetalle.Message = "Ocurrio un error al actualizar el documento:\n " + responseBody.ToString();
                        dispatchResponseDetalle.ErrorCode = "Y";
                    }
                    dispatchResponseListDetalle.Add(dispatchResponseDetalle);
                }

                dispatchResponse.Details= dispatchResponseListDetalle;
                dispatchResponseList.Add(dispatchResponse);

                //await recountDeliveredSucess( dispatch.DocEntry);
            }

            return new ResponseData()
            {
                Data = new DispatchResponseList(){ Dispatch=dispatchResponseList },
                StatusCode=HttpStatusCode.OK
            };
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
        ~DespachoDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
