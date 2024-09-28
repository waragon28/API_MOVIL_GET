
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
using Sentry;

namespace SAP_Core.DAL
{
    public class RutaTrabajoDAL : Connection, IDisposable
    {

        public ListaRutaTrabajo GetRutaTrabajo(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaRutaTrabajo rutas = new();
            List<RutaTrabajoBO> listRutaTrabajo = new();
           
            string strSQL = string.Format("CALL {0}.APP_WORK_PATH ('{1}')", DataSource.bd(), imei);

            try
            {
                connection.Open();
                HanaCommand command = new(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    RutaTrabajoBO rutaTrabajo = new();

                    while (reader.Read())
                    {
                        rutaTrabajo = new RutaTrabajoBO();
                        rutaTrabajo.Territory = reader["Zona"].ToString().ToUpper();
                        rutaTrabajo.TerritoryId = reader["Territorio_ID"].ToString().ToUpper();
                        rutaTrabajo.Day = reader["Dia"].ToString().ToUpper();
                        rutaTrabajo.Frequency = reader["Frecuencia"].ToString();
                        rutaTrabajo.VisitDate = reader["FechaVisita"].ToString();
                        rutaTrabajo.Status = reader["Estado"].ToString().ToUpper();
                        rutaTrabajo.SlpCode = reader["FuerzaTrabajo_ID"].ToString();
                        rutaTrabajo.InitDate = reader["FechaInicioRuta"].ToString();

                        listRutaTrabajo.Add(rutaTrabajo);
                    }
                }
                rutas.WorkPath = listRutaTrabajo;
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
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return rutas;

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
        ~RutaTrabajoDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
