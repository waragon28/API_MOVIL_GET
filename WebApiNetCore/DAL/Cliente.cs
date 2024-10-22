//#define VISTONY
//#define ROFALAB

using System;
using System.Collections.Generic;
using System.Linq;
using SAP_Core.BO;
using Sap.Data.Hana;
using System.Data;
using SAP_Core.Utils;
using Microsoft.Extensions.Caching.Memory;
using SalesForce.Util;
using System.Threading.Tasks;
using SalesForce.BO;
using WebApiNetCore;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
using System.Text.Json;
using System.Net;
using static WebApiNetCore.Utils.Other;
using Microsoft.IdentityModel.Tokens;
using WebApiNetCore.DAL;
using System.Runtime.ConstrainedExecution;

namespace SAP_Core.DAL
{
    public class ClienteDAL : Connection,IDisposable
    {
        private ServiceLayer serviceLayer;
        private static readonly string _awsAccessKey  = Startup.Configuration.GetValue<string>("S3:AWSAccessKey");
        private static readonly string _awsSecretKey = Startup.Configuration.GetValue<string>("S3:AWSSecretKey");
        private static readonly string _endpoingURL = Startup.Configuration.GetValue<string>("S3:EndpoingURL");
        private static readonly string _bucketName = Startup.Configuration.GetValue<string>("S3:Bucketname");

        CorreoAlert correoAlert = new CorreoAlert();

        public ClienteDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }
        private IMemoryCache _memoryCache;
        public ListaClientes GetClientes (string imei)
        {
            HanaDataReader reader;
            HanaConnection connection=GetConnection();;
            ListaClientes clientes = new ListaClientes();
            List<ClienteBO> listCliente = new List<ClienteBO>();
           
            ClienteBO cliente = new ClienteBO();
            string strSQL = string.Format("CALL {0}.APP_CUSTOMERS ('{1}')", DataSource.bd(), imei);

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
                        DireccionClienteDAL dirDAL = new DireccionClienteDAL();
                        List<DireccionClienteBO> listDirecciones = new List<DireccionClienteBO>();
                        listDirecciones = dirDAL.GetDireccion(imei, reader["Cliente_ID"].ToString());
                        List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
                        DocumentoDeudaDAL documentoDeudaDAL = new DocumentoDeudaDAL();
                        List<VisitBOs> listVisits = new List<VisitBOs>();
                        VisitDAL visitDAL = new VisitDAL(_memoryCache);
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

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Cliente GetClientes DAL Vistony", ex.Message.ToString());
                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetClientes - " + ex.Message + " - " + imei);
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
            return clientes;
        } 
        public ListaClientes GetCliente (string imei, string cli)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection(); 
            ListaClientes clientes = new ListaClientes();
            List<ClienteBO> listCliente = new List<ClienteBO>();
            ClienteBO cliente = new ClienteBO();
            string strSQL = string.Format("CALL {0}.APP_CUSTOMER_DETAIL ('{1}','{2}')", DataSource.bd(), imei, cli);

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
                        DireccionClienteDAL dirDAL = new DireccionClienteDAL();
                        List<DireccionClienteBO> listDirecciones = new List<DireccionClienteBO>();
                        listDirecciones = dirDAL.GetDireccion(imei, reader["Cliente_ID"].ToString());
                        List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
                        DocumentoDeudaDAL documentoDeudaDAL = new DocumentoDeudaDAL();
                        List<VisitBOs> listVisits = new List<VisitBOs>();
                        VisitDAL visitDAL = new VisitDAL(_memoryCache);
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
                        cliente.CustomerWhiteList = reader["CustomerWhiteList"].ToString();
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetClientes - " + ex.Message + " - " + imei+" "+cli);
                HanaCommand command = new HanaCommand(strSQL, connection);

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Cliente GetCliente DAL Vistony", ex.Message.ToString());
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
            return clientes;
        }
        //VALIDACION DE EXISTENCIA DE COLUMNA
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

        public Economic_Activity GetActivity_Economic(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            Economic_Activity ObjEconomic_Activity = new Economic_Activity();
            List<LineEconomic_Activity> ListLineEconomic_Activity = new List<LineEconomic_Activity>();
            LineEconomic_Activity ObjtLineEconomic_Activity = new LineEconomic_Activity();
            ClienteBO2 cliente = new ClienteBO2();
            string strSQL = string.Format("CALL {0}.APP_ECONOMIC_ACTIVITY ('{1}')", DataSource.bd(), imei);

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
                        ObjtLineEconomic_Activity = new LineEconomic_Activity();
                        ObjtLineEconomic_Activity.Name = reader["Name"].ToString();
                        ObjtLineEconomic_Activity.Code = reader["Code"].ToString().ToUpper();
                        ListLineEconomic_Activity.Add(ObjtLineEconomic_Activity);
                    }
                }
                ObjEconomic_Activity.Data = ListLineEconomic_Activity;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetClientes2 - " + ex.Message + " - " + imei);
                HanaCommand command = new HanaCommand(strSQL, connection);

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Cliente GetActivity_Economic DAL Vistony", ex.Message.ToString());
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return ObjEconomic_Activity;
        }
        public ListaClientes2 GetClientes2(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaClientes2 clientes = new ListaClientes2();
            List<ClienteBO2> listCliente = new List<ClienteBO2>();
            //List<DireccionClienteBO> listDireccion = null;
            ClienteBO2 cliente = new ClienteBO2();
            //string strSQL = string.Format("CALL {0}.APP_CUSTOMERS3 ('{1}')", DataSource.bd(), imei);
            string strSQL = string.Format("CALL {0}.APP_CUSTOMERS3 ('{1}')", DataSource.bd(), imei);

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
                        cliente = new ClienteBO2();
                        cliente.CardCode = reader["CardCode"].ToString();
                        cliente.CardName = reader["CardName"].ToString().ToUpper();
                        cliente.SinTerminoContado = reader["SinTerminoContado"].ToString().ToUpper();
                        cliente.LicTradNum = reader["LicTradNum"].ToString().ToUpper();
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
                        cliente.CustomerWhiteList = reader["CustomerWhiteList"].ToString();
                        cliente.Addresses = reader["Addresses"] == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<DireccionClienteBO>>(reader["Addresses"].ToString());
                        dynamic invoice = reader["Invoices"] == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentoDeudaBO>>(reader["Invoices"].ToString());
                        cliente.Invoices = invoice;
                        cliente.LastFreePurchase = ColumnExists(reader, "LastFreePurchase") ? reader["LastFreePurchase"].ToString() : null;
                        cliente.LastTradeMarketing = ColumnExists(reader, "LastTradeMarketing") ? reader["LastTradeMarketing"].ToString() : null;
                        cliente.EconomyActivity = ColumnExists(reader, "EconomyActivity") ? reader["EconomyActivity"].ToString() : null;
                        cliente.CustomerRecovery = ColumnExists(reader, "CustomerRecovery") ? reader["CustomerRecovery"].ToString() : null;
                        listCliente.Add(cliente);
                    }
                }
                clientes.Customers = listCliente;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetClientes2 - " + ex.Message + " - " + imei );
                HanaCommand command = new HanaCommand(strSQL, connection);

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Cliente GetClientes2 DAL Vistony", ex.Message.ToString());
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
            return clientes;
        }
        public ListaClientes2 GetClientesDespacho(string imei, string fecha)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaClientes2 clientes = new ListaClientes2();
            List<ClienteBO2> listCliente = new List<ClienteBO2>();
            ClienteBO2 cliente = new ClienteBO2();
           // string strSQL = string.Format("CALL {0}.APP_DESPACHO_CUSTOMER ('{1}','{2}')", Utils.bd(), imei, fecha);
            string strSQL = string.Format("CALL {0}.APP_DESPACHO_CUSTOMER3 ('{1}','{2}')", DataSource.bd(), imei, fecha);

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
                        cliente.Addresses = reader["Addresses"] == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<DireccionClienteBO>>(reader["Addresses"].ToString());
                        dynamic invoice = reader["Invoices"] == null ? null : Newtonsoft.Json. JsonConvert.DeserializeObject<List<DocumentoDeudaBO>>(reader["Invoices"].ToString());
                        cliente.Invoices = invoice;
                        cliente.CustomerCriticalDistribution= reader["CustomerCriticalDistribution"].ToString();
                        listCliente.Add(cliente);
                    }
                }
                clientes.Customers = listCliente;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetClientesDespacho - " + ex.Message + " - " + imei+" "+fecha);
                HanaCommand command = new HanaCommand(strSQL, connection);

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Cliente GetClientesDespacho DAL Vistony", ex.Message.ToString());
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

            return clientes;
        }

        public async Task<ResponseData> update(inAddressesList addressesList)
        {
            List<AddressResponse> addressResponseList = new();
            try
            {
 var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);

            DateTime localDate = DateTime.Now;

            foreach (Address address in addressesList.Addresses)
            {

                Bpaddress temp = new()
                {
                    BPCode = address.CardCode,
                    RowNum = address.AddressCode,
                    U_VIS_LongitudeApp = address.Longitude,
                    U_VIS_LatitudeApp = address.Latitude,
                    U_VIS_GeolocationDate = DateTime.Now.ToString("yyyyMMdd"),
                    U_VIS_Photo = S3_Imagen.Upload(s3Client, _bucketName, address.Photo, address.CardCode + "_" + address.AddressCode + ".png", IMAGENES.DIRECCIONES).GetAwaiter().GetResult()
                };

                ClienteDatos clienteDatos = new()
                {
                    EmailAddress = address.email,
                    Cellular = address.mobilephone,
                    Phone1 = address.phone,
#if VISTONY
                    U_SYP_CATCLI = address.lineofbusinessCode,
                    U_EconomyActivity = address.activityeconomiccode
#endif
                };


                ConfirmaDatosClie confirmaDatosClie = new()
                {
                    U_CardCode = address.CardCode,
                    U_email = address.email,
                    U_Fecha = localDate.Date.ToString("yyyyMMdd"),
                    U_Hora = localDate.ToString("HHmm"),
                    U_mobilephone = address.mobilephone,
                    U_phone = address.phone,
                    U_photocomprobation = S3_Imagen.Upload(s3Client, _bucketName,address.photocomprobation, "photocomprobation_" + address.CardCode + "_" + address.AddressCode + ".png", IMAGENES.COMPROBACION_DATOS_CLIENTE).GetAwaiter().GetResult() ,
                    U_verificationcode = address.verificationcode,
                    U_rubric= S3_Imagen.Upload(s3Client, _bucketName, address.rubric, "rubric_" + address.CardCode + "_" + address.AddressCode + ".png", 
                      IMAGENES.COMPROBACION_DATOS_CLIENTE).GetAwaiter().GetResult() 
                };


                string jsonclienteDatos = JsonSerializer.Serialize(clienteDatos);
                ResponseData responseClienteDatos = await serviceLayer.Request("/b1s/v1/BusinessPartners('" + address.CardCode + "')", Method.PATCH, jsonclienteDatos);



                string jsonString = "{\"BPAddresses\":[" + JsonSerializer.Serialize(temp) + "]}";


                ResponseData response = await serviceLayer.Request( "/b1s/v1/BusinessPartners('" + address.CardCode + "')", Method.PATCH, jsonString);
                AddressResponse addressResponse = new();

                string vconfirmaDatosClie = JsonSerializer.Serialize(confirmaDatosClie);
                ResponseData responsevconfirmaDatosClie = await serviceLayer.Request("/b1s/v1/VIS_OCRD_OCDC", Method.POST, vconfirmaDatosClie);
               /// string X = responsevconfirmaDatosClie.Data.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NoContent && 
                    responseClienteDatos.StatusCode == HttpStatusCode.NoContent && 
                    responsevconfirmaDatosClie.StatusCode == HttpStatusCode.Created)
                {
                    addressResponse.AddressCode = int.Parse(address.AddressCode);
                    addressResponse.CardCode = address.CardCode;
                    addressResponse.Message = "Dirección actualizada correctamente";
                    addressResponse.ErrorCode = "N";

                    //RONALD ME DIJO
                    /*if (address.Photo!=null)
                    {
                        S3_Imagen.Upload(s3Client, _bucketName, address.Photo, address.CardCode + "_" + address.AddressCode + ".png", IMAGENES.DIRECCIONES).GetAwaiter().GetResult();
                    }*/
                }
                else
                {
                    var responseBody = await response.Data.Content.ReadAsStringAsync();
                    var responseBody2 = await responseClienteDatos.Data.Content.ReadAsStringAsync();
                    var responseBody3= await responsevconfirmaDatosClie.Data.Content.ReadAsStringAsync();

                    addressResponse.AddressCode = int.Parse(address.AddressCode);
                    addressResponse.CardCode = address.CardCode;
                    addressResponse.Message = "Ocurrio un error al actualizar la dirección:\n " + responseBody.ToString() +"\n Error al actualizar Datos del cliente "+ responseBody2 + " "+ responseBody3;
                    addressResponse.ErrorCode = "Y";
                }

                addressResponseList.Add(addressResponse);
            }

            }
            catch (Exception ex)
            {

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Cliente update DAL Vistony", ex.Message.ToString());
            }
            finally
            {

            }
           
            return new ResponseData()
            {
                Data = new AddressResponseList(){ Addresses= addressResponseList },
                StatusCode = HttpStatusCode.OK
            };
        }
        public string getUrlImagen(string fileNameAndExtension)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
            fileNameAndExtension = @"SalesForceApp/Direcciones/pe/" + fileNameAndExtension;
            string text = S3_Imagen.getUrlImage(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }

        public string getUrlImagenDatosCliente(string fileNameAndExtension)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = _endpoingURL
            };

            IAmazonS3 s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, s3ClientConfig);
            fileNameAndExtension = @"SalesForceApp/ComprobacionDatosCliente/" + fileNameAndExtension;
            string text = S3_Imagen.getUrlImage(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }

        public ListaValidateCredit getValidateCredit(string salesPerson, string day)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ListaValidateCredit clientes = new ListaValidateCredit();
            List<ValidateCredit> listCliente = new List<ValidateCredit>();
            //List<DireccionClienteBO> listDireccion = null;
            ValidateCredit cliente = new ValidateCredit();
            string strSQL = string.Format("CALL {0}.APP_VIST_VALIDACIONCLIENTES_CREDITOS ('{1}','{2}')", DataSource.bd(), salesPerson,day);

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
                        cliente = new ValidateCredit();
                        cliente.SlpCode = reader["Vendedor_ID"].ToString();
                        cliente.SlpName = reader["Vendedor"].ToString();
                        cliente.ZonaID = reader["Zona_ID"].ToString();
                        cliente.ZonaName = reader["Zona"].ToString();
                        cliente.CardCode = reader["Cliente_ID"].ToString();
                        cliente.CardName = reader["Cliente"].ToString();
                        cliente.Address = reader["Domicilio_ID"].ToString();
                        cliente.Street = reader["Direccion"].ToString();
                        cliente.Departamento = reader["Departamento"].ToString();
                        cliente.Provincia = reader["Provincia"].ToString();
                        cliente.Distrito = reader["Distrito"].ToString();
                        cliente.NumAtCard = reader["DocumentoLegal"].ToString();
                        cliente.Asiento = reader["Asiento"].ToString();
                        cliente.LineNum = reader["Line_ID"].ToString();
                        cliente.CtaContable = reader["CtaContable"].ToString();
                        cliente.FechaEmision = reader["FechaEmision"].ToString();
                        cliente.FechaVencimiento = reader["FechaVencimiento"].ToString();
                        cliente.ImporteDoc = Convert.ToDecimal(reader["Importe_Documento"]);
                        cliente.Saldo = Convert.ToDecimal(reader["Saldo"]);
                        cliente.Cobranza = Convert.ToDecimal(reader["Cobranza"]);
                        cliente.NroOP = reader["NroOperacion"].ToString();
                        cliente.FechaPago = reader["FechaPago"].ToString();
                        cliente.TipoDoc = reader["Tipo Documento"].ToString();
                        cliente.DiasMora = Convert.ToInt32(reader["Dias Mora"]);
                        listCliente.Add(cliente);
                    }
                }
                clientes.listValidate = listCliente;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_getValidateCredit - " + ex.Message + " - " + salesPerson + " " + day);
                HanaCommand command = new HanaCommand(strSQL, connection);

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Cliente getValidateCredit DAL Vistony", ex.Message.ToString());
                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return clientes;
        }

        public OfBusiness getLineBusiness(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            LineBusiness objLineBusiness = new LineBusiness();
            List<LineBusiness> lsLineBusiness = new List<LineBusiness>();
            string strSQL = string.Format("CALL {0}.APP_LINE_OF_BUSINESS ('{1}')", DataSource.bd(),imei);
            OfBusiness objOfBusiness = new OfBusiness();
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
                        objLineBusiness = new LineBusiness();
                        objLineBusiness.Code = reader["Code"].ToString();
                        objLineBusiness.Name = reader["Name"].ToString();
                        objLineBusiness.EconomicActivity = reader["EconomicActivity"].ToString();
                        
                        lsLineBusiness.Add(objLineBusiness);
                    }
                }
                objOfBusiness.LineOfBusiness = lsLineBusiness;
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

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "getLineBusiness - " + ex.Message + " - " + imei);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return objOfBusiness;
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
