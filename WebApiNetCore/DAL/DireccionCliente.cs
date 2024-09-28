
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
    public class DireccionClienteDAL : Connection,IDisposable
    {
        public List<DireccionClienteBO> GetDireccion(string imei, string cliente)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<DireccionClienteBO> listDireccion = new List<DireccionClienteBO>();
            DireccionClienteBO direccion = new DireccionClienteBO();
            string strSQL = string.Format("CALL {0}.APP_ADDRESSES ('{1}','{2}')", DataSource.bd(), imei, cliente);
           
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        direccion = new DireccionClienteBO();
                        //direccion.CardCode = reader["Cliente_ID"].ToString();
                        direccion.ShipToCode = reader["DomEmbarque_ID"].ToString();
                        direccion.Street = reader["Direccion"].ToString().ToUpper();
                        direccion.TerritoryID = reader["Zona_ID"].ToString().ToUpper();
                        direccion.Territory = reader["Zona"].ToString().ToUpper();
                        direccion.SlpCode = reader["FuerzaTrabajo_ID"].ToString();
                        direccion.SlpName = reader["NombreFuerzaTrabajo"].ToString().ToUpper();
                        listDireccion.Add(direccion);
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetDireccion - " + ex.Message," "+
                    imei+" "+cliente);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();

                string message = ex.Message;
                throw;
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return listDireccion;
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
        ~DireccionClienteDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
