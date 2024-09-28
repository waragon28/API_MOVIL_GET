﻿using Newtonsoft.Json;
using Sap.Data.Hana;
using SAP_Core.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class TraceabilityDAL : Connection, IDisposable
    {
        public ListTraceability GetTraceability(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<TraceabilityBO> ListTraceability = new List<TraceabilityBO>();
            ListTraceability traceabilties = new ListTraceability();
            TraceabilityBO traceability = new TraceabilityBO();
            string strSQL  = string.Format("CALL {0}.APP_SALES_ORDER_TRACEABILITY ('{1}','{2}')", Utils.bd(), imei, fecha);

            try { 
            connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    traceability = new TraceabilityBO();
                    traceability.Object = reader["Object"].ToString();
                    traceability.CardCode = reader["Cliente_ID"].ToString();
                    traceability.CardName = reader["NombreCliente"].ToString();
                    traceability.LicTradNum = reader["RucDni"].ToString();
                    traceability.DocEntry = Convert.ToInt32(reader["DocEntry"].ToString());
                    traceability.DocNum = reader["DocNum"].ToString();
                    traceability.Address = reader["DomEmbarque_ID"].ToString();
                    traceability.Street = reader["DomEmbarque"].ToString();
                    traceability.Canceled = reader["CANCELED"].ToString();
                    traceability.Amount = Convert.ToDecimal(reader["MontoTotalOrden"].ToString());
                    traceability.Status = reader["EstadoAprobacion"].ToString();
                    traceability.SalesOrderID = reader["OrdenVenta_ID"].ToString();
                    traceability.SlpCode = reader["SlpCode"].ToString();
                    traceability.PymntGroup = reader["PymntGroup"].ToString();
                    traceability.Invoices = reader["Invoices"] == null ? null : JsonConvert.DeserializeObject<List<DocumentoDeudaBO>>(reader["Invoices"].ToString());
                    ListTraceability.Add(traceability);
                }
            }
            traceabilties.Traceabilities = ListTraceability;
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


            return traceabilties;

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
        ~TraceabilityDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}