
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
    public class QuotaDAL : Connection, IDisposable
    {
        public ListQuotaBO GetQuotas(string CardCode, string SlpCode)
        {
           
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<QuotaBO> listQuota = new List<QuotaBO>();
            ListQuotaBO quotas = new ListQuotaBO();
            QuotaBO quota = new QuotaBO();
            string strSQL = string.Format("CALL {0}.P_VIS_CALCULO_CUOTA_CABECERA ('{1}','{2}')", DataSource.bd(), CardCode, SlpCode);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        quota = new QuotaBO();
                        quota.PymntGrp = reader["Condicion Pago"].ToString().ToUpper();
                        quota.Tramo = reader["Tramo"].ToString().ToUpper();
                        quota.Balance = Convert.ToDecimal(reader["Saldo"].ToString());
                        quota.Quota = Convert.ToInt32(reader["Cuotas"].ToString());
                        listQuota.Add(quota);
                    }
                }
               
                quotas.Quotas = listQuota;
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

            return quotas;
        }
        public ListQuotaDetailBO GetQuotaDetail(string SlpCode, string CardCode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<QuotaDetailBO> listQuota = new List<QuotaDetailBO>();
            ListQuotaDetailBO quotas = new ListQuotaDetailBO();
            QuotaDetailBO quota = new QuotaDetailBO();
            string strSQL = string.Format("CALL {0}.P_VIS_SIS_TABLATEMP ('{1}','{2}')", DataSource.bd(), SlpCode, CardCode);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    quota = new QuotaDetailBO();
                    quota.QuotaNumber = reader["NROCUOTA"].ToString();
                    quota.Order = Convert.ToDecimal(reader["PEDIDO"]);
                    quota.Corriente = Convert.ToDecimal(reader["CORRIENTE"].ToString());
                    quota.Due = Convert.ToDecimal(reader["VENCIDO"]);
                    quota.Total = Convert.ToDecimal(reader["TOTAL"]);
                    quota.Date = reader["FECHA"].ToString();
                    listQuota.Add(quota);
                }
            }
            quotas.QuotasDetail = listQuota;
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

            return quotas;

        }
        public ListQuotaInvoiceBO GetQuotaInvoice(string SlpCode, string CardCode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<QuotaInvoiceBO> listQuota = new List<QuotaInvoiceBO>();
            ListQuotaInvoiceBO quotas = new ListQuotaInvoiceBO();
            QuotaInvoiceBO quota = new QuotaInvoiceBO();
            string strSQL = string.Format("CALL {0}.P_VIS_CALCULO_CABECERA_CUOTA_FACTURA ('{1}','{2}')", DataSource.bd(), SlpCode, CardCode);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    quota = new QuotaInvoiceBO();
                    quota.TaxDate = reader["FECHAEMISION"].ToString();
                    quota.DueDate = reader["FECHAVENCIMIENTO"].ToString();
                    quota.NumInvoince = reader["NROFACTURA"].ToString();
                    quota.DocTotal = Convert.ToDecimal(reader["IMPORTEFACTURA"]);
                    quota.PymntGroup = reader["CONDICION_PAGO"].ToString();
                    quota.DueDays = reader["DIAS_MORA"].ToString();
                    quota.QuotaAmmount = Convert.ToDecimal(reader["MONTO_CUOTA"]);
                    quota.QuotaNumber = Convert.ToInt32(reader["CUOTA"]);
                    quota.Type = reader["TIPO"].ToString();
                    quota.Dues = reader["DIASVENCIMIENTO"].ToString();
                    quota.Balance = Convert.ToDecimal(reader["SALDO"]);
                    listQuota.Add(quota);
                }
            }
            quotas.QuotaInvoices = listQuota;
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
        ~QuotaDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
