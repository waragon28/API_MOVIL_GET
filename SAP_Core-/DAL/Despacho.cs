
using Newtonsoft.Json;
using Sap.Data.Hana;
using SAP_Core.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class DespachoDAL : Connection, IDisposable
    {
        public ListaDespachoHeaderBO GetDespachoHeader(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaDespachoHeaderBO despachos = new ListaDespachoHeaderBO();
            List<DespachoHeaderBO> listaDespacho = new List<DespachoHeaderBO>();
            DespachoHeaderBO despacho = new DespachoHeaderBO();
            string strSQL = string.Format("CALL {0}.APP_DESPACHO_HEADER ('{1}','{2}')", Utils.bd(), imei, fecha);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        despacho = new DespachoHeaderBO();
                        despacho.ControlCode = reader["ControlCode"].ToString();
                        despacho.Assistant = reader["Assistant"].ToString().ToUpper();
                        despacho.Brand = reader["Brand"].ToString();
                        despacho.OverallWeight = Convert.ToDecimal(reader["OverallWeight"]);
                        despacho.LicensePlate = reader["LicensePlate"].ToString();
                        despacho.Detail = reader["Detail"] == null ? null : JsonConvert.DeserializeObject<List<DetailDespacho>>(reader["Detail"].ToString());
                        listaDespacho.Add(despacho);
                    }
                }
                despachos.Obtener_DespachoCResult = listaDespacho;
                connection.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }



            return despachos;

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
        ~DespachoDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
