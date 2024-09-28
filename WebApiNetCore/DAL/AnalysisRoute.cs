
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
    public class AnalysisRouteDAL : Connection, IDisposable
    {
        public ListAnalysisRoute GetAnalysis(string imei, string dia)
        {
            HanaDataReader reader;
            HanaConnection connection;

            ListAnalysisRoute analysisRoutess = new ListAnalysisRoute();

            List<AnalysisRouteBO> analysisRouteBOBOs = new List<AnalysisRouteBO>();

            AnalysisRouteBO analysisRouteBO;
            string strSQL = string.Empty;

            strSQL = string.Format("CALL {0}.APP_SALES_ANALYSIS_BY_ROUTE('{1}','{2}') ", DataSource.bd(), imei, dia);

            connection = GetConnection();
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        analysisRouteBO = new AnalysisRouteBO();
                        analysisRouteBO.CardCode = reader["CardCode"].ToString();
                        analysisRouteBO.CardName = reader["CardName"].ToString();
                        analysisRouteBO.ShipToCode = Int32.Parse(reader["ShipToCode"].ToString());
                        analysisRouteBO.Street = reader["Street"].ToString();
                        analysisRouteBO.TerritoryID = reader["TerritoryID"].ToString();
                        analysisRouteBO.Territory = reader["Territory"].ToString();
                        analysisRouteBO.Day = reader["Day"].ToString();
                        analysisRouteBO.CommercialClass = reader["CommercialClass"].ToString();
                        analysisRouteBO.GallonCurrentYearCurrentPeriod = Double.Parse(reader["GallonCurrentYearCurrentPeriod"].ToString());
                        analysisRouteBO.GallonCurrentYearPreviousPeriod = Double.Parse(reader["GallonCurrentYearPreviousPeriod"].ToString());
                        analysisRouteBO.GallonCurrentYearSecondPriorPeriod = Double.Parse(reader["GallonCurrentYearSecondPriorPeriod"].ToString());
                        analysisRouteBO.GallonPreviousYearCurrentPeriod = Double.Parse(reader["GallonPreviousYearCurrentPeriod"].ToString());
                        analysisRouteBO.GallonPreviousYearPreviousPeriod = Double.Parse(reader["GallonPreviousYearPreviousPeriod"].ToString());

                        analysisRouteBO.GallonPreviousYearSecondPreviousPeriod = Double.Parse(reader["GallonPreviousYearSecondPreviousPeriod"].ToString());
                        analysisRouteBO.AverageQuarterCurrentYear = Double.Parse(reader["AverageQuarterCurrentYear"].ToString());
                        analysisRouteBO.AverageQuarterPreviousYear = Double.Parse(reader["AverageQuarterPreviousYear"].ToString());

                        analysisRouteBO.Indicator1 = Double.Parse(reader["Indicator1"].ToString());
                        analysisRouteBO.Indicator2 = Double.Parse(reader["Indicator2"].ToString());
                        analysisRouteBO.Indicator3 = Double.Parse(reader["Indicator3"].ToString());
                        analysisRouteBO.Quota = Double.Parse(reader["Quota"].ToString());

                        analysisRouteBOBOs.Add(analysisRouteBO);

                    }
                }

                analysisRoutess.AnalysisRoutes = analysisRouteBOBOs;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetAnalysis - " + ex.Message + "Parametros "+ imei+ " "+ dia);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return analysisRoutess;
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
        ~AnalysisRouteDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
