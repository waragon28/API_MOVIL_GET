
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP_Core.BO;
using System.Threading.Tasks;
using Sap.Data.Hana;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class OrdenVentaCDAL : Connection,IDisposable
    {
        public ListaOrdenVentaC GetOrdenVentaC(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaOrdenVentaC ordenes = new ListaOrdenVentaC();
            List<OrdenVentaC> listOrdenVentaC = new List<OrdenVentaC>();
            OrdenVentaC ordenVentaC = new OrdenVentaC();
            string strSQL = string.Format("CALL {0}.APP_SALES_ORDERS ('{1}','{2}')", Utils.bd(), imei, fecha);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ordenVentaC = new OrdenVentaC();
                    ordenVentaC.SalesOrderID = reader["OrdenVenta_ID"].ToString();
                    ordenVentaC.Object = reader["Object"].ToString();
                    ordenVentaC.DocNum = reader["DocNum"].ToString();
                    ordenVentaC.CardCode = reader["Cliente_ID"].ToString();
                    ordenVentaC.CardName = reader["NombreCliente"].ToString();
                    ordenVentaC.LicTradNum = reader["RucDni"].ToString();
                    ordenVentaC.SlpCode = reader["U_VIS_SlpCode"].ToString();
                    ordenVentaC.ApprovalStatus = reader["EstadoAprobacion"].ToString();
                    ordenVentaC.ApprovalCommentary = "";
                    ordenVentaC.DocTotal = Convert.ToDecimal(reader["MontoTotalOrden"].ToString());
                    listOrdenVentaC.Add(ordenVentaC);
                }
            }
            ordenes.SalesOrder = listOrdenVentaC;
            connection.Close();

            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }
            return ordenes;
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
        ~OrdenVentaCDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion

    }
}
