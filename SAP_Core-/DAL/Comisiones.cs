
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;

namespace SAP_Core.DAL
{
    public class ComisionesDAL : Connection,IDisposable
    {
        public ListaComisiones GetComision(string imei, string anio, string mes)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaComisiones comisiones = new ListaComisiones();
            List<ComisionesBO> listComisiones = new List<ComisionesBO>();
            ComisionesBO comision = null;
            string strSQL = string.Format("CALL {0}.APP_COMMISSIONS ('{1}','{2}','{3}')", Utils.bd(), imei, anio, mes);
            
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comision = new ComisionesBO();
                        comision.Variable = reader["Variable"].ToString();
                        comision.Uom = reader["UDM"].ToString();
                        comision.Advance = Convert.ToDecimal(reader["Avance"].ToString());
                        comision.Quota = Convert.ToDecimal(reader["Cuota"].ToString());
                        comision.Percentage = Convert.ToDecimal(reader["Porcentaje"].ToString());
                        comision.CodeColor = reader["CodeColor"].ToString();
                        comision.HideData = reader["HideData"].ToString();
                        listComisiones.Add(comision);
                    }
                }
                comisiones.Commissions = listComisiones;
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
            return comisiones;
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
        ~ComisionesDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
