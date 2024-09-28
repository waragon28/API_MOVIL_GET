
using System;
using System.Collections.Generic;
using System.Linq;
using SAP_Core.BO;
using Sap.Data.Hana;
using Newtonsoft.Json;

namespace SAP_Core.DAL
{
    public class ClienteDAL : Connection,IDisposable
    {
        public ListaClientes GetClientes (string imei)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();;
            ListaClientes clientes = new ListaClientes();
            List<ClienteBO> listCliente = new List<ClienteBO>();
           
            ClienteBO cliente = new ClienteBO();
            string strSQL = string.Format("CALL {0}.APP_CUSTOMERS ('{1}')", Utils.bd(), imei);

            try
            {
                connection.OpenAsync();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DireccionClienteDAL dirDAL = new DireccionClienteDAL();
                        List<DireccionClienteBO> listDirecciones = new List<DireccionClienteBO>();
                        listDirecciones = dirDAL.GetDireccion(imei, reader["Cliente_ID"].ToString());
                        List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
                        DocumentoDeudaDAL documentoDeudaDAL = new DocumentoDeudaDAL();
                        List<VisitBOs> listVisits = new List<VisitBOs>();
                        VisitDAL visitDAL = new VisitDAL();
                        listVisits = visitDAL.GetVisit(imei, reader["Cliente_ID"].ToString());
                        cliente = new ClienteBO();
                        cliente.CardCode = reader["Cliente_ID"].ToString();
                        cliente.CardName = reader["NombreCliente"].ToString().ToUpper();
                        cliente.LicTradNum = reader["RucDni"].ToString().ToUpper();
                        //cliente.Currency = reader["Moneda"].ToString();
                        cliente.Phone = reader["TelefonoFijo"].ToString();
                        cliente.CellPhone = reader["TelefonoMovil"].ToString();
                        cliente.ZipCode = reader["Ubigeo_ID"].ToString().ToUpper();
                        cliente.CreditLimit = Convert.ToDouble(reader["LimiteCredito"].ToString());
                        cliente.Balance = Convert.ToDouble(reader["Saldo"].ToString());
                        cliente.VisitOrder = reader["OrdenVisita"].ToString();
                        cliente.Email = reader["Correo"].ToString();
                        cliente.Category = reader["Categoria"].ToString();
                        cliente.PymntGroup = reader["TerminoPago_ID"].ToString();
                        cliente.PayToCode = reader["PayToCode"].ToString();
                        cliente.PriceList = reader["PriceList"].ToString();
                        cliente.Currency = reader["Currency"].ToString();
                        cliente.DueDays = Convert.ToInt32(reader["DueDays"].ToString());
                        cliente.Addresses = listDirecciones;
                        listDocumentoDeuda = documentoDeudaDAL.GetDocumentoDeudaCliente2(imei, cliente.CardCode);
                        cliente.Invoices = listDocumentoDeuda.Count() != 0 ? listDocumentoDeuda : null;
                        cliente.Visits = listVisits.Count() != 0 ? listVisits : null;
                        listCliente.Add(cliente);
                    }
                }
                clientes.Customers = listCliente;
                connection.Close();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }
            return clientes;
        } 
        public ListaClientes GetCliente (string imei, string cli)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection(); 
            ListaClientes clientes = new ListaClientes();
            List<ClienteBO> listCliente = new List<ClienteBO>();
            ClienteBO cliente = new ClienteBO();
            string strSQL = string.Format("CALL {0}.APP_CUSTOMER_DETAIL ('{1}','{2}')", Utils.bd(), imei, cli);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        DireccionClienteDAL dirDAL = new DireccionClienteDAL();
                        List<DireccionClienteBO> listDirecciones = new List<DireccionClienteBO>();
                        listDirecciones = dirDAL.GetDireccion(imei, reader["Cliente_ID"].ToString());
                        List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
                        DocumentoDeudaDAL documentoDeudaDAL = new DocumentoDeudaDAL();
                        List<VisitBOs> listVisits = new List<VisitBOs>();
                        VisitDAL visitDAL = new VisitDAL();
                        //listVisits = visitDAL.GetVisit(imei, reader["Cliente_ID"].ToString());
                        cliente = new ClienteBO();
                        cliente.CardCode = reader["Cliente_ID"].ToString();
                        cliente.CardName = reader["NombreCliente"].ToString().ToUpper();
                        cliente.LicTradNum = reader["RucDni"].ToString().ToUpper();
                        //cliente.Currency = reader["Moneda"].ToString();
                        cliente.Phone = reader["TelefonoFijo"].ToString();
                        cliente.CellPhone = reader["TelefonoMovil"].ToString();
                        cliente.ZipCode = reader["Ubigeo_ID"].ToString().ToUpper();
                        cliente.CreditLimit = Convert.ToDouble(reader["LimiteCredito"].ToString());
                        cliente.Balance = Convert.ToDouble(reader["Saldo"].ToString());
                        cliente.VisitOrder = reader["OrdenVisita"].ToString();
                        cliente.Email = reader["Correo"].ToString();
                        cliente.Category = reader["Categoria"].ToString();
                        cliente.PymntGroup = reader["TerminoPago_ID"].ToString();
                        cliente.PayToCode = reader["PayToCode"].ToString();
                        cliente.PriceList = reader["PriceList"].ToString();
                        cliente.Currency = reader["Currency"].ToString();
                        cliente.DueDays = Convert.ToInt32(reader["DueDays"].ToString());
                        cliente.Addresses = dirDAL.GetDireccion(imei, reader["Cliente_ID"].ToString());
                        listDocumentoDeuda = documentoDeudaDAL.GetDocumentoDeudaCliente2(imei, cliente.CardCode);
                        cliente.Invoices = listDocumentoDeuda.Count() != 0 ? listDocumentoDeuda : null;
                        //cliente.Visits = listVisits.Count() != 0 ? listVisits : null;
                        listCliente.Add(cliente);
                    }
                }
                clientes.Customers = listCliente;
                connection.Close();
            }
            catch (Exception)
            {
             
            }
            finally
            {
                if (connection.State.Equals("Open"))
                {
                    connection.Close();
                }
            }
            return clientes;
        }
        public ListaClientes2 GetClientes2(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaClientes2 clientes = new ListaClientes2();
            List<ClienteBO2> listCliente = new List<ClienteBO2>();
            //List<DireccionClienteBO> listDireccion = null;
            ClienteBO2 cliente = new ClienteBO2();
            string strSQL = string.Format("CALL {0}.APP_CUSTOMERS3 ('{1}')", Utils.bd(), imei);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                        
                        cliente = new ClienteBO2();
                        cliente.CardCode = reader["CardCode"].ToString();
                        cliente.CardName = reader["CardName"].ToString().ToUpper();
                        cliente.LicTradNum = reader["LicTradNum"].ToString().ToUpper();
                        //cliente.Currency = reader["Moneda"].ToString();
                        cliente.Phone = reader["Phone"].ToString();
                        cliente.CellPhone = reader["CellPhone"].ToString();
                        cliente.ZipCode = reader["ZipCode"].ToString().ToUpper();
                        cliente.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                        cliente.Balance = Convert.ToDecimal(reader["Balance"]);
                        cliente.VisitOrder = reader["VisitOrder"].ToString();
                        cliente.Email = reader["Email"].ToString();
                        cliente.Category = reader["Category"].ToString();
                        cliente.PymntGroup = reader["PymntGroup"].ToString();
                        cliente.PayToCode = reader["PayToCode"].ToString();
                        cliente.PriceList = reader["PriceList"].ToString();
                        cliente.Currency = reader["Currency"].ToString();
                        cliente.LineOfBusiness = reader["LineOfBusiness"]== null?"": reader["LineOfBusiness"].ToString();
                        cliente.LastPurchase = reader["LastPurchase"].ToString();
                        cliente.DueDays = Convert.ToInt32(reader["DueDays"].ToString());
                        cliente.Addresses = reader["Addresses"] == null ? null : JsonConvert.DeserializeObject<List<DireccionClienteBO>>(reader["Addresses"].ToString());
                        dynamic invoice = reader["Invoices"] == null ? null : JsonConvert.DeserializeObject<List<DocumentoDeudaBO>>(reader["Invoices"].ToString());
                        cliente.Invoices = invoice;
                        listCliente.Add(cliente);
                    }
                }
                clientes.Customers = listCliente;
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
            return clientes;
        }
        public ListaClientes2 GetClientesDespacho(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaClientes2 clientes = new ListaClientes2();
            List<ClienteBO2> listCliente = new List<ClienteBO2>();
            ClienteBO2 cliente = new ClienteBO2();
            string strSQL = string.Format("CALL {0}.APP_DESPACHO_CUSTOMER ('{1}','{2}')", Utils.bd(), imei, fecha);

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cliente = new ClienteBO2();
                        cliente.CardCode = reader["CardCode"].ToString();
                        cliente.CardName = reader["CardName"].ToString().ToUpper();
                        cliente.LicTradNum = reader["LicTradNum"].ToString().ToUpper();
                        //cliente.Currency = reader["Moneda"].ToString();
                        cliente.Phone = reader["Phone"].ToString();
                        cliente.CellPhone = reader["CellPhone"].ToString();
                        cliente.ZipCode = reader["ZipCode"].ToString().ToUpper();
                        cliente.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                        cliente.Balance = Convert.ToDecimal(reader["Balance"]);
                        cliente.VisitOrder = reader["VisitOrder"].ToString();
                        cliente.Email = reader["Email"].ToString();
                        cliente.Category = reader["Category"].ToString();
                        cliente.PymntGroup = reader["PymntGroup"].ToString();
                        cliente.PayToCode = reader["PayToCode"].ToString();
                        cliente.PriceList = reader["PriceList"].ToString();
                        cliente.Currency = reader["Currency"].ToString();
                        cliente.DueDays = Convert.ToInt32(reader["DueDays"].ToString());
                        cliente.Addresses = reader["Addresses"] == null ? null : JsonConvert.DeserializeObject<List<DireccionClienteBO>>(reader["Addresses"].ToString());
                        dynamic invoice = reader["Invoices"] == null ? null : JsonConvert.DeserializeObject<List<DocumentoDeudaBO>>(reader["Invoices"].ToString());
                        cliente.Invoices = invoice;
                        listCliente.Add(cliente);
                    }
                }
                clientes.Customers = listCliente;
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

            return clientes;
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
        ~ClienteDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }    
}
