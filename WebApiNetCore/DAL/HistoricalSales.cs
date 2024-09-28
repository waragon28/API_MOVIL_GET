
using Sap.Data.Hana;
using SAP_Core.BO;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class HistoricalSalesDAL : Connection, IDisposable
    {
        public ListHistoricalSalesBO GetHistoricalType(string imei, string type, string cardCode, string fecini, string fecfin)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<HistoricalSalesBO> listRecord = new List<HistoricalSalesBO>();
            ListHistoricalSalesBO records = new ListHistoricalSalesBO();
            HistoricalSalesBO record = new HistoricalSalesBO();
            string strSQL = string.Format("CALL {0}.APP_HISTORICAL_SALES_TYPE ('{1}','{2}','{3}','{4}','{5}')", DataSource.bd(), imei, type, cardCode, fecini, fecfin);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    record = new HistoricalSalesBO();
                    record.SlpCode = reader["FUERZATRABAJO_ID"].ToString().ToUpper();
                    record.Year = reader["ANIO"].ToString().ToUpper();
                    record.Month = reader["MES"].ToString();
                    record.Variable = reader["VARIABLE"].ToString();
                    record.TotalMN = Convert.ToDecimal(reader["TOTAL_MN"].ToString());
                    record.Galones = Convert.ToDecimal(reader["Galones"]);
                    record.UOM = reader["UOM"].ToString();
                    listRecord.Add(record);
                }
            }
            records.RecordSales = listRecord;
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

            return records;

        }
        public ListHistoricalSalesBO GetHistoricalVariable(string imei, string fecini, string fecfin)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<HistoricalSalesBO> listRecord = new List<HistoricalSalesBO>();
            ListHistoricalSalesBO records = new ListHistoricalSalesBO();
            HistoricalSalesBO record = new HistoricalSalesBO();
            string strSQL  = string.Format("CALL {0}.APP_HISTORICAL_SALES_VARIABLE ('{1}','{2}','{3}')", DataSource.bd(), imei, fecini, fecfin);

           

            try {


                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        record = new HistoricalSalesBO();
                        record.SlpCode = reader["FUERZATRABAJO_ID"].ToString().ToUpper();
                        record.Year = reader["ANIO"].ToString().ToUpper();
                        record.Month = reader["MES"].ToString();
                        record.Variable = reader["VARIABLE"].ToString();
                        record.TotalMN = Convert.ToDecimal(reader["TOTAL_MN"].ToString());
                        record.Galones = Convert.ToDecimal(reader["Galones"]);
                        record.UOM = reader["UOM"].ToString();
                        listRecord.Add(record);
                    }
                }
                records.RecordSales = listRecord;
                connection.Close();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return records;

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
        ~HistoricalSalesDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
