﻿
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
    public class InvoicesDAL : Connection, IDisposable
    {
        public ListaDocumentoDeuda GetInvoices(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaDocumentoDeuda documentos = new ListaDocumentoDeuda();
            List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
            DocumentoDeudaBO documentoDeuda = new DocumentoDeudaBO();
            string strSQL  = string.Format("CALL {0}.APP_INVOICES_DATE ('{1}','{2}')", DataSource.bd(), imei, fecha);

           
            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        documentoDeuda = new DocumentoDeudaBO();
                        documentoDeuda.DocEntry = reader["DocEntry"].ToString();
                        documentoDeuda.DocNum = reader["DocNum"].ToString().ToUpper();
                        documentoDeuda.ShipToCode = reader["DomEmbarque_ID"].ToString().ToUpper();
                        documentoDeuda.LegalNumber = reader["NroFactura"].ToString();
                        documentoDeuda.TaxDate = reader["FechaEmision"].ToString();
                        documentoDeuda.DueDate = reader["FechaVencimiento"].ToString();
                        documentoDeuda.DueDays = reader["DiasVencidos"].ToString();
                        documentoDeuda.Currency = reader["Moneda"].ToString();
                        documentoDeuda.DocTotal = Convert.ToDecimal(reader["ImporteFactura"].ToString());
                        documentoDeuda.Balance = Convert.ToDecimal(reader["Saldo"].ToString());
                        documentoDeuda.RawBalance = Convert.ToDecimal(reader["Saldo_Sin_Procesar"].ToString());
                        documentoDeuda.Driver = reader["Chofer"].ToString();
                        documentoDeuda.IDDriver = reader["empID"].ToString();
                        documentoDeuda.Mobile = reader["mobile"].ToString();
                        documentoDeuda.CardCode = reader["Cliente_ID"].ToString();
                        documentoDeuda.CardName = reader["CardName"].ToString();
                        documentoDeuda.LicTradNum = reader["LicTradNum"].ToString();
                        documentoDeuda.DeliveryDate = reader["FechaDespacho"].ToString();
                        documentoDeuda.DeliveryStatus = reader["EstadoDespacho"].ToString();
                        documentoDeuda.SalesOrderID = reader["U_VIS_SalesOrderID"].ToString();
                        documentoDeuda.PymntGroup = reader["PymntGroup"].ToString();
                        listDocumentoDeuda.Add(documentoDeuda);
                    }
                }
                documentos.Documents = listDocumentoDeuda;
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


            return documentos;

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
        ~InvoicesDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
