
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using SAP_Core.Utils;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class VisitDAL : Connection,IDisposable
    {

        public List<VisitBOs> GetVisit(string imei, string cardcode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<VisitBOs> listVisit = new List<VisitBOs>();
            VisitBOs visit;
            string strSQL= string.Format("CALL {0}.APP_VISITS ('{1}','{2}')", Utils.bd(), imei, cardcode);

            
            try
            {
                connection.OpenAsync();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        visit = new VisitBOs();
                        visit.Code = reader["Code"].ToString();
                        visit.Date = reader["Date"].ToString();
                        visit.Type = reader["Type"].ToString();
                        visit.Observation = reader["Observation"].ToString();
                        visit.Latitude = reader["U_VIS_Latitude"].ToString();
                        visit.Longitude = reader["U_VIS_Longitude"].ToString();
                        listVisit.Add(visit);
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {
                
                
            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }
            return listVisit;
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
        ~VisitDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
