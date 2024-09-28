
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Sap.Data.Hana;
using System.Data;
using SAP_Core.Utils;
using SalesForce.Util;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.BO;
using static WebApiNetCore.Utils.Other;
using System.Net;
using WebApiNetCore.BO;

namespace SAP_Core.DAL
{
    public class SalesOrderDAL : Connection, IDisposable
    {
        private ServiceLayer serviceLayer;
        public SalesOrderDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }
        public SalesOrdersBO getSalesOrders(JObject json)
        {
            dynamic objs = JsonConvert.DeserializeObject(json.ToString());
            dynamic c = objs["SalesOrders"];
            SalesOrdersBO orders = new SalesOrdersBO();
            List<SalesOrderBO> listOrders = new List<SalesOrderBO>();
            
            foreach (dynamic obj in c)
            {
                SalesOrderBO order = new SalesOrderBO();
                order.CardCode = obj["CardCode"].ToString();
                order.Comments = obj["Comments"].ToString();
                order.DocCurrency = obj["DocCurrency"].ToString();
                order.DocObjectCode = obj["quotation"].ToString() == "N" ? "oOrders" : "oQuotations";
                order.DocDate = obj["DocDate"].ToString();
                order.DocDueDate = obj["DocDueDate"].ToString();
                //order.DocRate = obj["DocRate"].ToString();
                order.DocType = obj["DocType"].ToString();
                order.U_VIS_SalesOrderID = obj["U_VIS_SalesOrderID"].ToString();
                order.DocumentsOwner = obj["DocumentsOwner"].ToString();
                order.FederalTaxID = obj["FederalTaxID"].ToString();
                order.PayToCode = obj["PayToCode"].ToString();
                order.PaymentGroupCode = obj["PaymentGroupCode"].ToString();
                order.SalesPersonCode = obj["SalesPersonCode"].ToString();
                order.ShipToCode = obj["ShipToCode"].ToString();
                order.U_VIST_SUCUSU = obj["Branch"].ToString();
                order.U_VIS_AppVersion = obj["AppVersion"].ToString();
                order.quotation = obj["quotation"].ToString();
                order.DiscountPercent = "0";
                order.Draft = obj["Draft"] == null ? "N" : obj["Draft"].ToString();
                order.DocumentLines = getDocumentLines(obj["DocumentLines"]);

                listOrders.Add(order);
            }
            orders.SalesOrders = listOrders;
            return orders;
        }
        public SalesOrdersPE getSalesOrdersPE(JObject json)
        {
            dynamic objs = JsonConvert.DeserializeObject(json.ToString());
            dynamic c = objs["SalesOrders"];
            SalesOrdersPE orders = new SalesOrdersPE();
            List<SalesOrderPE> listOrders = new List<SalesOrderPE>();

            foreach (dynamic obj in c)
            {
                SalesOrderPE order = new SalesOrderPE();
                order.CardCode = obj["CardCode"].ToString();
                order.Comments = obj["Comments"].ToString();
                order.DocCurrency = obj["DocCurrency"].ToString();
                order.DocDate = obj["DocDate"].ToString();
                order.DocDueDate = obj["DocDueDate"].ToString();
                order.DocRate = obj["DocRate"] == null ? "1" : obj["DocRate"].ToString();
                order.DocType = obj["DocType"].ToString();
                order.DocObjectCode = obj["quotation"].ToString() == "N" ? "oOrders" : "oQuotations";
                order.U_VIS_SalesOrderID = obj["U_VIS_SalesOrderID"].ToString();
                order.DocumentsOwner = obj["DocumentsOwner"].ToString();
                order.FederalTaxID = obj["FederalTaxID"].ToString();
                order.PayToCode = obj["PayToCode"].ToString();
                order.PaymentGroupCode = obj["PaymentGroupCode"].ToString();
                order.SalesPersonCode = obj["SalesPersonCode"].ToString();
                order.ShipToCode = obj["ShipToCode"].ToString();
                order.U_VIST_SUCUSU = obj["Branch"].ToString();
                order.U_VIS_AppVersion = obj["AppVersion"].ToString();
                order.TaxDate = obj["TaxDate"].ToString();
                order.U_VIS_AgencyCode = obj["U_VIS_AgencyCode"].ToString();
                order.U_VIS_AgencyDir = obj["U_VIS_AgencyDir"].ToString();
                order.U_VIS_AgencyName = obj["U_VIS_AgencyName"].ToString();
                order.U_VIS_AgencyRUC = obj["U_VIS_AgencyRUC"].ToString();
                order.U_SYP_MDMT = obj["U_SYP_MDMT"].ToString();
                order.U_SYP_TVENTA = obj["U_SYP_TVENTA"].ToString();
                order.U_SYP_VIST_TG = obj["U_SYP_VIST_TG"].ToString();
                order.U_SYP_DOCEXPORT = obj["U_SYP_DOCEXPORT"].ToString();
                order.U_SYP_FEEST = obj["U_SYP_FEEST"].ToString();
                order.U_SYP_FEMEX = obj["U_SYP_FEMEX"].ToString();
                order.U_SYP_FETO = obj["U_SYP_FETO"].ToString();
                order.DiscountPercent = "0";
                if (obj["quotation"].ToString() == "N")
                {
                    /*if (obj["ApPrcnt"].ToString() == "Y" || obj["ApCredit"].ToString() == "Y" || obj["ApDues"].ToString() == "Y" || obj["ApTPag"].ToString() == "Y")
                    {
                        order.Document_ApprovalRequests = getApproval(obj["ApPrcnt"], obj["ApCredit"], obj["ApDues"], obj["ApTPag"]);
                        order.DiscountPercent = "0.2";

                    }*/
                }
                order.quotation = obj["quotation"].ToString();
                order.Draft = obj["Draft"] == null ? "N" : obj["Draft"].ToString();
                order.DocumentLines = getDocumentLinesPE(obj["DocumentLines"]);

                listOrders.Add(order);
            }
            orders.SalesOrders = listOrders;
            return orders;
        }
        public List<DocumentLineBO> getDocumentLines(dynamic json)
        {
            dynamic objs = JsonConvert.DeserializeObject(json.ToString());
            
            List<DocumentLineBO> listLines = new List<DocumentLineBO>();
            foreach (dynamic obj in json)
            {
                DocumentLineBO lines = new DocumentLineBO();
                lines.CostingCode = obj["CostingCode"].ToString();
                lines.CostingCode2 = obj["CostingCode2"].ToString();
                lines.CostingCode3 = obj["CostingCode3"] == null ? "" : obj["CostingCode3"].ToString();
                lines.DiscountPercent = obj["DiscountPercent"].ToString();
                lines.Dscription = obj["Dscription"].ToString();
                lines.ItemCode = obj["ItemCode"].ToString();
                lines.UnitPrice = obj["Price"].ToString();
                lines.Quantity = obj["Quantity"].ToString();
                lines.TaxCode = obj["TaxCode"].ToString();
                lines.TaxOnly = obj["TaxOnly"].ToString();
                lines.WarehouseCode = obj["WarehouseCode"].ToString();
                listLines.Add(lines);
            }
            return listLines;
        }
        public List<DocumentLinePE> getDocumentLinesPE(dynamic json)
        {
            dynamic objs = JsonConvert.DeserializeObject(json.ToString());

            List<DocumentLinePE> listLines = new List<DocumentLinePE>();
            foreach (dynamic obj in json)
            {
                DocumentLinePE lines = new DocumentLinePE();
                lines.CostingCode = obj["CostingCode"].ToString();
                lines.CostingCode2 = obj["CostingCode2"].ToString();
                lines.CostingCode3 = obj["CostingCode3"] == null ? "" : obj["CostingCode3"].ToString();
                lines.DiscountPercent = obj["DiscountPercent"].ToString();
                lines.Dscription = obj["Dscription"].ToString();
                lines.ItemCode = obj["ItemCode"].ToString();
                lines.Price = obj["Price"].ToString();
                lines.Quantity = obj["Quantity"].ToString();
                lines.TaxCode = obj["TaxCode"].ToString();
                lines.TaxOnly = obj["TaxOnly"].ToString();
                lines.WarehouseCode = obj["WarehouseCode"].ToString();
                lines.AcctCode = obj["AcctCode"].ToString();
                lines.COGSAccountCode = obj["COGSAccountCode"].ToString();
                lines.U_SYP_FECAT07 = obj["U_SYP_FECAT07"].ToString();
                lines.U_VIST_CTAINGDCTO = obj["U_VIST_CTAINGDCTO"].ToString();
                lines.U_VIS_CommentText = obj["U_VIS_CommentText"].ToString();
                lines.U_VIS_PromID = obj["U_VIS_PromID"].ToString();
                lines.U_VIS_PromLineID = obj["U_VIS_PromLineID"].ToString();
                listLines.Add(lines);
            }
            return listLines;
        }
        public List<Approval> getApproval(dynamic prcnt, dynamic credit, dynamic dues, dynamic tpag)
        {
            List<Approval> listLines = new List<Approval>();
            Approval lines = new Approval();
            string message = string.Empty;
            lines.ApprovalTemplatesID = "4";
            
            if (prcnt == "Y")
            {
                message += "Excede el porcentaje de descuento. \n";
            }
            if (credit == "Y")
            {
                message += "Excede el límite de crédito. \n";
            }
            if (dues == "Y")
            {
                message += "Cuenta con documentos vencidos. \n";
            }
            if (tpag == "Y")
            {
                message += "Término de Pago distinto al cliente.";
            }
            lines.Remarks = message;
            listLines.Add(lines);


            return listLines;
        }
        public SalesOrder validSalesOrder(string CardCode, string DocDate, string SalesOrderID, string slpCode)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();

            SalesOrder s = new SalesOrder();

            string strSQL = string.Format("CALL {0}.APP_VALID_SALESORDER ('{1}','{2}','{3}','{4}')", DataSource.bd(), CardCode, DocDate, SalesOrderID, slpCode);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);
            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    s = new SalesOrder();
                    s.SalesOrderID = reader["U_VIS_SalesOrderID"].ToString();
                    s.DocEntry = reader["DocEntry"].ToString();
                    s.DocNum = reader["DocNum"].ToString();
                    s.ErrorCode = "0";
                    s.Message = "Documento " + reader["DocNum"].ToString() + " creado satisfactoriamente";
                }
            }
            //cobranzasD.CollectionDetail = listaCobranzaD;
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

            return s;
        }
        public async Task<ResponseData> insert(string endPoint, string jsonPayload)
        {
            ResponseData response = await serviceLayer.Request("/b1s/v1/" + endPoint, Method.POST, jsonPayload);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                dynamic responseBody = await response.Data.Content.ReadAsStringAsync();

                responseBody = JsonConvert.DeserializeObject(responseBody.ToString());

                response.Data = responseBody;
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                var responseBody = await response.Data.Content.ReadAsStringAsync();
                ResponseError error = System.Text.Json.JsonSerializer.Deserialize<ResponseError>(responseBody);


                response.StatusCode = HttpStatusCode.FailedDependency;
                response.Data = error;
            }

            return response;
        }
        public ListObjects getObjects(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<ObjectBO> listAgencias = new List<ObjectBO>();
            ListObjects agencias = new ListObjects();
            ObjectBO agencia = new ObjectBO();
            string strSQL = string.Format("CALL {0}.APP_OBJECT ('{1}')", DataSource.bd(), imei);
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
                        agencia = new ObjectBO();
                        agencia.Code = reader["Code"].ToString().ToUpper();
                        agencia.Name = reader["Name"].ToString().ToUpper();
                        listAgencias.Add(agencia);
                    }
                }
                agencias.Objects = listAgencias;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "SalesOrder-getObjects - " + ex.Message);
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
            return agencias;
        }
        public ListBusinessLayer getBusinessLayer(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<BusinessLayerBO> listAgencias = new List<BusinessLayerBO>();
            ListBusinessLayer agencias = new ListBusinessLayer();
            BusinessLayerBO agencia = new BusinessLayerBO();
            string strSQL = string.Format("CALL {0}.APP_BUSSINESS_LAYER ('{1}')", DataSource.bd(), imei);
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
                        agencia = new BusinessLayerBO();
                        agencia.Code = reader["Code"].ToString();
                        agencia.Name = reader["Name"].ToString();
                        agencia.U_VIS_Objetive = reader["U_VIS_Objetive"].ToString();
                        agencia.U_VIS_VariableType = reader["U_VIS_VariableType"].ToString();
                        agencia.U_VIS_Variable = reader["U_VIS_Variable"].ToString();
                        agencia.U_VIS_Trigger = reader["U_VIS_Trigger"].ToString();
                        agencia.U_VIS_TriggerType = reader["U_VIS_TriggerType"].ToString();
                        agencia.U_VIS_Active = reader["U_VIS_Active"].ToString();
                        agencia.U_VIS_ValidFrom = reader["U_VIS_ValidFrom"].ToString();
                        agencia.U_VIS_ValidUntil = reader["U_VIS_ValidUntil"].ToString();
                        agencia.Detail = reader["Detail"] == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<BusinessLayerDetailBO>>(reader["Detail"].ToString());
                        listAgencias.Add(agencia);
                    }
                }
                agencias.BusinessLayer = listAgencias;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "SalesOrder-getObjects - " + ex.Message);
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
            return agencias;
        }
        public LisstBusinessLayerSalesDetail getBusinessLayerDetail(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<BusinessLayerSalesDetailBO> listAgencias = new List<BusinessLayerSalesDetailBO>();
            LisstBusinessLayerSalesDetail agencias = new LisstBusinessLayerSalesDetail();
            BusinessLayerSalesDetailBO agencia = new BusinessLayerSalesDetailBO();
            string strSQL = string.Format("CALL {0}.APP_BUSSINESS_LAYER_SALES_DETAIL ('{1}')", DataSource.bd(), imei);
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
                        agencia = new BusinessLayerSalesDetailBO();
                        agencia.Code = reader["Code"].ToString();
                        agencia.Name = reader["Name"].ToString();
                        agencia.Object = reader["Object"].ToString();
                        agencia.Action = reader["Action"].ToString();
                        agencia.Detail = reader["Detail"] == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<BusinessLayerSalesDetailDetailBO>>(reader["Detail"].ToString());
                        listAgencias.Add(agencia);
                    }
                }
                agencias.BusinessLayerDetail = listAgencias;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "SalesOrder-getObjects - " + ex.Message);
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
        ~SalesOrderDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
