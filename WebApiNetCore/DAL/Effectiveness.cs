using Sap.Data.Hana;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.BO;
using WebApiNetCore.Utils;

namespace WebApiNetCore.DAL
{
    public class EffectivenessDAL : Connection, IDisposable
    {
        public ListEffectiveness GetEffectiveness(string imei, string fini, string ffin)
        {
            HanaDataReader reader;
            HanaConnection connection;
            ListEffectiveness effectiveness = new ListEffectiveness();
            List<EffectivenessBO> effectivenessBOs = new List<EffectivenessBO>();
            //List<DireccionClienteBO> listDireccion = null;
            EffectivenessBO effectivenessBO = new EffectivenessBO();
            string strSQL = string.Empty;

            strSQL = string.Format("CALL {0}.APP_SUMMARY_OF_EFFECTIVENESS('{1}','{2}','{3}') ", DataSource.bd(), imei, fini, ffin);

            connection = GetConnection();
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        effectivenessBO = new EffectivenessBO();
                        effectivenessBO.Route = reader["Ruta"].ToString();
                        effectivenessBO.Customers = reader["CLIENTE"].ToString();
                        effectivenessBO.Visit = reader["VISITA"].ToString();
                        effectivenessBO.SalesOrder = reader["PEDIDO"].ToString();
                        effectivenessBO.Collection = reader["COBRANZA"].ToString();
                        effectivenessBO.Debtor = reader["CLIENTEDEUDA"].ToString();
                        effectivenessBO.AmountSO = reader["MONTOPEDIDO"].ToString();
                        effectivenessBO.AmountCll = reader["MONTOCOBRANZA"].ToString();
                        effectivenessBO.VisitEff = reader["EFECTIVIDAD_VISITA"].ToString();
                        effectivenessBO.OrdersEff = reader["EFECTIVIDAD_PEDIDO"].ToString();
                        effectivenessBO.CollctnEff = reader["EFECTIVIDAD_COBRANZA"].ToString();
                        effectivenessBO.CusCoverage = reader["CLIENTECOBERTURA"].ToString();
                        effectivenessBO.Coverage = reader["COBERTURA"].ToString();
                        effectivenessBO.CoverageEff = reader["EFECTIVIDAD_COBERTURA"].ToString();
                        effectivenessBOs.Add(effectivenessBO);

                    }
                }
                effectiveness.Effecetiveness = effectivenessBOs;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetEffectiveness - " + ex.Message+" "+
                    imei+" "+fini+" "+ffin);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return effectiveness;
        }


        public string GetEffectivenessQuote(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();

            string strSQL = string.Format("CALL {0}.APP_QUOTE_EFFECTIVENESS('{1}')", DataSource.bd(),imei);
            string json = "";

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                json =  Other.sqlDatoToJson(reader);
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetEffectivenessQuote - " + ex.Message+" "+
                    imei);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return json;
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
        ~EffectivenessDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
