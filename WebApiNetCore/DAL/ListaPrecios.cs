
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sap.Data.Hana;
using SAP_Core.BO;
using System.Configuration;
using System.Data;
using SAP_Core.Utils;
using Sentry;

namespace SAP_Core.DAL
{
    public class ListaPreciosDAL : Connection,IDisposable
    {
        public ListaPreciosWhs GetPriceListWarehouse(string imei,string whsCode, string pl1, string pl2)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaPreciosBO whsBO = new ListaPreciosBO();
            List<ListaPreciosBO> lwhsBO = new List<ListaPreciosBO>();
            ListaPreciosWhs lwhs = new ListaPreciosWhs();
            
            string strSQL = string.Format("CALL {0}.APP_PRICE_LIST_WAREHOUSE('{1}','{2}','{3}','{4}')", DataSource.bd(), imei, whsCode, pl1, pl2);
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        whsBO = new ListaPreciosBO();

                        whsBO.ItemCode = reader["ItemCode"].ToString();
                        whsBO.ItemName = reader["ItemName"].ToString().ToUpper();
                        whsBO.WhsStock = Convert.ToDecimal(reader["Disponible"].ToString());
                        whsBO.StockTotal = Convert.ToDecimal(reader["StockTotal"].ToString());
                        whsBO.Uom = reader["UDM"].ToString().ToUpper();
                        whsBO.Cash = Convert.ToDecimal(reader["Contado"].ToString());
                        whsBO.Credit = Convert.ToDecimal(reader["Credito"].ToString());
                        whsBO.Gallons = Convert.ToDecimal(reader["GAL"].ToString());
                        whsBO.Type = reader["Tipo"].ToString();
                        whsBO.DiscPrcnt = Math.Round(Convert.ToDecimal(reader["DiscPrcnt"].ToString()), 0);
                        whsBO.CashDscnt = reader["CashDscnt"].ToString();


                        whsBO.Units = Convert.ToInt32(reader["Units"]);

                        whsBO.OilTax = reader["OilTax"].ToString();
                        whsBO.Liter = Convert.ToDecimal(reader["Liter"].ToString());
                        whsBO.SIGAUS = reader["SIGAUS"].ToString();

                        whsBO.MonedaAdicional = reader["MonedaAdicional"].ToString();
                        whsBO.MonedaAdicionalContado = Convert.ToDecimal(reader["MonedaAdicionalContado"]);
                        whsBO.MonedaAdicionalCredito = Convert.ToDecimal(reader["MonedaAdicionalCredito"]);
                        whsBO.CodePriceListCredit = Convert.ToInt32(reader["CodePriceListCredit"]);
                        whsBO.CodePriceListCash = Convert.ToInt32(reader["CodePriceListCash"]);
                        whsBO.Inventariable = reader["Inventariable"].ToString();
                        lwhsBO.Add(whsBO);
                    }
                }
                lwhs.PriceList = lwhsBO;

                connection.Close();
            }
            catch (Exception e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return lwhs;
        }
        public LPListWarehouse GetListWarehouse(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            LPWarehouseBO whsBO = new LPWarehouseBO();
            List<LPWarehouseBO> lwhsBO = new List<LPWarehouseBO>();
            LPListWarehouse lwhs = new LPListWarehouse();
            string strSQL = string.Format("CALL {0}.APP_WAREHOUSE('{1}')", DataSource.bd(), imei);
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        whsBO = new LPWarehouseBO();
                        whsBO.WhsCode = reader["WhsCode"].ToString();
                        whsBO.WhsName = reader["WhsName"].ToString();
                        whsBO.PriceListCash = Convert.ToInt32(reader["PriceListCash"]);
                        whsBO.PriceListCredit = Convert.ToInt32(reader["PriceListCredit"]);
                        whsBO.U_VIST_SUCUSU = reader["U_VIST_SUCUSU"].ToString();
                        lwhsBO.Add(whsBO);
                    }
                }
                lwhs.WarehouseList =lwhsBO;

                connection.Close();
            }
            catch (Exception e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return lwhs;
        }
        public ListaPreciosHeader GetListaPreciosHeader(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaPreciosHeaderBO listarPrecios = new ListaPreciosHeaderBO();
            List<ListaPreciosHeaderBO> listaPrecios = new List<ListaPreciosHeaderBO>();
            ListaPreciosHeader listaPrecio = new ListaPreciosHeader();
            string strSQL = string.Format("CALL {0}.APP_PRICE_LIST_HEAD ('{1}')", DataSource.bd(), imei);
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listarPrecios = new ListaPreciosHeaderBO();
                        listarPrecios.ListNum = Convert.ToInt32(reader["ListNum"].ToString());
                        listarPrecios.ListName = reader["ListName"].ToString();
                        listarPrecios.PrcntIncrease = Convert.ToDecimal(reader["U_VIS_PercentageIncrease"].ToString());
                        listaPrecios.Add(listarPrecios);
                    }
                }
                listaPrecio.PriceListHeader = listaPrecios;

                connection.Close();
            }
            catch (Exception e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return listaPrecio;
        }

        public ListaPrecios GetListaPrecios(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaPrecios listarPrecios = new ListaPrecios();
            List<ListaPreciosBO> listaPrecios = new List<ListaPreciosBO>();
            ListaPreciosBO listaPrecio = new ListaPreciosBO();
            string strSQL = string.Format("CALL {0}.APP_PRICE_LIST ('{1}')", DataSource.bd(), imei);
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listaPrecio = new ListaPreciosBO();
                        listaPrecio.ItemCode = reader["ItemCode"].ToString();
                        listaPrecio.ItemName = reader["ItemName"].ToString().ToUpper();
                        listaPrecio.WhsStock = Convert.ToDecimal(reader["Disponible"].ToString());
                        listaPrecio.StockTotal = Convert.ToDecimal(reader["StockTotal"].ToString());
                        listaPrecio.Uom = reader["UDM"].ToString().ToUpper();
                        listaPrecio.Cash = Convert.ToDecimal(reader["Contado"].ToString());
                        listaPrecio.Credit = Convert.ToDecimal(reader["Credito"].ToString());
                        listaPrecio.Gallons = Convert.ToDecimal(reader["GAL"].ToString());
                        listaPrecio.Type = reader["Tipo"].ToString();
                        listaPrecio.DiscPrcnt = Math.Round(Convert.ToDecimal(reader["DiscPrcnt"].ToString()),0);
                        listaPrecio.CashDscnt = reader["CashDscnt"].ToString();
                        listaPrecio.Units = Convert.ToInt32(reader["Units"]);
                        listaPrecio.OilTax = reader["OilTax"].ToString();
                        listaPrecio.Liter = Convert.ToDecimal(reader["Liter"].ToString());
                        listaPrecio.SIGAUS = reader["SIGAUS"].ToString();
                        listaPrecio.MonedaAdicional = reader["MonedaAdicional"].ToString();
                        listaPrecio.MonedaAdicionalContado = Convert.ToDecimal(reader["MonedaAdicionalContado"]);
                        listaPrecio.MonedaAdicionalCredito = Convert.ToDecimal(reader["MonedaAdicionalCredito"]);
                        listaPrecio.CodePriceListCredit = Convert.ToInt32(reader["CodePriceListCredit"]);
                        listaPrecio.CodePriceListCash = Convert.ToInt32(reader["CodePriceListCash"]);
                        listaPrecio.Inventariable = reader["Inventariable"].ToString();
                        listaPrecios.Add(listaPrecio);
                    }
                }
                listarPrecios.PriceList = listaPrecios;
                connection.Close();
            }
            catch (Exception e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                Console.WriteLine(e.Message.ToString());
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return listarPrecios;
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
        ~ListaPreciosDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
