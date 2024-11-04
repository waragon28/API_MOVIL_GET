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
using WebApiNetCore.BO;
using System.ComponentModel;
using Newtonsoft.Json;
using Azure;
using GoogleApi.Entities.Interfaces;
using System.Net.Http;
using WebApiNetCore.DAL;
using Microsoft.Graph.IdentityGovernance.PrivilegedAccess.Group.EligibilityScheduleRequests;
using ZXing;
using Microsoft.AspNetCore.Builder;
using GoogleApi.Entities.Maps.Directions.Response;
using Microsoft.IdentityModel.Abstractions;
using System.Runtime.InteropServices;

namespace SAP_Core.DAL
{
    public class DespachoDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;
        private static readonly string _awsAccessKey = Startup.Configuration.GetValue<string>("S3:AWSAccessKey");
        private static readonly string _awsSecretKey = Startup.Configuration.GetValue<string>("S3:AWSSecretKey");
        private static readonly string _endpoingURL = Startup.Configuration.GetValue<string>("S3:EndpoingURL");
        private static readonly string _bucketName = Startup.Configuration.GetValue<string>("S3:Bucketname");

        CorreoAlert correoAlert = new CorreoAlert();
        UsuarioDAL user = new UsuarioDAL();

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
           string strSQL = string.Format("CALL {0}.APP_DESPACHO_LIST_CLIENT_QA('{1}','{2}')",DataSource.bd(), fecha, imei); //QA
          //  string strSQL = string.Format("CALL {0}.APP_DESPACHO_LIST_CLIENT('{1}','{2}')",DataSource.bd(), fecha, imei);
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

                    LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();

                    string jsonString = "{\"U_SuccessQuantity\":" + data["CountSuccess"] + ",\"U_FailedQuantity\":" + data["CountFailed"] + "}";
                    await serviceLayer.Request("/b1s/v1/VIS_DIS_ODRT(" + docEntry + ")", Method.PATCH, jsonString, sl.token);
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
                user.LogoutServiceLayer().GetAwaiter().GetResult();
            }
        }

       /* private async Task updateStatusDeliveryNotesOrInvoices(string docEntry,string status,string returnReasonText, string sessionId)
        {
            
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
        }*/

        public async Task<ResponseData> changeStatus(string id, int line,string status)
        {
            string jsonString = "{\"Code\":\"" + id + "\",\"VIS_INC1Collection\":[{\"Code\":\""+id+"\",\"LineId\":"+ line + ",\"U_Inactive\":\""+status+"\"}]}";

            ResponseData response=null;
            try
            {


                LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();
                response = await serviceLayer.Request(String.Format("/b1s/v1/VIS_OINC('{0}')", id), Method.PATCH, jsonString, sl.token);

            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Data  = ex.Message.ToString();
            }
            finally
            {
                user.LogoutServiceLayer().GetAwaiter().GetResult();
            }
               
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
            ResponseData response = null;
            LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();

            try
            {
                foreach (dynamic subData in data["Data"])
                {
                    if (subData["SAP"] == "")
                    {
                        string jsonString = "{\"Warehouse\":\"" + subData["Warehouse"] + "\",\"Sublevel1\":\"" + subData["Sublevel1"] + "\",\"Sublevel2\":\"" + subData["Sublevel2"] + "\",\"Sublevel3\":\"" + subData["SubLevel3"] + "\",\"BinCode\":\"" + subData["BinCode"] + "\",\"Inactive\":\"tNO\",\"Description\":\"Ubicación  " + subData["BinCode"] + "\"}";

                        response = await serviceLayer.Request(String.Format("/b1s/v1/BinLocations"), Method.POST, jsonString, sl.token);
                        listRespondeData.Add(response.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Data = ex.Message.ToString();
            }
            finally
            {
                user.LogoutServiceLayer().GetAwaiter().GetResult();
            }
           
            
            return new ResponseData()
            {
                Data = listRespondeData,
                StatusCode = response.StatusCode
            };
        }


        public async Task<ResponseData> SolicitudDevolucion(string DocEntry,string returnReasonText,string Token)
            
        {
            ResponseData rs = new ResponseData();
            HanaConnection connection = GetConnection();
            try
            {
                HanaDataReader reader;
                string strSQL = string.Format("CALL {0}.APP_SOLI_DEV_C ({1})", DataSource.bd(), DocEntry);
                //   SoliDevBO ObjSoliDevBO = new SoliDevBO();
                SolicitudDevolucion ObjSolicitudDevolucion = new SolicitudDevolucion();

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ObjSolicitudDevolucion = new SolicitudDevolucion();
                        ObjSolicitudDevolucion.DocType = "dDocument_Items";
                        ObjSolicitudDevolucion.DocDate = reader["DocDate"].ToString();
                        ObjSolicitudDevolucion.DocDueDate = reader["DocDueDate"].ToString();
                        ObjSolicitudDevolucion.CardCode = reader["CardCode"].ToString();
                        ObjSolicitudDevolucion.NumAtCard = reader["NumAtCard"].ToString();
                        ObjSolicitudDevolucion.DocCurrency = reader["DocCur"].ToString();
                        ObjSolicitudDevolucion.Comments = reader["Comments"].ToString();
                        ObjSolicitudDevolucion.PaymentGroupCode = Convert.ToInt32(reader["GroupNum"].ToString());
                        ObjSolicitudDevolucion.SalesPersonCode = Convert.ToInt32(reader["SlpCode"].ToString());
                        ObjSolicitudDevolucion.DocumentsOwner = Convert.ToInt32(reader["OwnerCode"].ToString());
                        ObjSolicitudDevolucion.TaxDate = reader["TaxDate"].ToString();
                        ObjSolicitudDevolucion.DocObjectCode = "oReturnRequest";
                        ObjSolicitudDevolucion.ShipToCode = reader["ShipToCode"].ToString();
                        ObjSolicitudDevolucion.PayToCode = reader["PayToCode"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDMT = reader["U_SYP_MDMT"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDTD = "";
                        ObjSolicitudDevolucion.U_SYP_MDSD = "";
                        ObjSolicitudDevolucion.U_SYP_MDCD = "";
                        ObjSolicitudDevolucion.U_SYP_FECHAREF = reader["FechaRef"].ToString();//AGREGADO
                        ObjSolicitudDevolucion.U_SYP_STATUS = reader["U_SYP_STATUS"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DETPAGADO = reader["U_SYP_DETPAGADO"].ToString();
                        ObjSolicitudDevolucion.U_SYP_AUTODET = reader["U_SYP_AUTODET"].ToString();
                        ObjSolicitudDevolucion.U_SYP_NGUIA = reader["U_SYP_NGUIA"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDFN = reader["U_SYP_MDFN"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDFC = reader["U_SYP_MDFC"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDVN = reader["U_SYP_MDVN"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDVC = reader["U_SYP_MDVC"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDTS = reader["U_SYP_MDTS"].ToString();
                        ObjSolicitudDevolucion.U_SYP_CONSIGNADOR = reader["U_SYP_CONSIGNADOR"].ToString();
                        ObjSolicitudDevolucion.U_SYP_GRFT = reader["U_SYP_GRFT"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TIPO_TRANSF = reader["U_SYP_TIPO_TRANSF"].ToString();
                        ObjSolicitudDevolucion.U_SYP_CONMON = reader["U_SYP_CONMON"].ToString();
                        ObjSolicitudDevolucion.U_SYP_ANTPEN = reader["U_SYP_ANTPEN"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DOCAPR = reader["U_SYP_DOCAPR"].ToString();
                        ObjSolicitudDevolucion.U_SYP_OCTRA = reader["U_SYP_OCTRA"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TIPEXP = reader["U_SYP_TIPEXP"].ToString();
                        ObjSolicitudDevolucion.U_SYP_PNACT = reader["U_SYP_PNACT"].ToString();
                        ObjSolicitudDevolucion.U_PZCreated = reader["U_PZCreated"].ToString();
                        ObjSolicitudDevolucion.U_SYP_INC = reader["U_SYP_INC"].ToString();
                        ObjSolicitudDevolucion.U_SYP_CON_STOK = reader["U_SYP_CON_STOK"].ToString();
                        ObjSolicitudDevolucion.U_SYP_PDTREV = reader["U_SYP_PDTREV"].ToString();
                        ObjSolicitudDevolucion.U_SYP_PDTCRE = reader["U_SYP_PDTCRE"].ToString();
                        ObjSolicitudDevolucion.U_VIT_VENMOS = reader["U_VIT_VENMOS"].ToString();
                        ObjSolicitudDevolucion.U_VIST_PROMADIC = reader["U_VIST_PROMADIC"].ToString();
                        ObjSolicitudDevolucion.U_VIST_SUCUSU = reader["U_VIST_SUCUSU"].ToString();
                        ObjSolicitudDevolucion.U_VIST_APSOLV = reader["U_VIST_APSOLV"].ToString();
                        ObjSolicitudDevolucion.U_VIST_DIS = reader["U_VIST_DIS"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TVENTA = reader["U_SYP_TVENTA"].ToString();
                        ObjSolicitudDevolucion.U_VIS_SalesOrderID = reader["U_VIS_SalesOrderID"].ToString();
                        ObjSolicitudDevolucion.U_VIS_CommentApproval = reader["U_VIS_CommentApproval"].ToString();
                        ObjSolicitudDevolucion.U_VIS_OVCommentary = reader["U_VIS_OVCommentary"].ToString();
                        ObjSolicitudDevolucion.U_VIS_EVCommentary = reader["U_VIS_EVCommentary"].ToString();
                        ObjSolicitudDevolucion.U_VIS_INCommentary = reader["U_VIS_INCommentary"].ToString();
                        ObjSolicitudDevolucion.U_VIS_CompleteOV = reader["U_VIS_CompleteOV"].ToString();
                        ObjSolicitudDevolucion.U_VIS_OVRejected = reader["U_VIS_OVRejected"].ToString();
                        ObjSolicitudDevolucion.U_VIS_ApprovedBy = reader["U_VIS_ApprovedBy"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_CONSOL = reader["U_SYP_DT_CONSOL"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_FCONSOL = reader["U_SYP_DT_FCONSOL"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_HCONSOL = reader["U_SYP_DT_HCONSOL"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_CORRDES = reader["U_SYP_DT_CORRDES"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_FCDES = reader["U_SYP_DT_FCDES"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_OCUR = returnReasonText;
                        ObjSolicitudDevolucion.U_SYP_DT_AYUDANTE = reader["U_SYP_DT_AYUDANTE"].ToString();
                        ObjSolicitudDevolucion.U_SYP_DT_ESTDES = reader["U_SYP_DT_ESTDES"].ToString();
                        ObjSolicitudDevolucion.U_VIS_AgencyCode = reader["U_VIS_AgencyCode"].ToString();
                        ObjSolicitudDevolucion.U_VIS_AgencyRUC = reader["U_VIS_AgencyRUC"].ToString();
                        ObjSolicitudDevolucion.U_VIS_AgencyName = reader["U_VIS_AgencyName"].ToString();
                        ObjSolicitudDevolucion.U_VIS_AgencyDir = reader["U_VIS_AgencyDir"].ToString();
                        ObjSolicitudDevolucion.U_VIST_REVASIDSCTO = reader["U_VIST_REVASIDSCTO"].ToString();
                        ObjSolicitudDevolucion.U_VIS_AgencyUbigeo = reader["U_VIS_AgencyUbigeo"].ToString();
                        ObjSolicitudDevolucion.U_SYP_VIST_TG = reader["U_SYP_VIST_TG"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEHASH = reader["U_SYP_FEHASH"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FERESP = reader["U_SYP_FERESP"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEEST = reader["U_SYP_FEEST"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEESUNAT =Convert.ToInt32(reader["U_SYP_FEESUNAT"].ToString());
                        ObjSolicitudDevolucion.U_SYP_FECDR = reader["U_SYP_FECDR"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEXML = reader["U_SYP_FEXML"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FETO = reader["U_SYP_FETO"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEEE = Convert.ToInt32(reader["U_SYP_FEEE"].ToString());
                        ObjSolicitudDevolucion.U_SYP_FEMEX = Convert.ToInt32(reader["U_SYP_FEMEX"].ToString());
                        ObjSolicitudDevolucion.U_SYP_FEMB = reader["U_SYP_FEMB"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEANT = Convert.ToInt32(reader["U_SYP_FEANT"].ToString());
                        ObjSolicitudDevolucion.U_SYP_FEGIT = Convert.ToInt32(reader["U_SYP_FEGIT"].ToString());
                        ObjSolicitudDevolucion.U_SYP_FEGPB = Convert.ToDouble(reader["U_SYP_FEGPB"].ToString());
                        ObjSolicitudDevolucion.U_SYP_FEGMT = reader["U_SYP_FEGMT"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEGFI = reader["U_SYP_FEGFI"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEGFE = reader["U_SYP_FEGFE"].ToString();
                        ObjSolicitudDevolucion.U_VIS_TypeRequest = reader["U_VIS_TypeRequest"].ToString();
                        ObjSolicitudDevolucion.U_VIS_TransferType = reader["U_VIS_TransferType"].ToString();
                        ObjSolicitudDevolucion.U_VIS_MTDev = reader["U_VIS_MTDev"].ToString();
                        ObjSolicitudDevolucion.U_VIS_MTReq = reader["U_VIS_MTReq"].ToString();
                        ObjSolicitudDevolucion.U_LB_WITHCC = reader["U_LB_WITHCC"].ToString();
                        ObjSolicitudDevolucion.U_VIS_AppVersion = reader["U_VIS_AppVersion"].ToString();
                        ObjSolicitudDevolucion.U_VIS_Intent = Convert.ToDouble(reader["U_VIS_Intent"].ToString());
                        ObjSolicitudDevolucion.U_VIS_Brand = reader["U_VIS_Brand"].ToString();
                        ObjSolicitudDevolucion.U_VIS_Model = reader["U_VIS_Model"].ToString();
                        ObjSolicitudDevolucion.U_VIS_Version = reader["U_VIS_Version"].ToString();
                        ObjSolicitudDevolucion.U_SYP_GUIARESU = reader["U_SYP_GUIARESU"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEPDF = reader["U_SYP_FEPDF"].ToString();
                        ObjSolicitudDevolucion.U_CancelReason = reader["U_CancelReason"].ToString();
                        ObjSolicitudDevolucion.U_CancelReasonOV = reader["U_CancelReasonOV"].ToString();
                        ObjSolicitudDevolucion.U_Transfered = reader["U_Transfered"].ToString();
                        ObjSolicitudDevolucion.U_TaskMigrate = reader["U_TaskMigrate"].ToString();
                        ObjSolicitudDevolucion.U_VIS_Alert1 = reader["U_VIS_Alert1"].ToString();
                        ObjSolicitudDevolucion.U_VIS_Alert2 = reader["U_VIS_Alert2"].ToString();
                        ObjSolicitudDevolucion.U_VIS_Alert3 = reader["U_VIS_Alert3"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEGUC = reader["U_SYP_FEGUC"].ToString();
                        ObjSolicitudDevolucion.U_VIS_Flete = Convert.ToInt32(reader["U_VIS_Flete"].ToString());
                        ObjSolicitudDevolucion.U_VIS_Draft1 = reader["U_VIS_Draft1"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEEB = reader["U_SYP_FEEB"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEGES = reader["U_SYP_FEGES"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEGAF = reader["U_SYP_FEGAF"].ToString();
                        ObjSolicitudDevolucion.U_SYP_FEGRD = reader["U_SYP_FEGRD"].ToString();
                        ObjSolicitudDevolucion.U_Confirma_Pedido_Dup = reader["U_Confirma_Pedido_Dup"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MOTND = reader["U_SYP_MOTND"].ToString();
                        ObjSolicitudDevolucion.U_SYP_TPOND = reader["U_SYP_TPOND"].ToString();
                        ObjSolicitudDevolucion.U_VIS_TypeRequest ="03";
                        ObjSolicitudDevolucion.U_SYP_MDTO = reader["U_SYP_MDTD"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDSO = reader["U_SYP_MDSD"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MDCO = reader["U_SYP_MDCD"].ToString();
                        ObjSolicitudDevolucion.U_SYP_MOTNC = returnReasonText;
                        ObjSolicitudDevolucion.DocumentLines = Detalle(DocEntry);
                        ObjSolicitudDevolucion.U_SYP_TPONC = "06";
                    }

                    dynamic Json = JsonConvert.SerializeObject(ObjSolicitudDevolucion);

                    var response = await serviceLayer.Request(string.Format("/b1s/v1/ReturnRequest"), Method.POST, Json, Token);

                    string XD = ""; 
                    var XX = response.Data.Content.ReadAsStringAsync();
                  //  correoAlert.EnviarCorreoOffice365("Prueba API Ventas " + "Generacion POST de SLD Vistony",
                   //               "DocEntry Entrega " + XX);
                }

                

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                correoAlert.EnviarCorreoOffice365("Prueba API Ventas " + "Generacion de SLD Vistony",
                    ex.Message.ToString());
            }
            connection.Close();
            return rs;
        }

        public List<DocumentLineSD> Detalle(string DocEntry)
        {
            List<DocumentLineSD> LsDocumentLinew = new List<DocumentLineSD>();
            DocumentLineSD documentLine = new DocumentLineSD();
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            string strSQL = string.Format("CALL {0}.APP_SOLI_DEV_D('{1}')", DataSource.bd(), DocEntry);
            //   SoliDevBO ObjSoliDevBO = new SoliDevBO();
            SolicitudDevolucion ObjSolicitudDevolucion = new SolicitudDevolucion();

            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    documentLine = new DocumentLineSD();
                    documentLine.BaseType=13;
                    documentLine.BaseEntry =Convert.ToInt32(reader["DocEntry"].ToString());
                    documentLine.BaseLine =Convert.ToInt32(reader["LineNum"].ToString());
                    documentLine.LineNum = Convert.ToInt32(reader["LineNum"].ToString());
                    documentLine.ItemCode = reader["ItemCode"].ToString();
                    documentLine.Quantity =Convert.ToDouble(reader["Quantity"].ToString());
                    documentLine.Price = Convert.ToDouble(reader["Price"].ToString());
                    documentLine.DiscountPercent = Convert.ToDouble(reader["DiscPrcnt"].ToString());
                    documentLine.WarehouseCode = reader["WhsCode"].ToString();
                    documentLine.AccountCode = reader["AcctCode"].ToString();
                    documentLine.CostingCode = reader["OcrCode"].ToString();
                    documentLine.TaxCode = reader["TaxCode"].ToString();
                    documentLine.COGSCostingCode = reader["OcrCode"].ToString();
                    documentLine.CostingCode2 = reader["OcrCode2"].ToString();
                    documentLine.CostingCode3 = reader["OcrCode3"].ToString();
                    documentLine.COGSCostingCode2 = reader["OcrCode2"].ToString();
                    documentLine.COGSCostingCode3 = reader["OcrCode3"].ToString();
                    documentLine.U_SYP_HASPROV = reader["U_SYP_HASPROV"].ToString();
                    documentLine.U_SYP_FECAT07 = reader["U_SYP_FECAT07"].ToString();
                    documentLine.U_VIST_CTAINGDCTO = reader["U_VIST_CTAINGDCTO"].ToString();
                    documentLine.U_VIS_PromID = reader["U_VIS_PromID"].ToString();
                    documentLine.U_VIS_PromLineID = reader["U_VIS_PromLineID"].ToString();
                    documentLine.TaxOnly = reader["TaxOnly"].ToString();
                    LsDocumentLinew.Add(documentLine);
                }
            }
            return LsDocumentLinew;
        }


        public void ActualizarAvance(string DocEntryRuta)
        {
            HanaConnection connection = GetConnection();
            try
            {
                string strSQL = string.Format("CALL {0}.VIS_UPDATE_STATUS_DESPACHOS('{1}')",
                            DataSource.bd(), DocEntryRuta);
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }


        public async Task<ResponseData> update(InDispatchList dispatchList)
        {

            HanaConnection connection = GetConnection();
            List<DispatchResponseDetalle> dispatchResponseListDetalle = new();
            List<DispatchResponse> dispatchResponseList = new();

            DispatchResponseDetalle dispatchResponseDetalle = new();

            try
            {

            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);

            foreach (InDispatch dispatch in dispatchList.Dispatch)
            {

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
                    U_U_ReturnReasonText = d.ReturnReasonText,
                    U_Delivered = d.Delivered,
                    U_UserName = d.UserName,
                    U_UserCode = d.UserCode,
                    U_Latitude = d.Latitude,
                    U_Longitude = d.Longitude,
                    U_DocEntry = d.DeliveryNotes
                }).ToList();

                DispatchResponse dispatchResponse = new();
                dispatchResponse.DocEntry = dispatch.DocEntry;
                
                
                foreach (VIS_DIS_Drt1collection temp in listTemp.VIS_DIS_DRT1Collection)
                { 
                    string deliveryNote = temp.U_DocEntry;
                    string returnReasonText = temp.U_U_ReturnReasonText;

                    string jsonString = "{\"VIS_DIS_DRT1Collection\":[" + JsonConvert.SerializeObject(temp) + "]}";

                        // Deserializamos el JSON a un JObject
                        JObject data = JObject.Parse(jsonString);

                        // Accedemos directamente a la propiedad 'U_PhotoStore' del primer objeto
                        var photoStore = data["VIS_DIS_DRT1Collection"]?[0]?["U_PhotoStore"];  
                        
                        // Validamos si existe el nodo 'Details' dentro de 'Dispatch'
                        var U_CheckInTime = data["VIS_DIS_DRT1Collection"]?[0]?["U_CheckInTime"];

                        // Validamos si existe 'PhotoDocument'
                        if (photoStore.ToString() !=String.Empty)
                        {

                            string U_PhotoDocument = data["VIS_DIS_DRT1Collection"]?[0]?["U_PhotoDocument"].ToString();
                            string U_PhotoStore = data["VIS_DIS_DRT1Collection"]?[0]?["U_PhotoStore"].ToString();
                            string LineId = data["VIS_DIS_DRT1Collection"]?[0]?["LineId"].ToString();
                            string DocEntry = data["VIS_DIS_DRT1Collection"]?[0]?["DocEntry"].ToString();


                            string strSQL = string.Format("UPDATE {0}.\"@VIS_DIS_DRT1\" SET \"U_PhotoDocument\" = '{1}'," +
                                                          "\"U_PhotoStore\" = '{2}' " +
                                        " WHERE \"DocEntry\" ='{3}' AND  \"LineId\" ='{4}'",
                            DataSource.bd(), U_PhotoDocument, U_PhotoStore, DocEntry, LineId);
                            connection.Open();
                            HanaCommand command = new HanaCommand(strSQL, connection);
                            HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }

                        }
                        else if (U_CheckInTime.ToString() != String.Empty)
                        {
                            string U_CheckInTime2 = data["VIS_DIS_DRT1Collection"]?[0]?["U_CheckInTime"].ToString().Substring(0, 4);
                            string U_CheckOutTime = data["VIS_DIS_DRT1Collection"]?[0]?["U_CheckOutTime"].ToString().Substring(0, 4);
                            string DocEntry = data["VIS_DIS_DRT1Collection"]?[0]?["DocEntry"].ToString();
                            string U_Latitude = data["VIS_DIS_DRT1Collection"]?[0]?["U_Latitude"].ToString();
                            string U_Longitude = data["VIS_DIS_DRT1Collection"]?[0]?["U_Longitude"].ToString();
                            string LineId = data["VIS_DIS_DRT1Collection"]?[0]?["LineId"].ToString();

                            string strSQL = string.Format("UPDATE {0}.\"@VIS_DIS_DRT1\" SET \"U_CheckInTime\" = '{1}'," +
                                                          "\"U_CheckOutTime\" = '{2}',\"U_Latitude\"  = '{3}'," +
                                                          "\"U_Longitude\" = '{4}'"+
                                                          " WHERE \"DocEntry\" ='{5}' AND  \"LineId\" ='{6}'",

                           DataSource.bd(), U_CheckInTime2, U_CheckOutTime, U_Latitude, U_Longitude, DocEntry, LineId);

                            connection.Open();
                            HanaCommand command = new HanaCommand(strSQL, connection);
                            HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }

                            //ActualizarAvance(DocEntry);

                        }
                        else
                        {
                            string U_Comments = data["VIS_DIS_DRT1Collection"]?[0]?["U_Comments"].ToString();
                            string U_Delivered = data["VIS_DIS_DRT1Collection"]?[0]?["U_Delivered"].ToString();
                            string U_ReturnReason = data["VIS_DIS_DRT1Collection"]?[0]?["U_ReturnReason"].ToString();

                            string U_U_ReturnReasonText = data["VIS_DIS_DRT1Collection"]?[0]?["U_U_ReturnReasonText"].ToString();
                            string ReturnReasonText = data["VIS_DIS_DRT1Collection"]?[0]?["U_U_ReturnReasonText"].ToString();
                            string UserName = data["VIS_DIS_DRT1Collection"]?[0]?["U_UserName"].ToString();
                            string UserCode = data["VIS_DIS_DRT1Collection"]?[0]?["U_UserCode"].ToString();

                            string strSQL = string.Format("UPDATE {0}.\"@VIS_DIS_DRT1\" SET \"U_Comments\" = '{1}'," +
                                                            "\"U_Delivered\" = '{2}',\"U_ReturnReason\"  = '{3}'," +
                                                            "\"U_U_ReturnReasonText\" = '{4}', \"U_UserName\" = '{5}'," +
                                                            "\"U_UserCode\" = '{6}' WHERE \"DocEntry\" ='{7}' AND  \"U_DocEntry\" ='{8}'",

                             DataSource.bd(), U_Comments, U_Delivered, U_ReturnReason, U_U_ReturnReasonText, UserName, UserCode, dispatch.DocEntry,deliveryNote);
                            connection.Open();
                            HanaCommand command = new HanaCommand(strSQL, connection);
                            HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }

                            //COMENTADO WARAGON (DEMORA EN ACTUALIZAR ENTREGA) new Task(async () => await updateStatusDeliveryNotesOrInvoices( deliveryNote, temp.U_Delivered, returnReasonText, sl.token)).Start();
                            strSQL = string.Format("UPDATE {0}.\"ODLN\" SET \"U_SYP_DT_ESTDES\"='{1}', \"U_SYP_DT_OCUR\"='{2}' WHERE \"DocEntry\"='{3}'",
                                 DataSource.bd(), U_Delivered, U_U_ReturnReasonText, deliveryNote);
                            connection.Open();
                             command = new HanaCommand(strSQL, connection);
                             reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }

                            if (temp.U_Delivered == "A")
                            {

                                LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();

                                //MANTENER COMENTADO HASTA QUE SALGA A PRODUCCION LA GENERACION DE SOLICITUD DE DEVOLUCION AUTOMATICA
                             //   await new Task(async () => await 
                                      await SolicitudDevolucion(deliveryNote, returnReasonText, sl.token);
                                     //COMENTADO POR CIERRE 
                             //QA
                            }
                            
                            //ActualizarAvance(Convert.ToString(dispatch.DocEntry));
                        }

                        dispatchResponseDetalle.LineId = temp.LineId;
                        dispatchResponseDetalle.Message = "Documento actualizado correctamente";
                        dispatchResponseDetalle.ErrorCode = "N";

                        dispatchResponseListDetalle.Add(dispatchResponseDetalle);

                    }

                dispatchResponse.Details= dispatchResponseListDetalle;
                dispatchResponseList.Add(dispatchResponse);

                await recountDeliveredSucess( dispatch.DocEntry);
            }

            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Despacho update Vistony", ex.Message.ToString());
            }
            finally
            {
                user.LogoutServiceLayer().GetAwaiter().GetResult();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
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
