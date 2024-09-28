
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sap.Data.Hana;
using SAP_Core.BO;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class ListaPreciosDAL : Connection,IDisposable
    {

        public ListaPrecios GetListaPrecios(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaPrecios listarPrecios = new ListaPrecios();
            List<ListaPreciosBO> listaPrecios = new List<ListaPreciosBO>();
            ListaPreciosBO listaPrecio = new ListaPreciosBO();
            string strSQL = string.Format("CALL {0}.APP_PRICE_LIST ('{1}')", Utils.bd(), imei);

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
                        listaPrecios.Add(listaPrecio);
                    }
                }
                listarPrecios.PriceList = listaPrecios;
                connection.Close();
            }
            catch (Exception e)
            {
                string m = e.Message;
                throw;
            }
            finally
            {
                if (connection.State.Equals("Open"))
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
