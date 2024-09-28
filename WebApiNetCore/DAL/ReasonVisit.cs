
using Sap.Data.Hana;
using SAP_Core.BO;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class ReasonVisitDAL : Connection, IDisposable
    {
        public ListReasonVisit GetReasons(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<ReasonVisitBO> listReason = new List<ReasonVisitBO>();
            ListReasonVisit reasons = new ListReasonVisit();
            ReasonVisitBO reason = new ReasonVisitBO();
            string strSQL  = string.Format("CALL {0}.APP_REASONVISIT ('{1}')", DataSource.bd(), imei);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    reason = new ReasonVisitBO();
                    reason.Code = reader["CodigoMotivo"].ToString().ToUpper();
                    reason.Name = reader["Motivo"].ToString().ToUpper();
                    reason.Type = reader["TipoVisita"].ToString();
                    listReason.Add(reason);
                }
            }
            reasons.Reasons = listReason;
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

            return reasons;

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
        ~ReasonVisitDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }

}
