
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class DocumentoDeudaDAL : Connection, IDisposable
    {
        public ListaDocumentoDeuda GetDocumentoDeuda(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaDocumentoDeuda documentos = new ListaDocumentoDeuda();
            List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
            DocumentoDeudaBO documentoDeuda = new DocumentoDeudaBO();
            string strSQL  = string.Format("CALL {0}.APP_INVOICES ('{1}')", Utils.bd(), imei);
            
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
                        listDocumentoDeuda.Add(documentoDeuda);
                    }
                }
                documentos.Documents = listDocumentoDeuda;
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


            return documentos;

        }
        public ListaDocumentoDeuda GetDocumentoDeudaCliente(string imei, string cliente)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaDocumentoDeuda documentos = new ListaDocumentoDeuda();
            List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
            DocumentoDeudaBO documentoDeuda = new DocumentoDeudaBO();
            string strSQL = string.Format("CALL {0}.APP_INVOICES ('{1}','{2}')", Utils.bd(), imei, cliente);

           
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
                        listDocumentoDeuda.Add(documentoDeuda);
                    }
                }
                documentos.Documents = listDocumentoDeuda;
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


            return documentos;

        }
        public List<DocumentoDeudaBO> GetDocumentoDeudaCliente2(string imei, string cliente)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaDocumentoDeuda documentos = new ListaDocumentoDeuda();
            List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
            DocumentoDeudaBO documentoDeuda = new DocumentoDeudaBO();
            string strSQL = string.Format("CALL {0}.APP_INVOICES ('{1}','{2}')", Utils.bd(), imei, cliente);

           
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
                        listDocumentoDeuda.Add(documentoDeuda);
                    }
                }
                documentos.Documents = listDocumentoDeuda;
                connection.Close();
            }
            catch (Exception ex)
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


            return listDocumentoDeuda;

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
        ~DocumentoDeudaDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
