using Sap.Data.Hana;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore.BO;

namespace WebApiNetCore.DAL
{
    public class NotificationDAL : Connection, IDisposable
    {
        public SalesCalendars GetSalesCalendars(string imei, string fromDate, string toDate)
        {
            List<SalesCalendarBO> listQuota = new List<SalesCalendarBO>();
            SalesCalendars quotas = new SalesCalendars();
            SalesCalendarBO quota = new SalesCalendarBO();

            /*
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            string strSQL = string.Format("CALL {0}.APP_SALESCALENDAR ('{1}','{2}','{3}')", DataSource.bd(), imei, fromDate,toDate);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        quota = new SalesCalendarBO();
                        quota.Code = Convert.ToInt32(reader["Code"]);
                        quota.Year = reader["Year"].ToString().ToUpper();
                        quota.Month = reader["Month"].ToString();
                        quota.Day = reader["Day"].ToString();
                        quota.Habil = reader["Habil"].ToString();
                        listQuota.Add(quota);
                    }
                }

                quotas.SalesCalendar = listQuota;
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            */
            quota = new SalesCalendarBO();
            quota.Code =1;
            quota.Year = "2024";
            quota.Month = "11";
            quota.Day = "18";
            quota.Habil ="N";


            return quotas;
        }
        public QuotationNotifications GetQuotaNotification(GetQuotation query)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            QuotationNotifications quotas = new QuotationNotifications();
            QuotationNotification quota = new QuotationNotification();
            List<QuotationNotification> lquota = new List<QuotationNotification>();
            try
            {
                string docEntry = "";
                for (int row = 0; row < query.DocEntry.Count; row++)
                {
                    docEntry += query.DocEntry[row].ToString() + ",";
                }
                docEntry = docEntry.TrimEnd(',');
                string strSQL = string.Format("CALL {0}.APP_NOTIFICATION_QUOTATION ('{1}',ARRAY({2}))", DataSource.bd(),query.Imei,docEntry);
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        quota = new QuotationNotification();
                        quota.Object = reader["Object"].ToString();
                        quota.Cliente_ID = reader["Cliente_ID"].ToString();
                        quota.NombreCliente = reader["NombreCliente"].ToString();
                        quota.RucDni = reader["RucDni"].ToString();
                        quota.DocEntry = reader["DocEntry"].ToString();
                        quota.DocNum = reader["DocNum"].ToString();
                        quota.DomEmbarque_ID = reader["DomEmbarque_ID"].ToString();
                        quota.DomEmbarque = reader["DomEmbarque"].ToString();
                        quota.CANCELED = reader["CANCELED"].ToString();
                        quota.MontoTotalOrden = reader["MontoTotalOrden"].ToString();
                        quota.EstadoAprobacion = reader["EstadoAprobacion"].ToString();
                        quota.OrdenVenta_ID = reader["OrdenVenta_ID"].ToString();
                        quota.SlpCode = reader["SlpCode"].ToString();
                        quota.PymntGroup = reader["PymntGroup"].ToString();
                        lquota.Add(quota);
                    }
                    quotas.Quotation = lquota;
                }
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return quotas;
        }
        public Services GetService(string imei, string code)
        {

            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<ServiceBO> listQuota = new List<ServiceBO>();
            Services quotas = new Services();
            ServiceBO quota = new ServiceBO();
            string strSQL = string.Format("CALL {0}.APP_SERVICE ('{1}','{2}')", DataSource.bd(), imei, code);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        quota = new ServiceBO();
                        quota.Description = reader["U_VIS_Description"].ToString();
                        quota.StartTime = Convert.ToInt32(reader["U_VIS_StartTime"]);
                        quota.EndTime = Convert.ToInt32(reader["U_VIS_EndTime"]);
                        quota.Interval = Convert.ToInt32(reader["U_VIS_Interval"]);
                        quota.Status = reader["U_VIS_Status"].ToString();
                        quota.DocEntry = Convert.ToInt32(reader["DocEntry"]);
                        listQuota.Add(quota);
                    }
                }

                quotas.Service = listQuota;
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return quotas;
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
        ~NotificationDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
