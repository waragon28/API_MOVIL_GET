
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Configuration;
using System.Data;
using SAP_Core.Utils;
using SalesForce.Util;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using SalesForce.BO;
using static WebApiNetCore.Utils.Other;
using System.Net;
using Newtonsoft.Json;
using WebApiNetCore.BO;
using System.Runtime.ConstrainedExecution;
using Sentry;
using WebApiNetCore.Utils;

namespace SAP_Core.DAL
{
    public class CobranzaDDAL : Connection,IDisposable
    {
        private ServiceLayer serviceLayer;
        UsuarioDAL user = new UsuarioDAL();
        public CobranzaDDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }
        public ReciboSAP SetCollection(JObject obj)
        {
            var validacion = new Verification()
            {
                CardCode = obj["CardCode"].ToString(),
                IncomeDate = obj["IncomeDate"].ToString(),
                DocEntry = obj["DocEntryFT"].ToString(),
                Receip = obj["Receip"].ToString(),
                SlpCode = obj["SlpCode"].ToString(),
            };

            ReciboSAP rs = new(validacion);
            rs.U_VIS_BankID = obj["BankID"].ToString();
            rs.U_VIS_Deposit = obj["Deposit"].ToString();
            rs.U_VIS_IncomeDate = obj["IncomeDate"].ToString();
            rs.U_VIS_Banking = obj["Banking"].ToString();
            rs.U_VIS_ItemDetail = obj["ItemDetail"].ToString();
            rs.U_VIS_CardCode = obj["CardCode"].ToString();
            rs.U_VIS_DocEntry = obj["DocEntryFT"].ToString();
            rs.U_VIS_DocNum = obj["DocNum"].ToString();
            rs.U_VIS_DocTotal = Convert.ToDecimal(obj["DocTotal"].ToString());
            rs.U_VIS_Balance = Convert.ToDecimal(obj["Balance"].ToString());
            rs.U_VIS_AmountCharged = Convert.ToDecimal(obj["AmountCharged"].ToString());
            rs.U_VIS_NewBalance = Convert.ToDecimal(obj["NewBalance"].ToString());
            rs.U_VIS_Receip = Convert.ToInt32(obj["Receip"].ToString());
            rs.U_VIS_AppVersion = obj["AppVersion"].ToString();
            rs.U_VIS_Status = obj["Status"].ToString();
            rs.U_VIS_QRStatus = obj["QRStatus"].ToString();
            rs.U_VIS_Comments = obj["Commentary"].ToString();
            rs.U_VIS_UserID = obj["UserID"].ToString();
            rs.U_VIS_SlpCode = obj["SlpCode"].ToString();
            rs.U_VIS_CancelReason = obj["CancelReason"].ToString();
            rs.U_VIS_DirectDeposit = obj["DirectDeposit"].ToString();
            rs.U_VIS_POSPay = obj["POSPay"].ToString();
            rs.U_VIS_IncomeTime = obj["IncomeTime"].ToString();
            rs.U_VIS_Intent = obj["Intent"].ToString();
            rs.U_VIS_Brand = obj["Brand"].ToString();
            rs.U_VIS_Model = obj["Model"].ToString();
            rs.U_VIS_Version = obj["OSVersion"].ToString();
            rs.U_VIS_CollectionSalesperson = obj["U_VIS_CollectionSalesperson"] == null ? null : obj["U_VIS_CollectionSalesperson"].ToString();
            rs.U_VIS_Type = obj["U_VIS_Type"] == null ? null : obj["U_VIS_Type"].ToString();

            return rs;
        }
        public async Task<ResponseData> insert(string jsonPayload)
        {

            LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();
            ResponseData response = await serviceLayer.Request("/b1s/v1/VIST_COBRANZA", Method.POST, jsonPayload, sl.token);

            try
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    dynamic responseBody = await response.Data.Content.ReadAsStringAsync();

                    responseBody = JsonConvert.DeserializeObject(responseBody.ToString());

                    response.Data = responseBody["Code"];
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {

                    ResponseError error = null;

                    if (response.Data != null)
                    {
                        var responseBody = await response.Data.Content.ReadAsStringAsync();
                        error = System.Text.Json.JsonSerializer.Deserialize<ResponseError>(responseBody);
                    }

                    response.StatusCode = HttpStatusCode.FailedDependency;
                    response.Data = error;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            finally
            {
                user.LogoutServiceLayer().GetAwaiter().GetResult();
            }

            return response;
        }
        public ListaCobranzaD GetCollections(string imei, string fecha)
        {
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listaCobranzaD = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS ('{1}','{2}')", DataSource.bd(), imei, fecha);
            
            try
            {
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cobranzaD = new CobranzaDBO();
                                    cobranzaD.Code = Other.GetStringValue(reader,"Code");
                                    cobranzaD.DocEntry = Other.GetStringValue(reader, "DocEntry");
                                    cobranzaD.DocEntryFT = Other.GetStringValue(reader, "DocEntryFT");
                                    cobranzaD.BankName = Other.GetStringValue(reader,"BankName");
                                    cobranzaD.BankID = Other.GetStringValue(reader,"BankID");
                                    cobranzaD.DepositID = Other.GetStringValue(reader,"Deposit");
                                    cobranzaD.ItemDetail = Other.GetStringValue(reader,"ItemDetail");
                                    cobranzaD.CardCode = Other.GetStringValue(reader,"CardCode");
                                    cobranzaD.DocNum = Other.GetStringValue(reader,"DocNum");
                                    cobranzaD.DocTotal = Other.GetDecimalValue(reader, "DocTotal");
                                    cobranzaD.Balance = Other.GetDecimalValue(reader, "Balance");
                                    cobranzaD.AmountCharged = Other.GetDecimalValue(reader, "AmountCharged");
                                    cobranzaD.NewBalance = Other.GetDecimalValue(reader, "NewBalance");
                                    cobranzaD.IncomeDate = Other.GetStringValue(reader,"IncomeDate");
                                    cobranzaD.Receip = Other.GetIntValue(reader, "Receip");
                                    cobranzaD.Status = Other.GetStringValue(reader,"Status");
                                    cobranzaD.Commentary = Other.GetStringValue(reader,"Comments").ToUpper();
                                    cobranzaD.UserCode = Other.GetStringValue(reader,"UserID");
                                    cobranzaD.SlpCode = Other.GetStringValue(reader,"SlpCode");
                                    cobranzaD.CardName = Other.GetStringValue(reader,"CardName").ToUpper();
                                    cobranzaD.LegalNumber = Other.GetStringValue(reader,"NumAtCard");
                                    cobranzaD.Banking = Other.GetStringValue(reader,"Banking");
                                    cobranzaD.CancellationReason = Other.GetStringValue(reader,"CancelReason").ToUpper();
                                    cobranzaD.QRStatus = Other.GetStringValue(reader,"QRStatus");
                                    cobranzaD.DirectDeposit = Other.GetStringValue(reader,"DirectDeposit");
                                    cobranzaD.POSPay = Other.GetStringValue(reader,"POSPay");
                                    cobranzaD.U_VIS_CollectionSalesperson = Other.GetStringValue(reader,"U_VIS_CollectionSalesperson");
                                    cobranzaD.U_VIS_Type = Other.GetStringValue(reader,"U_VIS_Type");
                                    listaCobranzaD.Add(cobranzaD);
                                }
                            }
                        }
                    }
                }

                cobranzasD.CollectionDetail = listaCobranzaD;

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollections", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollections", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return cobranzasD;
        }
        public CobranzaDBO GetCollectionDetail(string imei, string recibo)
        {
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DETAIL ('{1}','{2}')", DataSource.bd(), imei, recibo);

            try
            {
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cobranzaD.Code = Other.GetStringValue(reader,"Code");
                                    cobranzaD.DocEntry = Other.GetStringValue(reader,"DocEntry");
                                    cobranzaD.BankID = Other.GetStringValue(reader,"BankID");
                                    cobranzaD.DepositID = Other.GetStringValue(reader,"Deposit");
                                    cobranzaD.ItemDetail = Other.GetStringValue(reader,"ItemDetail");
                                    cobranzaD.CardCode = Other.GetStringValue(reader,"CardCode");
                                    cobranzaD.DocEntryFT = Other.GetStringValue(reader, "DocEntryFT");
                                    cobranzaD.DocNum = Other.GetStringValue(reader,"DocNum");
                                    cobranzaD.DocTotal = Other.GetDecimalValue(reader,"DocTotal");
                                    cobranzaD.Balance = Other.GetDecimalValue(reader,"Balance");
                                    cobranzaD.AmountCharged =Other.GetDecimalValue(reader,"AmountCharged");
                                    cobranzaD.NewBalance = Other.GetDecimalValue(reader,"NewBalance");
                                    cobranzaD.IncomeDate = Other.GetStringValue(reader,"IncomeDate");
                                    cobranzaD.Receip = Other.GetIntValue(reader,"Receip");
                                    cobranzaD.Status = Other.GetStringValue(reader,"Status");
                                    cobranzaD.Commentary = Other.GetStringValue(reader,"Comments").ToUpper();
                                    cobranzaD.UserCode = Other.GetStringValue(reader,"UserID");
                                    cobranzaD.SlpCode = Other.GetStringValue(reader,"SlpCode");
                                    cobranzaD.CardName = Other.GetStringValue(reader,"CardName").ToUpper();
                                    cobranzaD.LegalNumber = Other.GetStringValue(reader,"NumAtCard");
                                    cobranzaD.Banking = Other.GetStringValue(reader,"Banking");
                                    cobranzaD.CancellationReason = Other.GetStringValue(reader,"CancelReason").ToUpper();
                                    cobranzaD.QRStatus = Other.GetStringValue(reader,"QRStatus");
                                    cobranzaD.DirectDeposit = Other.GetStringValue(reader,"DirectDeposit");
                                    cobranzaD.POSPay = Other.GetStringValue(reader,"POSPay");
                                    cobranzaD.U_VIS_CollectionSalesperson = Other.GetStringValue(reader,"U_VIS_CollectionSalesperson");
                                    cobranzaD.U_VIS_Type = Other.GetStringValue(reader,"U_VIS_Type");
                                }
                            }
                        }
                    }
                }

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDetail", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDetail", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return cobranzaD;
        }
        public List<CobranzaDBO> GetCollectionDocument(string imei, string docEntry)
        {
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DOCUMENT ('{1}','{2}')", DataSource.bd(), imei, docEntry);

            try
            {
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cobranzaD = new CobranzaDBO();
                                    cobranzaD.DocEntry =Other.GetStringValue(reader,"DocEntry");
                                    cobranzaD.BankID =Other.GetStringValue(reader,"BankID");
                                    cobranzaD.DepositID =Other.GetStringValue(reader,"Deposit");
                                    cobranzaD.ItemDetail =Other.GetStringValue(reader,"ItemDetail");
                                    cobranzaD.CardCode =Other.GetStringValue(reader,"CardCode");
                                    cobranzaD.DocEntryFT =Other.GetStringValue(reader,"DocEntryFT");
                                    cobranzaD.DocNum =Other.GetStringValue(reader,"DocNum");
                                    cobranzaD.DocTotal = Other.GetDecimalValue(reader, "DocTotal");
                                    cobranzaD.Balance = Other.GetDecimalValue(reader, "Balance");
                                    cobranzaD.AmountCharged = Other.GetDecimalValue(reader, "AmountCharged");
                                    cobranzaD.NewBalance = Other.GetDecimalValue(reader, "NewBalance");
                                    cobranzaD.IncomeDate =Other.GetStringValue(reader,"IncomeDate");
                                    cobranzaD.Receip = Other.GetIntValue(reader,"Receip");
                                    cobranzaD.Status =Other.GetStringValue(reader,"Status");
                                    cobranzaD.Commentary =Other.GetStringValue(reader,"Comments").ToUpper();
                                    cobranzaD.UserCode =Other.GetStringValue(reader,"UserID");
                                    cobranzaD.SlpCode =Other.GetStringValue(reader,"SlpCode");
                                    cobranzaD.CardName =Other.GetStringValue(reader,"CardName").ToUpper();
                                    cobranzaD.LegalNumber =Other.GetStringValue(reader,"NumAtCard");
                                    cobranzaD.Banking =Other.GetStringValue(reader,"Banking");
                                    cobranzaD.CancellationReason =Other.GetStringValue(reader,"CancelReason").ToUpper();
                                    cobranzaD.QRStatus =Other.GetStringValue(reader,"QRStatus");
                                    cobranzaD.DirectDeposit =Other.GetStringValue(reader,"DirectDeposit");
                                    cobranzaD.POSPay =Other.GetStringValue(reader,"POSPay");
                                    cobranzaD.U_VIS_CollectionSalesperson =Other.GetStringValue(reader,"U_VIS_CollectionSalesperson");
                                    cobranzaD.U_VIS_Type =Other.GetStringValue(reader,"U_VIS_Type");
                                    listCobranzaBO.Add(cobranzaD);
                                }
                            }
                        }
                    }
                }
                
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDocument", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDocument", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return listCobranzaBO;
        }
        public ListaCobranzaD GetCollectionStatus(string imei, string status)
        {
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL =  string.Format("CALL {0}.APP_COLLECTIONS_STATUS ('{1}','{2}')", DataSource.bd(), imei, status);

            try
            {
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cobranzaD = new CobranzaDBO();
                                    cobranzaD.Code =  Other.GetStringValue(reader,"Code");
                                    cobranzaD.DocEntry =  Other.GetStringValue(reader,"DocEntry");
                                    cobranzaD.BankID =  Other.GetStringValue(reader,"BankID");
                                    cobranzaD.DepositID =  Other.GetStringValue(reader,"Deposit");
                                    cobranzaD.ItemDetail =  Other.GetStringValue(reader,"ItemDetail");
                                    cobranzaD.CardCode =  Other.GetStringValue(reader,"CardCode");
                                    cobranzaD.DocEntryFT =  Other.GetStringValue(reader,"DocEntryFT");
                                    cobranzaD.DocNum =  Other.GetStringValue(reader,"DocNum");
                                    cobranzaD.DocTotal = Other.GetDecimalValue(reader,"DocTotal");
                                    cobranzaD.Balance =  Other.GetDecimalValue(reader,"Balance");
                                    cobranzaD.AmountCharged = Other.GetDecimalValue(reader,"AmountCharged");
                                    cobranzaD.NewBalance = Other.GetDecimalValue(reader,"NewBalance");
                                    cobranzaD.IncomeDate =  Other.GetStringValue(reader,"IncomeDate");
                                    cobranzaD.Receip = Other.GetIntValue(reader,"Receip");
                                    cobranzaD.Status =  Other.GetStringValue(reader,"Status");
                                    cobranzaD.Commentary =  Other.GetStringValue(reader,"Comments").ToUpper();
                                    cobranzaD.UserCode =  Other.GetStringValue(reader,"UserID");
                                    cobranzaD.SlpCode =  Other.GetStringValue(reader,"SlpCode");
                                    cobranzaD.CardName =  Other.GetStringValue(reader,"CardName").ToUpper();
                                    cobranzaD.LegalNumber =  Other.GetStringValue(reader,"NumAtCard");
                                    cobranzaD.Banking =  Other.GetStringValue(reader,"Banking");
                                    cobranzaD.CancellationReason =  Other.GetStringValue(reader,"CancelReason").ToUpper();
                                    cobranzaD.QRStatus =  Other.GetStringValue(reader,"QRStatus");
                                    cobranzaD.DirectDeposit =  Other.GetStringValue(reader,"DirectDeposit");
                                    cobranzaD.POSPay =  Other.GetStringValue(reader,"POSPay");
                                    cobranzaD.U_VIS_CollectionSalesperson =  Other.GetStringValue(reader,"U_VIS_CollectionSalesperson");
                                    cobranzaD.U_VIS_Type =  Other.GetStringValue(reader,"U_VIS_Type");
                                    listCobranzaBO.Add(cobranzaD);
                                }
                            }
                        }
                    }
                }

                cobranzasD.CollectionDetail = listCobranzaBO;


            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionStatus", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionStatus", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return cobranzasD;
        }
        public ListaCobranzaD GetCollectionStatus2(string imei, string status, string user)
        {
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_STATUS2 ('{1}','{2}','{3}')", DataSource.bd(), imei, status, user);

            try {
                 using (HanaConnection connection = GetConnection())
                 {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cobranzaD = new CobranzaDBO();
                                    cobranzaD.Code = Other.GetStringValue(reader,"Code");
                                    cobranzaD.DocEntry = Other.GetStringValue(reader,"DocEntry");
                                    cobranzaD.BankID = Other.GetStringValue(reader,"BankID");
                                    cobranzaD.DepositID = Other.GetStringValue(reader,"Deposit");
                                    cobranzaD.ItemDetail = Other.GetStringValue(reader,"ItemDetail");
                                    cobranzaD.CardCode = Other.GetStringValue(reader,"CardCode");
                                    cobranzaD.DocEntryFT = Other.GetStringValue(reader,"DocEntryFT");
                                    cobranzaD.DocNum = Other.GetStringValue(reader,"DocNum");
                                    cobranzaD.DocTotal =Other.GetDecimalValue(reader,"DocTotal");
                                    cobranzaD.Balance = Other.GetDecimalValue(reader,"Balance");
                                    cobranzaD.AmountCharged =Other.GetDecimalValue(reader,"AmountCharged");
                                    cobranzaD.NewBalance = Other.GetDecimalValue(reader,"NewBalance");
                                    cobranzaD.IncomeDate = Other.GetStringValue(reader,"IncomeDate");
                                    cobranzaD.Receip =Other.GetIntValue(reader,"Receip");
                                    cobranzaD.Status = Other.GetStringValue(reader,"Status");
                                    cobranzaD.Commentary = Other.GetStringValue(reader,"Comments").ToUpper();
                                    cobranzaD.UserCode = Other.GetStringValue(reader,"UserID");
                                    cobranzaD.SlpCode = Other.GetStringValue(reader,"SlpCode");
                                    cobranzaD.CardName = Other.GetStringValue(reader,"CardName").ToUpper();
                                    cobranzaD.LegalNumber = Other.GetStringValue(reader,"NumAtCard");
                                    cobranzaD.Banking = Other.GetStringValue(reader,"Banking");
                                    cobranzaD.CancellationReason = Other.GetStringValue(reader,"CancelReason").ToUpper();
                                    cobranzaD.QRStatus = Other.GetStringValue(reader,"QRStatus");
                                    cobranzaD.DirectDeposit = Other.GetStringValue(reader,"DirectDeposit");
                                    cobranzaD.POSPay = Other.GetStringValue(reader,"POSPay");
                                    cobranzaD.U_VIS_CollectionSalesperson = Other.GetStringValue(reader,"U_VIS_CollectionSalesperson");
                                    cobranzaD.U_VIS_Type = Other.GetStringValue(reader,"U_VIS_Type");
                                    listCobranzaBO.Add(cobranzaD);
                                }
                            }
                        }
                    }
                }


            cobranzasD.CollectionDetail = listCobranzaBO;

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionStatus2", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionStatus2", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return cobranzasD;
        }
        public List<DepositsStatus2> GetCollectionDepositStatus(string imei, string status)
        {
            DepositsStatus2 cobranzasStatus = new DepositsStatus2();
            List<DepositsStatus2>  listCobranzaBO = new List<DepositsStatus2>();
            string strSQL = string.Format("CALL {0}.APP_DEPOSITS_STATUS ('{1}','{2}')", DataSource.bd(), imei, status);

            try
            {
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cobranzasStatus = new DepositsStatus2();
                                    cobranzasStatus.Code = Other.GetStringValue(reader,"Code");
                                    cobranzasStatus.U_VIS_BankID = Other.GetStringValue(reader,"U_VIS_BankID");
                                    cobranzasStatus.BankName = Other.GetStringValue(reader,"BankName");
                                    cobranzasStatus.U_VIS_IncomeType = Other.GetStringValue(reader,"U_VIS_IncomeType");
                                    cobranzasStatus.U_VIS_Deposit = Other.GetStringValue(reader,"U_VIS_Deposit");
                                    cobranzasStatus.U_VIS_Date = Other.GetStringValue(reader,"U_VIS_Date");
                                    cobranzasStatus.U_VIS_DeferredDate = Other.GetStringValue(reader,"U_VIS_DeferredDate");
                                    cobranzasStatus.U_VIS_Banking = Other.GetStringValue(reader,"U_VIS_Banking");
                                    cobranzasStatus.U_VIS_UserID = Other.GetStringValue(reader,"U_VIS_UserID");
                                    cobranzasStatus.U_VIS_SlpCode = Other.GetStringValue(reader,"U_VIS_SlpCode");
                                    cobranzasStatus.U_VIS_AmountDeposit = Other.GetStringValue(reader,"U_VIS_AmountDeposit");
                                    cobranzasStatus.U_VIS_Comments = Other.GetStringValue(reader,"U_VIS_Comments");
                                    cobranzasStatus.U_VIS_CancelReason = Other.GetStringValue(reader,"U_VIS_CancelReason").ToUpper();
                                    cobranzasStatus.U_VIS_DirectDeposit = Other.GetStringValue(reader,"U_VIS_DirectDeposit");
                                    cobranzasStatus.U_VIS_POSPay = Other.GetStringValue(reader,"U_VIS_POSPay");
                                    cobranzasStatus.U_VIS_CollectionSalesPerson = Other.GetStringValue(reader,"U_VIS_CollectionSalesPerson");
                                    cobranzasStatus.U_VIS_BankValidation = Other.GetStringValue(reader,"U_VIS_BankValidation");
                                    cobranzasStatus.U_VIS_ObservationStatus = Other.GetStringValue(reader,"U_VIS_ObservationStatus");

                                    listCobranzaBO.Add(cobranzasStatus);
                                }
                            }
                        }
                    }
                }


            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDepositStatus", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDepositStatus", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return listCobranzaBO;
        }
        public ListaCobranzaD GetCollectionDeposit(string imei, string deposit)
        {
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DEPOSIT ('{1}','{2}')", DataSource.bd(), imei, deposit);

            try {
                
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cobranzaD = new CobranzaDBO();
                                    cobranzaD.Code = Other.GetStringValue(reader,"Code");
                                    cobranzaD.BankID = Other.GetStringValue(reader,"BankID");
                                    cobranzaD.DepositID = Other.GetStringValue(reader,"Deposit");
                                    cobranzaD.ItemDetail = Other.GetStringValue(reader,"ItemDetail");
                                    cobranzaD.CardCode = Other.GetStringValue(reader,"CardCode");
                                    cobranzaD.DocNum = Other.GetStringValue(reader,"DocNum");
                                    cobranzaD.DocTotal = Other.GetDecimalValue(reader,"DocTotal");
                                    cobranzaD.Balance = Other.GetDecimalValue(reader, "Balance");
                                    cobranzaD.AmountCharged = Other.GetDecimalValue(reader, "AmountCharged");
                                    cobranzaD.NewBalance = Other.GetDecimalValue(reader, "NewBalance");
                                    cobranzaD.IncomeDate = Other.GetStringValue(reader,"IncomeDate");
                                    cobranzaD.Receip = Other.GetIntValue(reader, "Receip");
                                    cobranzaD.Status = Other.GetStringValue(reader,"Status");
                                    cobranzaD.Commentary = Other.GetStringValue(reader,"Comments").ToUpper();
                                    cobranzaD.UserCode = Other.GetStringValue(reader,"UserID");
                                    cobranzaD.SlpCode = Other.GetStringValue(reader,"SlpCode");
                                    cobranzaD.CardName = Other.GetStringValue(reader,"CardName").ToUpper();
                                    cobranzaD.LegalNumber = Other.GetStringValue(reader,"NumAtCard");
                                    cobranzaD.Banking = Other.GetStringValue(reader,"Banking");
                                    cobranzaD.CancellationReason = Other.GetStringValue(reader,"CancelReason").ToUpper();
                                    cobranzaD.QRStatus = Other.GetStringValue(reader,"QRStatus");
                                    cobranzaD.DirectDeposit = Other.GetStringValue(reader,"DirectDeposit");
                                    cobranzaD.POSPay = Other.GetStringValue(reader,"POSPay");
                                    cobranzaD.U_VIS_CollectionSalesperson = Other.GetStringValue(reader,"U_VIS_CollectionSalesperson");
                                    cobranzaD.U_VIS_Type = Other.GetStringValue(reader,"U_VIS_Type");
                                    listCobranzaBO.Add(cobranzaD);
                                }
                            }
                        }
                    }
                }
                cobranzasD.CollectionDetail = listCobranzaBO;

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDeposit", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCollectionDeposit", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return cobranzasD;
        }
        public ReceipResponse GetValidCollections(string IncomeDate, string CardCode, string DocEntry, string Receip, string SlpCode)
        {
            ReceipResponse s = new ReceipResponse();
            string strSQL = string.Format("CALL {0}.APP_VALID_COLLECTION ('{1}','{2}','{3}','{4}',{5})", DataSource.bd(), IncomeDate, CardCode, DocEntry, Receip, SlpCode);

            try{
                    using (HanaConnection connection = GetConnection())
                    {
                        connection.Open();
                        using (HanaCommand command = new HanaCommand(strSQL, connection))
                        {
                            using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        s = new ReceipResponse();
                                        s.ErrorCode = "0";
                                        s.Receip = reader["Receip"].ToString();
                                        s.Message = "Cobranza " + reader["ItemDetail"].ToString() + " registrada satisfactoriamente";
                                        s.ItemDetail = reader["ItemDetail"].ToString();
                                        s.Code = reader["Code"].ToString();
                                    }
                                }
                            }
                        }
                    }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetValidCollections", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetValidCollections", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return s;
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
        ~CobranzaDDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
