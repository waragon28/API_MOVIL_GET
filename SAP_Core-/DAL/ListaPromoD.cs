
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class ListaPromoDDAL : Connection,IDisposable
    {

        public ListarPromoD GetListaPromoD(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListarPromoD listarPromoD = new ListarPromoD();
            List<ListaPromoD> listaPromoDs = new List<ListaPromoD>();
            ListaPromoD listaPromoD = new ListaPromoD();
            string strSQL = string.Format("CALL {0}.APP_PROMOTION_DETAIL ('{1}')", Utils.bd(), imei);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listaPromoD = new ListaPromoD();
                        listaPromoD.PromotionListID = reader["CodigoListaPromo"].ToString().ToUpper();
                        listaPromoD.PromotionID = reader["CodigoPromocion"].ToString().ToUpper();
                        listaPromoD.PromotionDetail = reader["DetallePromocion"].ToString().ToUpper();
                        listaPromoD.ItemCode = reader["CodigoProducto"].ToString();
                        listaPromoD.ItemName = reader["Producto"].ToString().ToUpper();
                        listaPromoD.Uom = reader["UDM"].ToString();
                        listaPromoD.Quantity = Convert.ToDecimal(reader["QtyProductoRegalo"].ToString());
                        listaPromoD.DiscountPrcent = Convert.ToDecimal(reader["Descuento"].ToString());
                        listaPromoDs.Add(listaPromoD);
                    }
                }
                listarPromoD.PromotionListDetail = listaPromoDs;
                connection.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }

            return listarPromoD;
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
        ~ListaPromoDDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion

    }
}
