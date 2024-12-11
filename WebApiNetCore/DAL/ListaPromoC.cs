
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

namespace SAP_Core.DAL
{
    public class ListaPromoCDAL : Connection,IDisposable
    {
        public ListarPromoC GetListaPromoC(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListarPromoC listarPromoC = new ListarPromoC();
            List<ListaPromoC> listaPromoCs = new List<ListaPromoC>();
            ListaPromoC listaPromoC = new ListaPromoC();
            string strSQL = string.Format("CALL {0}.APP_PROMOTION_HEADER('{1}')", DataSource.bd(), imei);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listaPromoC = new ListaPromoC();
                        listaPromoC.PromotionListID = reader["Lista_Promocion_ID"].ToString().ToUpper();
                        listaPromoC.PromotionID = reader["Promocion_ID"].ToString().ToUpper();
                        listaPromoC.ItemCode = reader["Producto_ID"].ToString().ToUpper();
                        listaPromoC.ItemName = reader["Producto"].ToString().ToUpper();
                        listaPromoC.Uom = reader["UMD"].ToString().ToUpper();
                        listaPromoC.Quantity = Convert.ToDecimal(reader["Cantidad"].ToString());
                        listaPromoC.DiscountPrcent = Convert.ToDecimal(reader["Porcentaje_Descuento"].ToString());
                        listaPromoC.Cantidad_Maxima = Convert.ToDecimal(reader["Cantidad_Maxima"].ToString());
                        listaPromoC.Tipo_Malla = reader["Tipo_Malla"].ToString().ToUpper();
                        listaPromoC.Combo = reader["Combo"].ToString().ToUpper();
                        listaPromoCs.Add(listaPromoC);
                    }
                }
                listarPromoC.PromotionListHeader = listaPromoCs;
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

            return listarPromoC;
        }

        public ListarPromoC GetListaPromoCWhsCode(string imei,string WhsCode)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListarPromoC listarPromoC = new ListarPromoC();
            List<ListaPromoC> listaPromoCs = new List<ListaPromoC>();
            ListaPromoC listaPromoC = new ListaPromoC();
            string strSQL = string.Format("CALL {0}.APP_PROMOTION_HEADER_WAREHOUSE ('{1}','{2}')", DataSource.bd(), imei, WhsCode);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listaPromoC = new ListaPromoC();
                        listaPromoC.PromotionListID = reader["Lista_Promocion_ID"].ToString().ToUpper();
                        listaPromoC.PromotionID = reader["Promocion_ID"].ToString().ToUpper();
                        listaPromoC.ItemCode = reader["Producto_ID"].ToString().ToUpper();
                        listaPromoC.ItemName = reader["Producto"].ToString().ToUpper();
                        listaPromoC.Uom = reader["UMD"].ToString().ToUpper();
                        listaPromoC.Quantity = Convert.ToDecimal(reader["Cantidad"].ToString());
                        listaPromoC.DiscountPrcent = Convert.ToDecimal(reader["Porcentaje_Descuento"].ToString());
                        listaPromoC.Cantidad_Maxima = Convert.ToDecimal(reader["Cantidad_Maxima"].ToString());
                        listaPromoC.Tipo_Malla = reader["Tipo_Malla"].ToString().ToUpper();
                        listaPromoC.Combo = reader["Combo"].ToString().ToUpper();
                        listaPromoCs.Add(listaPromoC);
                    }
                }
                listarPromoC.PromotionListHeader = listaPromoCs;
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return listarPromoC;
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
        ~ListaPromoCDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
