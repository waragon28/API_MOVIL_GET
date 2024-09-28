using Sap.Data.Hana;
using SAP_Core.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class TipoDespachoDAL : Connection, IDisposable
    {
        public ListTipoDespacho GetOcurrencias(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<TipoDespachoBO> listTipoDespacho = new List<TipoDespachoBO>();
            ListTipoDespacho TiposDespacho = new ListTipoDespacho();
            TipoDespachoBO tipoDespacho = new TipoDespachoBO();
            string strSQL = string.Format("CALL {0}.APP_STATUS_DISPATCH ('{1}')", Utils.bd(), imei);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tipoDespacho = new TipoDespachoBO();
                    tipoDespacho.Value = reader["Value"].ToString().ToUpper();
                    tipoDespacho.Dscription = reader["Dscription"].ToString().ToUpper();
                    listTipoDespacho.Add(tipoDespacho);
                }
            }
            TiposDespacho.DispatchTypes = listTipoDespacho;
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


            return TiposDespacho;

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
        ~TipoDespachoDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
