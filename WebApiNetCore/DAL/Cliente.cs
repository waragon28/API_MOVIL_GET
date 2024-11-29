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
using WebApiNetCore.Utils;
using Sentry;

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
        UsuarioDAL user = new UsuarioDAL();

        public ClienteDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }
        private IMemoryCache _memoryCache;
        public ListaClientes GetCliente (string imei, string cli)
        {
            ListaClientes clientes = new ListaClientes();
            List<ClienteBO> listCliente = new List<ClienteBO>();
            ClienteBO cliente = new ClienteBO();
            string strSQL = string.Format("CALL {0}.APP_CUSTOMER_DETAIL ('{1}','{2}')", DataSource.bd(), imei, cli);

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
                                    DireccionClienteDAL dirDAL = new DireccionClienteDAL();
                                    List<DireccionClienteBO> listDirecciones = new List<DireccionClienteBO>();
                                    listDirecciones = dirDAL.GetDireccion(imei, Other.GetStringValue(reader, "Cliente_ID"));
                                    List<DocumentoDeudaBO> listDocumentoDeuda = new List<DocumentoDeudaBO>();
                                    DocumentoDeudaDAL documentoDeudaDAL = new DocumentoDeudaDAL();
                                    List<VisitBOs> listVisits = new List<VisitBOs>();
                                    VisitDAL visitDAL = new VisitDAL(_memoryCache);
                                    cliente = new ClienteBO();
                                    cliente.CardCode = Other.GetStringValue(reader, "Cliente_ID");
                                    cliente.CardName = Other.GetStringValue(reader, "NombreCliente").ToUpper();
                                    cliente.LicTradNum = Other.GetStringValue(reader, "RucDni").ToUpper();
                                    cliente.Phone = Other.GetStringValue(reader, "TelefonoFijo");
                                    cliente.CellPhone = Other.GetStringValue(reader, "TelefonoMovil");
                                    cliente.ZipCode = Other.GetStringValue(reader, "Ubigeo_ID").ToUpper();
                                    cliente.CreditLimit =Other.GetDoubleValue(reader, "LimiteCredito");
                                    cliente.Balance = Other.GetDoubleValue(reader, "Saldo");
                                    cliente.VisitOrder = Other.GetStringValue(reader, "OrdenVisita");
                                    cliente.Email = Other.GetStringValue(reader, "Correo");
                                    cliente.Category = Other.GetStringValue(reader, "Categoria");
                                    cliente.PymntGroup = Other.GetStringValue(reader, "TerminoPago_ID");
                                    cliente.PayToCode = Other.GetStringValue(reader, "PayToCode");
                                    cliente.PriceList = Other.GetStringValue(reader, "PriceList");
                                    cliente.Currency = Other.GetStringValue(reader, "Currency");
                                    cliente.DueDays = Other.GetIntValue(reader, "DueDays");
                                    cliente.CustomerWhiteList = Other.GetStringValue(reader, "CustomerWhiteList");
                                    cliente.Addresses = dirDAL.GetDireccion(imei, Other.GetStringValue(reader, "Cliente_ID"));
                                    listDocumentoDeuda = documentoDeudaDAL.GetDocumentoDeudaCliente2(imei, cliente.CardCode);
                                    cliente.Invoices = listDocumentoDeuda.Count() != 0 ? listDocumentoDeuda : null;
                                    listCliente.Add(cliente);
                                }
                            }
                        }
                    }
                }
                clientes.Customers = listCliente;
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCliente", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetCliente", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return clientes;
        }

        public Economic_Activity GetActivity_Economic(string imei)
        {
            Economic_Activity ObjEconomic_Activity = new Economic_Activity();
            List<LineEconomic_Activity> ListLineEconomic_Activity = new List<LineEconomic_Activity>();
            LineEconomic_Activity ObjtLineEconomic_Activity = new LineEconomic_Activity();
            string strSQL = string.Format("CALL {0}.APP_ECONOMIC_ACTIVITY ('{1}')", DataSource.bd(), imei);

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
                                    ObjtLineEconomic_Activity = new LineEconomic_Activity();
                                    ObjtLineEconomic_Activity.Name = Other.GetStringValue(reader, "Name");
                                    ObjtLineEconomic_Activity.Code = Other.GetStringValue(reader, "Code");
                                    ListLineEconomic_Activity.Add(ObjtLineEconomic_Activity);
                                }
                            }
                        }
                    }
                }
                            
                ObjEconomic_Activity.Data = ListLineEconomic_Activity;
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetActivity_Economic", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetActivity_Economic", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return ObjEconomic_Activity;
        }

        public ListaClientes2 GetClientes2(string imei)
        {
            ListaClientes2 clientes = new ListaClientes2();
            List<ClienteBO2> listCliente = new List<ClienteBO2>();
            ClienteBO2 cliente = new ClienteBO2();
            string strSQL = string.Format("CALL {0}.APP_CUSTOMERS3 ('{1}')", DataSource.bd(), imei);

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
                                    cliente = new ClienteBO2();
                                    cliente.CardCode = Other.GetStringValue(reader, "CardCode");
                                    cliente.CardName = Other.GetStringValue(reader, "CardName").ToUpper();
                                    cliente.SinTerminoContado = Other.GetStringValue(reader, "SinTerminoContado").ToUpper();
                                    cliente.LicTradNum = Other.GetStringValue(reader, "LicTradNum").ToUpper();
                                    cliente.Phone = Other.GetStringValue(reader, "Phone");
                                    cliente.CellPhone = Other.GetStringValue(reader, "CellPhone");
                                    cliente.ZipCode = Other.GetStringValue(reader, "ZipCode").ToUpper();
                                    cliente.CreditLimit =Other.GetDecimalValue(reader, "CreditLimit");
                                    cliente.Balance = Other.GetDecimalValue(reader, "Balance");
                                    cliente.VisitOrder = Other.GetStringValue(reader, "VisitOrder");
                                    cliente.Email = Other.GetStringValue(reader, "Email");
                                    cliente.Category = Other.GetStringValue(reader, "Category");
                                    cliente.PymntGroup = Other.GetStringValue(reader, "PymntGroup");
                                    cliente.PayToCode = Other.GetStringValue(reader, "PayToCode");
                                    cliente.PriceList = Other.GetStringValue(reader, "PriceList");
                                    cliente.Currency = Other.GetStringValue(reader, "Currency");
                                    cliente.LineOfBusiness = Other.GetStringValue(reader, "LineOfBusiness");
                                    cliente.LastPurchase = Other.GetStringValue(reader, "LastPurchase");
                                    cliente.DueDays = Other.GetIntValue(reader, "DueDays");
                                    cliente.CustomerWhiteList = Other.GetStringValue(reader, "CustomerWhiteList");
                                    cliente.Addresses = Other.GetJsonValue<List<DireccionClienteBO>>(reader, "Addresses");
                                    dynamic invoice = Other.GetJsonValue<List<DocumentoDeudaBO>>(reader, "Invoices");
                                    cliente.Invoices = invoice;
                                    cliente.LastFreePurchase = Other.GetStringValue(reader, "LastFreePurchase");
                                    cliente.LastTradeMarketing = Other.GetStringValue(reader, "LastTradeMarketing");
                                    cliente.EconomyActivity = Other.GetStringValue(reader, "EconomyActivity");
                                    cliente.CustomerRecovery = Other.GetStringValue(reader, "CustomerRecovery");
                                    listCliente.Add(cliente);
                                }
                            }
                        }
                    }
                }

                clientes.Customers = listCliente;
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetClientes2", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetClientes2", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return clientes;
        }
        public ListaClientes2 GetClientesDespacho(string imei, string fecha)
        {
            ListaClientes2 clientes = new ListaClientes2();
            List<ClienteBO2> listCliente = new List<ClienteBO2>();
            ClienteBO2 cliente = new ClienteBO2();
            string strSQL = string.Format("CALL {0}.APP_DESPACHO_CUSTOMER3 ('{1}','{2}')", DataSource.bd(), imei, fecha);

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
                                    cliente = new ClienteBO2();
                                    cliente.CardCode = Other.GetStringValue(reader, "CardCode");
                                    cliente.CardName = Other.GetStringValue(reader, "CardName").ToUpper();
                                    cliente.LicTradNum = Other.GetStringValue(reader, "LicTradNum").ToUpper();
                                    cliente.Phone = Other.GetStringValue(reader, "Phone");
                                    cliente.CellPhone = Other.GetStringValue(reader, "CellPhone");
                                    cliente.ZipCode = Other.GetStringValue(reader, "ZipCode").ToUpper();
                                    cliente.CreditLimit = Other.GetDecimalValue(reader, "CreditLimit");
                                    cliente.Balance = Convert.ToDecimal(reader["Balance"]);
                                    cliente.VisitOrder = Other.GetStringValue(reader, "VisitOrder");
                                    cliente.Email = Other.GetStringValue(reader, "Email");
                                    cliente.Category = Other.GetStringValue(reader, "Category");
                                    cliente.PymntGroup = Other.GetStringValue(reader, "PymntGroup");
                                    cliente.PayToCode = Other.GetStringValue(reader, "PayToCode");
                                    cliente.PriceList = Other.GetStringValue(reader, "PriceList");
                                    cliente.Currency = Other.GetStringValue(reader, "Currency");
                                    cliente.DueDays = Other.GetIntValue(reader, "DueDays");
                                    cliente.Addresses = Other.GetJsonValue<List<DireccionClienteBO>>(reader, "Addresses"); 
                                    dynamic invoice = Other.GetJsonValue<List<DocumentoDeudaBO>>(reader, "Invoices");
                                    cliente.Invoices = invoice;
                                    cliente.CustomerCriticalDistribution = reader["CustomerCriticalDistribution"].ToString();
                                    listCliente.Add(cliente);
                                }
                            }
                        } 
                    } 
                }
                clientes.Customers = listCliente;
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetClientesDespacho", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - GetClientesDespacho", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
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

                LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();

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
                    U_VIS_Photo = ""//S3_Imagen.Upload(s3Client, _bucketName, address.Photo, address.CardCode + "_" + address.AddressCode + ".png", IMAGENES.DIRECCIONES).GetAwaiter().GetResult()
                };

                ClienteDatos clienteDatos = new()
                {
                    EmailAddress = address.email,
                    Cellular = address.mobilephone,
                    Phone1 = address.phone,
                    U_SYP_CATCLI = address.lineofbusinessCode,
                    U_EconomyActivity = address.activityeconomiccode
                };


                ConfirmaDatosClie confirmaDatosClie = new()
                {
                    U_CardCode = address.CardCode,
                    U_email = address.email,
                    U_Fecha = localDate.Date.ToString("yyyyMMdd"),
                    U_Hora = localDate.ToString("HHmm"),
                    U_mobilephone = address.mobilephone,
                    U_phone = address.phone,
                    U_photocomprobation ="",// S3_Imagen.Upload(s3Client, _bucketName,address.photocomprobation, "photocomprobation_" + address.CardCode + "_" + address.AddressCode + ".png", IMAGENES.COMPROBACION_DATOS_CLIENTE).GetAwaiter().GetResult() ,
                    U_verificationcode = address.verificationcode,
                    U_rubric= ""//S3_Imagen.Upload(s3Client, _bucketName, address.rubric, "rubric_" + address.CardCode + "_" + address.AddressCode + ".png",  IMAGENES.COMPROBACION_DATOS_CLIENTE).GetAwaiter().GetResult() 
                };


                string jsonclienteDatos = JsonSerializer.Serialize(clienteDatos);


                ResponseData responseClienteDatos = await serviceLayer.Request("/b1s/v1/BusinessPartners('" + address.CardCode + "')", Method.PATCH, jsonclienteDatos, sl.token);



                string jsonString = "{\"BPAddresses\":[" + JsonSerializer.Serialize(temp) + "]}";


                ResponseData response = await serviceLayer.Request( "/b1s/v1/BusinessPartners('" + address.CardCode + "')", Method.PATCH, jsonString, sl.token);
                    AddressResponse addressResponse = new();

                string vconfirmaDatosClie = JsonSerializer.Serialize(confirmaDatosClie);
                ResponseData responsevconfirmaDatosClie = await serviceLayer.Request("/b1s/v1/VIS_OCRD_OCDC", Method.POST, vconfirmaDatosClie, sl.token);

                    if (response.StatusCode == HttpStatusCode.NoContent && 
                    responseClienteDatos.StatusCode == HttpStatusCode.NoContent && 
                    responsevconfirmaDatosClie.StatusCode == HttpStatusCode.Created)
                {
                    addressResponse.AddressCode = int.Parse(address.AddressCode);
                    addressResponse.CardCode = address.CardCode;
                    addressResponse.Message = "Dirección actualizada correctamente";
                    addressResponse.ErrorCode = "N";
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
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - update", addressesList + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - update", addressesList + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            finally
            {
                user.LogoutServiceLayer().GetAwaiter().GetResult();
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
            string text = "";// S3_Imagen.getUrlImage(s3Client, _bucketName, fileNameAndExtension);
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
            string text = "";//S3_Imagen.getUrlImage(s3Client, _bucketName, fileNameAndExtension);
            return text;
        }

        public ListaValidateCredit getValidateCredit(string salesPerson, string day)
        {
            ListaValidateCredit clientes = new ListaValidateCredit();
            List<ValidateCredit> listCliente = new List<ValidateCredit>();
            ValidateCredit cliente = new ValidateCredit();
            string strSQL = string.Format("CALL {0}.APP_VIST_VALIDACIONCLIENTES_CREDITOS ('{1}','{2}')", DataSource.bd(), salesPerson,day);

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
                                    cliente = new ValidateCredit();
                                    cliente.SlpCode =  Other.GetStringValue(reader,"Vendedor_ID");
                                    cliente.SlpName =  Other.GetStringValue(reader,"Vendedor");
                                    cliente.ZonaID =  Other.GetStringValue(reader,"Zona_ID");
                                    cliente.ZonaName =  Other.GetStringValue(reader,"Zona");
                                    cliente.CardCode =  Other.GetStringValue(reader,"Cliente_ID");
                                    cliente.CardName =  Other.GetStringValue(reader,"Cliente");
                                    cliente.Address =  Other.GetStringValue(reader,"Domicilio_ID");
                                    cliente.Street =  Other.GetStringValue(reader,"Direccion");
                                    cliente.Departamento =  Other.GetStringValue(reader,"Departamento");
                                    cliente.Provincia =  Other.GetStringValue(reader,"Provincia");
                                    cliente.Distrito =  Other.GetStringValue(reader,"Distrito");
                                    cliente.NumAtCard =  Other.GetStringValue(reader,"DocumentoLegal");
                                    cliente.Asiento =  Other.GetStringValue(reader,"Asiento");
                                    cliente.LineNum =  Other.GetStringValue(reader,"Line_ID");
                                    cliente.CtaContable =  Other.GetStringValue(reader,"CtaContable");
                                    cliente.FechaEmision =  Other.GetStringValue(reader,"FechaEmision");
                                    cliente.FechaVencimiento =  Other.GetStringValue(reader,"FechaVencimiento");
                                    cliente.ImporteDoc = Other.GetDecimalValue(reader,"Importe_Documento");
                                    cliente.Saldo = Other.GetDecimalValue(reader,"Saldo");
                                    cliente.Cobranza = Other.GetDecimalValue(reader,"Cobranza");
                                    cliente.NroOP =  Other.GetStringValue(reader,"NroOperacion");
                                    cliente.FechaPago =  Other.GetStringValue(reader,"FechaPago");
                                    cliente.TipoDoc =  Other.GetStringValue(reader,"Tipo Documento");
                                    cliente.DiasMora =Other.GetIntValue(reader,"Dias Mora");
                                    listCliente.Add(cliente);
                                }
                            }
                        } 
                    } 
                }
                clientes.listValidate = listCliente;
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - getValidateCredit", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - getValidateCredit", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return clientes;
        }

        public OfBusiness getLineBusiness(string imei)
        {
            LineBusiness objLineBusiness = new LineBusiness();
            List<LineBusiness> lsLineBusiness = new List<LineBusiness>();
            string strSQL = string.Format("CALL {0}.APP_LINE_OF_BUSINESS ('{1}')", DataSource.bd(),imei);
            OfBusiness objOfBusiness = new OfBusiness();
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
                                    objLineBusiness = new LineBusiness();
                                    objLineBusiness.Code = Other.GetStringValue(reader, "Code");
                                    objLineBusiness.Name = Other.GetStringValue(reader, "Name");
                                    objLineBusiness.EconomicActivity = Other.GetStringValue(reader, "EconomicActivity");

                                    lsLineBusiness.Add(objLineBusiness);
                                }
                            }
                        }
                    }
                }

                objOfBusiness.LineOfBusiness = lsLineBusiness;
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - getLineBusiness", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - getLineBusiness", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
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
