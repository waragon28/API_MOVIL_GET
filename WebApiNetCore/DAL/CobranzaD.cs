
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

namespace SAP_Core.DAL
{
    public class CobranzaDDAL : Connection,IDisposable
    {
        private ServiceLayer serviceLayer;
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
            ResponseData response = await serviceLayer.Request("/b1s/v1/VIST_COBRANZA", Method.POST, jsonPayload);

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

            return response;
        }
        public ListaCobranzaD GetCollections(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listaCobranzaD = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS ('{1}','{2}')", DataSource.bd(), imei, fecha);
            
            try
            {
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
                        cobranzaD = new CobranzaDBO();
                        cobranzaD.Code = reader["Code"].ToString();
                        cobranzaD.BankName = reader["BankName"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
                        cobranzaD.U_VIS_CollectionSalesperson = reader["U_VIS_CollectionSalesperson"].ToString();
                        cobranzaD.U_VIS_Type = reader["U_VIS_Type"].ToString();
                        listaCobranzaD.Add(cobranzaD);
                    }
                }
                cobranzasD.CollectionDetail = listaCobranzaD;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetCollections - " + ex.Message + " - " + imei + " " + fecha);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return cobranzasD;
        }
        public CobranzaDBO GetCollectionDetail(string imei, string recibo)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DETAIL ('{1}','{2}')", DataSource.bd(), imei, recibo);

            try
            {
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cobranzaD.Code = reader["Code"].ToString();
                        cobranzaD.DocEntry = reader["DocEntry"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
                        cobranzaD.U_VIS_CollectionSalesperson = reader["U_VIS_CollectionSalesperson"].ToString();
                        cobranzaD.U_VIS_Type = reader["U_VIS_Type"].ToString();
                    }
                }

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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetCollectionDetail - " + ex.Message + " - " + imei + " " + recibo);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return cobranzaD;
        }
        public List<CobranzaDBO> GetCollectionDocument(string imei, string docEntry)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DOCUMENT ('{1}','{2}')", DataSource.bd(), imei, docEntry);

            try
            {
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
                        cobranzaD = new CobranzaDBO();
                        cobranzaD.DocEntry = reader["DocEntry"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
                        cobranzaD.U_VIS_CollectionSalesperson = reader["U_VIS_CollectionSalesperson"].ToString();
                        cobranzaD.U_VIS_Type = reader["U_VIS_Type"].ToString();
                        listCobranzaBO.Add(cobranzaD);
                    }
                }

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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetCollectionDocument - " + ex.Message + " - " + imei + " " + docEntry);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return listCobranzaBO;
        }
        public ListaCobranzaD GetCollectionStatus(string imei, string status)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL =  string.Format("CALL {0}.APP_COLLECTIONS_STATUS ('{1}','{2}')", DataSource.bd(), imei, status);

            try
            {
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
                        cobranzaD = new CobranzaDBO();
                        cobranzaD.Code = reader["Code"].ToString();
                        cobranzaD.DocEntry = reader["DocEntry"].ToString();
                        cobranzaD.BankID = reader["BankID"].ToString();
                        cobranzaD.DepositID = reader["Deposit"].ToString();
                        cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                        cobranzaD.CardCode = reader["CardCode"].ToString();
                        cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                        cobranzaD.DocNum = reader["DocNum"].ToString();
                        cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                        cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                        cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                        cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                        cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                        cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                        cobranzaD.Status = reader["Status"].ToString();
                        cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                        cobranzaD.UserCode = reader["UserID"].ToString();
                        cobranzaD.SlpCode = reader["SlpCode"].ToString();
                        cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                        cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                        cobranzaD.Banking = reader["Banking"].ToString();
                        cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                        cobranzaD.QRStatus = reader["QRStatus"].ToString();
                        cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                        cobranzaD.POSPay = reader["POSPay"].ToString();
                        cobranzaD.U_VIS_CollectionSalesperson = reader["U_VIS_CollectionSalesperson"].ToString();
                        cobranzaD.U_VIS_Type = reader["U_VIS_Type"].ToString();
                        listCobranzaBO.Add(cobranzaD);
                    }
                }
                cobranzasD.CollectionDetail = listCobranzaBO;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetCollectionStatus - " + ex.Message+ " "+imei + " "+" "+status);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return cobranzasD;
        }
        public ListaCobranzaD GetCollectionStatus2(string imei, string status, string user)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_STATUS2 ('{1}','{2}','{3}')", DataSource.bd(), imei, status, user);

            try {
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
                    cobranzaD = new CobranzaDBO();
                    cobranzaD.Code = reader["Code"].ToString();
                    cobranzaD.DocEntry = reader["DocEntry"].ToString();
                    cobranzaD.BankID = reader["BankID"].ToString();
                    cobranzaD.DepositID = reader["Deposit"].ToString();
                    cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                    cobranzaD.CardCode = reader["CardCode"].ToString();
                    cobranzaD.DocEntryFT = reader["DocEntryFT"].ToString();
                    cobranzaD.DocNum = reader["DocNum"].ToString();
                    cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                    cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                    cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                    cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                    cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                    cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                    cobranzaD.Status = reader["Status"].ToString();
                    cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                    cobranzaD.UserCode = reader["UserID"].ToString();
                    cobranzaD.SlpCode = reader["SlpCode"].ToString();
                    cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                    cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                    cobranzaD.Banking = reader["Banking"].ToString();
                    cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                    cobranzaD.QRStatus = reader["QRStatus"].ToString();
                    cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                    cobranzaD.POSPay = reader["POSPay"].ToString();
                    cobranzaD.U_VIS_CollectionSalesperson = reader["U_VIS_CollectionSalesperson"].ToString();
                    cobranzaD.U_VIS_Type = reader["U_VIS_Type"].ToString();
                    listCobranzaBO.Add(cobranzaD);
                }
            }
            cobranzasD.CollectionDetail = listCobranzaBO;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetCollectionStatus2 - " + ex.Message + " " + imei + " " + " " + status + " " + user);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return cobranzasD;
        }
        public List<DepositsStatus2> GetCollectionDepositStatus(string imei, string status)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            DepositsStatus2 cobranzasStatus = new DepositsStatus2();
            List<DepositsStatus2>  listCobranzaBO = new List<DepositsStatus2>();
            string strSQL = string.Format("CALL {0}.APP_DEPOSITS_STATUS ('{1}','{2}')", DataSource.bd(), imei, status);

            try
            {
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
                        cobranzasStatus = new DepositsStatus2();
                        cobranzasStatus.Code = reader["Code"].ToString();
                        cobranzasStatus.U_VIS_BankID = reader["U_VIS_BankID"].ToString();
                        cobranzasStatus.BankName = reader["BankName"].ToString();
                        cobranzasStatus.U_VIS_IncomeType = reader["U_VIS_IncomeType"].ToString();
                        cobranzasStatus.U_VIS_Deposit = reader["U_VIS_Deposit"].ToString();
                        cobranzasStatus.U_VIS_Date = reader["U_VIS_Date"].ToString();
                        cobranzasStatus.U_VIS_DeferredDate = reader["U_VIS_DeferredDate"].ToString();
                        cobranzasStatus.U_VIS_Banking = reader["U_VIS_Banking"].ToString();
                        cobranzasStatus.U_VIS_UserID = reader["U_VIS_UserID"].ToString();
                        cobranzasStatus.U_VIS_SlpCode = reader["U_VIS_SlpCode"].ToString();
                        cobranzasStatus.U_VIS_AmountDeposit = reader["U_VIS_AmountDeposit"].ToString();
                        cobranzasStatus.U_VIS_Status = reader["U_VIS_Status"].ToString();
                        cobranzasStatus.U_VIS_Comments = reader["U_VIS_Comments"].ToString();
                        cobranzasStatus.U_VIS_CancelReason = reader["U_VIS_CancelReason"].ToString().ToUpper();
                        cobranzasStatus.U_VIS_DirectDeposit = reader["U_VIS_DirectDeposit"].ToString();
                        cobranzasStatus.U_VIS_POSPay = reader["U_VIS_POSPay"].ToString();
                        cobranzasStatus.U_VIS_CollectionSalesPerson = reader["U_VIS_CollectionSalesPerson"].ToString();
                        cobranzasStatus.U_VIS_BankValidation = reader["U_VIS_BankValidation"].ToString();
                        cobranzasStatus.U_VIS_ObservationStatus = reader["U_VIS_ObservationStatus"].ToString();
                        
                        listCobranzaBO.Add(cobranzasStatus);
                    }
                }
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "GetCollectionDepositStatus - " + ex.Message + " " + imei + " " + " " + status);
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

            return listCobranzaBO;
        }


        public ListaCobranzaD GetCollectionDeposit(string imei, string deposit)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaCobranzaD cobranzasD = new ListaCobranzaD();
            List<CobranzaDBO> listCobranzaBO = new List<CobranzaDBO>();
            CobranzaDBO cobranzaD = new CobranzaDBO();
            string strSQL = string.Format("CALL {0}.APP_COLLECTIONS_DEPOSIT ('{1}','{2}')", DataSource.bd(), imei, deposit);

            try {
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
                    cobranzaD = new CobranzaDBO();
                    cobranzaD.Code = reader["Code"].ToString();
                    cobranzaD.BankID = reader["BankID"].ToString();
                    cobranzaD.DepositID = reader["Deposit"].ToString();
                    cobranzaD.ItemDetail = reader["ItemDetail"].ToString();
                    cobranzaD.CardCode = reader["CardCode"].ToString();
                    cobranzaD.DocNum = reader["DocNum"].ToString();
                    cobranzaD.DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString());
                    cobranzaD.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                    cobranzaD.AmountCharged = Convert.ToDecimal(reader["AmountCharged"].ToString());
                    cobranzaD.NewBalance = Convert.ToDecimal(reader["NewBalance"].ToString());
                    cobranzaD.IncomeDate = reader["IncomeDate"].ToString();
                    cobranzaD.Receip = Convert.ToInt32(reader["Receip"].ToString());
                    cobranzaD.Status = reader["Status"].ToString();
                    cobranzaD.Commentary = reader["Comments"].ToString().ToUpper();
                    cobranzaD.UserCode = reader["UserID"].ToString();
                    cobranzaD.SlpCode = reader["SlpCode"].ToString();
                    cobranzaD.CardName = reader["CardName"].ToString().ToUpper();
                    cobranzaD.LegalNumber = reader["NumAtCard"].ToString();
                    cobranzaD.Banking = reader["Banking"].ToString();
                    cobranzaD.CancellationReason = reader["CancelReason"].ToString().ToUpper();
                    cobranzaD.QRStatus = reader["QRStatus"].ToString();
                    cobranzaD.DirectDeposit = reader["DirectDeposit"].ToString();
                    cobranzaD.POSPay = reader["POSPay"].ToString();
                    cobranzaD.U_VIS_CollectionSalesperson = reader["U_VIS_CollectionSalesperson"].ToString();
                    cobranzaD.U_VIS_Type = reader["U_VIS_Type"].ToString();
                    listCobranzaBO.Add(cobranzaD);
                }
            }
            cobranzasD.CollectionDetail = listCobranzaBO;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetCollectionDeposit - " + ex.Message + " " + imei + " " + " " + deposit);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return cobranzasD;
        }
        public ReceipResponse GetValidCollections(string IncomeDate, string CardCode, string DocEntry, string Receip, string SlpCode)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();
            ReceipResponse s = new ReceipResponse();
            string strSQL = string.Format("CALL {0}.APP_VALID_COLLECTION ('{1}','{2}','{3}','{4}',{5})", DataSource.bd(), IncomeDate, CardCode, DocEntry, Receip, SlpCode);

            try{

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
                    s = new ReceipResponse();
                    s.ErrorCode = "0";
                    s.Receip = reader["Receip"].ToString();
                    s.Message = "Cobranza " + reader["ItemDetail"].ToString() + " registrada satisfactoriamente";
                    s.ItemDetail = reader["ItemDetail"].ToString();
                    s.Code = reader["Code"].ToString();
                }
            }
            //cobranzasD.CollectionDetail = listaCobranzaD;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetValidCollections - " + ex.Message+" "+
                     IncomeDate + " " + CardCode + " " +  DocEntry + " " + Receip + " " + SlpCode);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
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
