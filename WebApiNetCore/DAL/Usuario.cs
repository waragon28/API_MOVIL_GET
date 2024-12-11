//#define VISTONY

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;

using Sap.Data.Hana;
using System.Data;
using SAP_Core.Utils;
using SalesForce.BO;
using Microsoft.AspNetCore.Components;
using SalesForce.Util;
using static WebApiNetCore.Utils.Other;
using WebApiNetCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json.Linq;
//using System.Configuration;

namespace SAP_Core.DAL
{
   public  class UsuarioDAL : Connection,IDisposable 
    {

        public ListUsuario Get_Usuario (string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            var user = new ListUsuario();
            List<UsuarioBO> listUsuario = new List<UsuarioBO>();
            UsuarioBO usuario = new UsuarioBO();
            string strSQL  = string.Format("CALL {0}.APP_USERS ('{1}')", DataSource.bd(), imei);
           
            try
            {
                
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usuario = new UsuarioBO();
                        usuario.Imei = reader["Imei"].ToString();
                        usuario.CompanyCode= reader["Compania_ID"].ToString().ToUpper();
                        usuario.Languaje = reader["Languaje"].ToString().ToUpper();
                        usuario.CompanyName = reader["NombreCompania"].ToString().ToUpper();
                        usuario.UserName = reader["NombreUsuario"].ToString().ToUpper();
                        usuario.UserCode = reader["Usuario_ID"].ToString().ToUpper();
                        usuario.WarehouseCode = reader["Almacen_ID"].ToString();
                        usuario.SalesPersonCode = reader["FuerzaTrabajo_ID"].ToString();
                        usuario.Profile = reader["Perfil"].ToString().ToUpper();
                        usuario.Rate = Convert.ToDecimal(reader["TipoCambio"].ToString());
                        usuario.CogsAcct = reader["CogsAcct"].ToString();
                        usuario.DiscAccount = reader["U_VIST_CTAINGDCTO"].ToString();
                        usuario.DocumentsOwner = reader["DocumentsOwner"].ToString();
                        usuario.Receipt = Convert.ToInt32(reader["Recibo"].ToString());
                        usuario.Branch = reader["U_VIST_SUCUSU"].ToString().ToUpper();
                        usuario.CostCenter = reader["CentroCosto"].ToString().ToUpper();
                        usuario.BusinessUnit = reader["UnidadNegocio"].ToString().ToUpper();
                        usuario.ProductionLine = reader["LineaProduccion"].ToString().ToUpper();

                        listUsuario.Add(usuario);
                    }
                }
                user.Users = listUsuario;
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
            return user;
        }
        
        public ListUsers getUserCL(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            var user = new ListUsers();
            List<User> listUsuario = new List<User>();
            User usuario = new User();
            string strSQL = string.Format("CALL {0}.APP_USERS ('{1}')", DataSource.bd(), imei);
            
            try
            {

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<ConfigUser> lu = new List<ConfigUser>();
                        usuario = new User();
                        usuario.Imei = reader["Imei"].ToString();
                        usuario.CompanyCode = reader["Compania_ID"].ToString().ToUpper();
                        usuario.CompanyName = reader["NombreCompania"].ToString().ToUpper();
                        usuario.Country = reader["Country"].ToString();
                        usuario.UserName = reader["NombreUsuario"].ToString().ToUpper();
                        usuario.UserCode = reader["DocumentsOwner"].ToString().ToUpper();
                        usuario.Profile = reader["Perfil"].ToString().ToUpper();
                        usuario.Phone = reader["Mobile"].ToString();
                        usuario.Branch = reader["U_VIST_SUCUSU"].ToString().ToUpper();
                        usuario.WhsCode = reader["Almacen_ID"].ToString();
                        usuario.SlpCode = reader["FuerzaTrabajo_ID"].ToString();
                        usuario.Sectorist = reader["Sectorista"].ToString();
                        usuario.Census = reader["Census"].ToString();
                        usuario.Quotation = reader["Cotizacion"].ToString().ToUpper();
                        usuario.Rate = reader["TipoCambio"] == null ? 0 : Convert.ToDecimal(reader["TipoCambio"]);
                        usuario.Status = reader["Status"] == null ? "Y" : Convert.ToString(reader["Status"]);
                        usuario.SendVisits = reader["SendVisits"] == null ? "Y" : Convert.ToString(reader["SendVisits"]);
                        usuario.SendValidations = reader["SendValidations"] == null ? "Y" : Convert.ToString(reader["SendValidations"]);
                        lu = getConfig(imei, reader["Usuario_ID"].ToString());
                        
                        usuario.Settings = lu.Count() > 0 ? lu : null;
                        listUsuario.Add(usuario);
                    }
                }
                user.Users = listUsuario;
                connection.Close();
            }
            catch (Exception ex)
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
            return user;
        }
        public List<ConfigUser> getConfig(string imei, string empID)
        {
            HanaDataReader reader;

            HanaConnection connection= GetConnection();


            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            List<ConfigUser> listUsuario = new List<ConfigUser>();
            
            ConfigUser usuario = new ConfigUser();
            string strSQL  = string.Format("CALL {0}.APP_USERS_CONFIG ('{1}','{2}')", DataSource.bd(), imei, empID);
            

            try
            {

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usuario = new ConfigUser();
                        usuario.Language = reader["Language"].ToString().ToUpper();
                        usuario.CostCenter = reader["CentroCosto"].ToString();
                        usuario.ProductionLine = reader["LineaProduccion"].ToString();
                        usuario.BusinessUnit = reader["UnidadNegocio"].ToString();
                        usuario.DiscAccount = reader["U_VIST_CTAINGDCTO"].ToString();
                        usuario.CogsAcct = reader["CogsAcct"].ToString();
                        usuario.TaxCode = reader["TaxCode"].ToString();
                        usuario.TaxRate = Convert.ToDecimal(reader["TaxRate"].ToString());
                        usuario.OutStock = reader["OutStock"].ToString();
                        usuario.Receip = reader["Recibo"].ToString();
                        usuario.Db = reader["GetDB"].ToString();
                        usuario.UsePrinter = reader["UsePrint"].ToString();
                        usuario.MaxDateDeposit = Convert.ToInt16(reader["MaxDateDeposit"]);
                        usuario.CashDscnt = reader["CashDscnt"] == null ? 0 : Convert.ToDecimal(reader["CashDscnt"].ToString());
                        usuario.ChangeCurrency = reader["ChgCurrency"].ToString();
                        usuario.OilTaxStatus = reader["OilTaxStatus"] == null ? "N" : reader["OilTaxStatus"].ToString();
                        usuario.FechaEntregaAuto = reader["FechaEntregaAuto"].ToString().ToUpper();
                        usuario.DeliveryRefusedMoney = reader["DeliveryRefusedMoney"] == null ? "Y" : Convert.ToString(reader["DeliveryRefusedMoney"]);
                        usuario.U_VIS_ManagementType = reader["U_VIS_ManagementType"].ToString().ToUpper();
                        usuario.Superviser = reader["Superviser"].ToString().ToUpper();
                        usuario.isInspectionValidated = reader["isInspectionValidated"].ToString().ToUpper();

                        usuario.ChangeWarehouse = reader.GetSchemaTable().Columns.Contains("ChangeWarehouse") && !reader.IsDBNull(reader.GetOrdinal("ChangeWarehouse"))
    ? reader["ChangeWarehouse"].ToString().ToUpper()
    : reader["ChangeWarehouse"].ToString().ToUpper();
                        usuario.UpdateCustomer = reader.GetSchemaTable().Columns.Contains("UpdateCustomer") && !reader.IsDBNull(reader.GetOrdinal("UpdateCustomer"))
    ? reader["UpdateCustomer"].ToString().ToUpper()
    : reader["UpdateCustomer"].ToString().ToUpper();
                        usuario.CustomerRecovery = ColumnExists(reader, "CustomerRecovery") ? reader["CustomerRecovery"].ToString() : null;
                        usuario.TypeTaxOilTax = ColumnExists(reader, "TypeTaxOilTax") ? reader["TypeTaxOilTax"].ToString() : null;

                        

                        listUsuario.Add(usuario);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
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
            return listUsuario;
        }
        bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        string cookieKey;
        public async Task<ResponseLoginV2> login(Acceso acceso)
        {
            acceso.CompanyDB = DataSource.bd();
            ResponseLoginV2 responseLogin =LoginSAP.LoginApprov(0, null, acceso,ref cookieKey);
            List<PerfilApproval> lOption = new List<PerfilApproval>();

            try
            {
                HanaDataReader reader;
                HanaConnection connection = GetConnection();

                PerfilApproval perfilApproval = new PerfilApproval();
                string strSQL = string.Format("CALL {0}.APP_USERS_MENU_APPROVAL ('{1}')", DataSource.bd(),acceso.UserName);
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        perfilApproval = new PerfilApproval();
                        perfilApproval.Option = reader["Option"].ToString().ToUpper();
                        lOption.Add(perfilApproval);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            responseLogin.Accesos = lOption;

            return responseLogin;
        }
        public async Task<LoginSL> loginServiceLayer()
        {
            try
            {

                string url = Startup.Configuration.GetValue<string>("ServiceLayer:PathUri")+"/b1s/v1/Login";//get

                Acceso acceso = new()
                {
                    CompanyDB = Startup.Configuration.GetValue<string>("SL:PE:CompanyDB"),
                    UserName = Startup.Configuration.GetValue<string>("SL:PE:UserName"),
                    Password = Startup.Configuration.GetValue<string>("SL:PE:Password")
                };
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient cliente = new(clientHandler);
                StringContent content = new(JsonSerializer.Serialize(acceso), Encoding.UTF8, "application/json");

                var respuesta = await cliente.PostAsync(url, content);

                var responseBody = await respuesta.Content.ReadAsStringAsync();
                // Parsear el JSON a un JObject
                JObject jsonObject = JObject.Parse(responseBody);

                LoginSL loginSL = new LoginSL();

                int sessionTimeoutMinutes = jsonObject["SessionTimeout"]?.ToObject<int>() ?? 0;
                DateTime baseTime = DateTime.Now; // O cualquier otra fecha base
                DateTime sessionExpirationTime = baseTime.AddMinutes(sessionTimeoutMinutes);

                loginSL.expityTime = sessionExpirationTime;
                loginSL.token = jsonObject["SessionId"]?.ToString();



                LoginSL r = loginSL;



                return r;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

        public async Task LogoutServiceLayer()
        {
            try
            {
                string url = Startup.Configuration.GetValue<string>("SL:Logout");
;
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient cliente = new(clientHandler);
                StringContent content = new("", Encoding.UTF8, "application/json");

                var respuesta = await cliente.PostAsync(url, content);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
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
        ~UsuarioDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

#endregion


    }// fin de la clase

}
