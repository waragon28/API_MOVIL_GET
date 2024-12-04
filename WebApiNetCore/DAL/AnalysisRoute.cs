
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
using WebApiNetCore.DAL;
using WebApiNetCore.Utils;
using Sentry;

namespace SAP_Core.DAL
{
    public class AnalysisRouteDAL : Connection, IDisposable
    {

        public ListAnalysisRoute GetAnalysis(string imei, string dia)
        {
            ListAnalysisRoute analysisRoutess = new ListAnalysisRoute();
            List<AnalysisRouteBO> analysisRouteBOBOs = new List<AnalysisRouteBO>();
            AnalysisRouteBO analysisRouteBO;

            string strSQL = string.Format("CALL {0}.APP_SALES_ANALYSIS_BY_ROUTE('{1}','{2}') ", DataSource.bd(), imei, dia);

            try
            {
                using (HanaConnection connection = GetConnection())
                {
                    connection.Open();
                    using (HanaCommand command = new HanaCommand(strSQL, connection))
                    {
                        using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    analysisRouteBO = new AnalysisRouteBO();
                                    analysisRouteBO.CardCode = Other.GetStringValue(reader, "CardCode");
                                    analysisRouteBO.CardName = Other.GetStringValue(reader, "CardName");
                                    analysisRouteBO.ShipToCode = Other.GetIntValue(reader, "ShipToCode");
                                    analysisRouteBO.Street = Other.GetStringValue(reader, "Street");
                                    analysisRouteBO.TerritoryID = Other.GetStringValue(reader, "TerritoryID");
                                    analysisRouteBO.Territory = Other.GetStringValue(reader, "Territory");
                                    analysisRouteBO.Day = Other.GetStringValue(reader, "Day");
                                    analysisRouteBO.CommercialClass = Other.GetStringValue(reader, "CommercialClass");
                                    analysisRouteBO.GallonCurrentYearCurrentPeriod = Other.GetDoubleValue(reader, "GallonCurrentYearCurrentPeriod");
                                    analysisRouteBO.GallonCurrentYearPreviousPeriod = Other.GetDoubleValue(reader, "GallonCurrentYearPreviousPeriod");
                                    analysisRouteBO.GallonCurrentYearSecondPriorPeriod = Other.GetDoubleValue(reader, "GallonCurrentYearSecondPriorPeriod");
                                    analysisRouteBO.GallonPreviousYearCurrentPeriod = Other.GetDoubleValue(reader, "GallonPreviousYearCurrentPeriod");
                                    analysisRouteBO.GallonPreviousYearPreviousPeriod = Other.GetDoubleValue(reader, "GallonPreviousYearPreviousPeriod");
                                    analysisRouteBO.GallonPreviousYearSecondPreviousPeriod = Other.GetDoubleValue(reader, "GallonPreviousYearSecondPreviousPeriod");
                                    analysisRouteBO.AverageQuarterCurrentYear = Other.GetDoubleValue(reader, "AverageQuarterCurrentYear");
                                    analysisRouteBO.AverageQuarterPreviousYear = Other.GetDoubleValue(reader, "AverageQuarterPreviousYear");
                                    analysisRouteBO.Indicator1 = Other.GetDoubleValue(reader, "Indicator1");
                                    analysisRouteBO.Indicator2 = Other.GetDoubleValue(reader, "Indicator2");
                                    analysisRouteBO.Indicator3 = Other.GetDoubleValue(reader, "Indicator3");
                                    analysisRouteBO.Quota = Other.GetDoubleValue(reader, "Quota");

                                    analysisRouteBOBOs.Add(analysisRouteBO);

                                }
                            }
                            analysisRoutess.AnalysisRoutes = analysisRouteBOBOs;
                        }
                    }
                }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetAnalysis", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetAnalysis", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
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
