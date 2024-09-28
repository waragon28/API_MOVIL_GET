
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
using System.Threading.Tasks;
using static WebApiNetCore.Utils.Other;

namespace SAP_Core.DAL
{
    public class KardexDAL : Connection
    {
        private ServiceLayer serviceLayer;
        public KardexDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }

        public ListaKardex getKardex(string CardCode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaKardex kardex = new ListaKardex();
            List<KardexBO> listaKardex = new List<KardexBO>();
            KardexBO kardx = new KardexBO();
            string strSQL = string.Format("CALL {0}.P_VIS_CRE_KARDEX_CLIENTE ('{1}')", DataSource.bd(), CardCode);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        kardx = new KardexBO();
                        kardx.CardCode = reader["CardCode"].ToString();
                        kardx.DocCur = reader["DocCur"].ToString();
                        kardx.NumAtCard = reader["U_SYP_MDSD"].ToString()+"-"+reader["U_SYP_MDCD"].ToString();
                        kardx.TaxDate = reader["TaxDate"].ToString();
                        kardx.DocDueDate = reader["DocDueDate"].ToString();
                        kardx.DocTotal = Convert.ToDecimal(reader["DocTotal"]);
                        kardx.DocEntry = reader["DocEntry"].ToString();
                        kardx.Balance = Convert.ToDecimal(reader["SALDO"]);
                        kardx.CardName = reader["CardName"].ToString();
                        kardx.LicTradNum = reader["LicTradNum"].ToString();
                        kardx.Mobile = reader["Phone1"]==null?"": reader["Phone1"].ToString();
                        kardx.SlpCode = reader["U_VIS_SlpCode"].ToString();
                        kardx.Street = reader["Street"].ToString();
                        kardx.PymntGroup = reader["PymntGroup"].ToString();
                        kardx.Block = reader["U_SYP_DEPA"].ToString();
                        kardx.County = reader["U_SYP_PROV"].ToString();
                        kardx.City = reader["U_SYP_DIST"].ToString();
                        kardx.AmountCharged = Convert.ToDecimal(reader["Importe Cobrado"]);
                        kardx.IncomeDate = reader["FECHA DE PAGO"].ToString();
                        kardx.OperationNumber = reader["NRO. OPERA"].ToString();
                        kardx.DocNum = reader["DocNum"].ToString();
                        kardx.JrnlMemo = reader["JrnlMemo"].ToString();
                        kardx.Comments = reader["Comments"].ToString();
                        kardx.Bank = reader["Banco"].ToString();
                        kardx.SalesInvoice = reader["VendedorFactura"] == null?"":reader["VendedorFactura"].ToString();
                        kardx.CollectorInvoice = reader["CobradorFactura"] == null ? "" : reader["CobradorFactura"].ToString();  

                        listaKardex.Add(kardx);
                    }
                }
                kardex.Kardex = listaKardex;
                connection.Close();
            }
            catch (Exception ex)
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
            return kardex;

        }

        /*public async Task<ResponseData> getValidationCliente(string imei, string dia)
        {
          
            ResponseData response = await serviceLayer.Request( "/b1s/v1/VIS_DIS_ODRT(" + dispatch.DocEntry + ")", Method.GET);
           
            if (response.StatusCode == HttpStatusCode.OK)
            {
               
             
            }
            else
            {
              
            }
                  

            return new ResponseData()
            {
                Data = new DispatchResponseList() { Dispatch = dispatchResponseList },
                StatusCode = HttpStatusCode.OK
            };
        }*/


    }
}
