
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
using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using Sentry;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.IO;

using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using Azure.Core;
using System.IdentityModel.Tokens.Jwt;
using ZXing.Aztec.Internal;
using WebApiNetCore.DAL;


//using System.Configuration;


namespace SAP_Core.DAL
{
   public  class ApprovalDAL : Connection,IDisposable 
    {
        private ServiceLayer serviceLayer;
        CorreoAlert correoAlert = new CorreoAlert();

        public ApprovalDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }

        public ListApprovalBo Get_Documents (string user,string status)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<ApprovalBo> listUsuario = new List<ApprovalBo>();
            

            string strSQL  = string.Format("CALL {0}.SP_VIS_APPROV_LIST_WDD('1','','','','','{1}','{2}')", DataSource.bd(), user, status);
           
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
                        ApprovalBo approval = new();
                        approval.ID = reader["ID"].ToString();
                        approval.CarCode = reader["Código SN"].ToString();
                        approval.CardName= reader["Nombre SN"].ToString().ToUpper();
                        approval.DocNumBorrador= reader["Pedido Borrador"].ToString().ToUpper();
                        approval.TypeDocument = reader["TipoDocumento"].ToString().ToUpper();
                        approval.DocDate = reader["Fecha del Documento"].ToString().ToUpper();
                        approval.DocTime = reader["Hora"].ToString().ToUpper();
                        approval.DocTotal = reader["Total del Documento"].ToString().ToUpper();
                        approval.ConditionalPayment = reader["Condición de Pago"].ToString().ToUpper();
                        approval.SalesPerson = reader["Empleado de Ventas"].ToString().ToUpper();
                        approval.MargenDocumento = reader["MargenDocumento"].ToString().ToUpper();
                        approval.DocEntry = reader["DocEntry"].ToString().ToUpper();

                        listUsuario.Add(approval);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_Documents DAL Vistony", ex.Message.ToString());

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Documents - " + ex.Message + " "+ user +" "+status);
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
            return new ListApprovalBo() { Data= listUsuario };
        }

        public ListDocumentosBO Documentos(string id, string User,string status)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<DocumentosBO> listUsuario = new List<DocumentosBO>();
            ListDocumentosBO listDocumentosBO = new ListDocumentosBO();

            string strSQL = string.Format("CALL {0}.APP_APPROV_LIST_WDD('{1}','{2}','{3}')", DataSource.bd(), id,User ,status);

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
                        DocumentosBO approval = new();
                        approval.ID = reader["ID"].ToString();
                        approval.CarCode = reader["Código SN"].ToString();
                        approval.CardName = reader["Nombre SN"].ToString().ToUpper();
                        approval.DocDate = reader["Fecha del Documento"].ToString().ToUpper();
                        approval.DocTime = reader["Hora"].ToString().ToUpper();
                        approval.DocTotal = Convert.ToDouble(reader["Total del Documento"].ToString());
                        approval.TypeDocument = reader["TipoDocumento"].ToString().ToUpper();
                        approval.DocNumBorrador = reader["Pedido Borrador"].ToString().ToUpper();

                        approval.ConditionalPayment = reader["Condición de Pago"].ToString().ToUpper();
                        approval.SalesPerson = reader["Empleado de Ventas"].ToString().ToUpper();
                        approval.MargenDocumento = Convert.ToDouble(reader["MargenDocumento"].ToString());
                        approval.DocEntry = reader["DocEntry"].ToString().ToUpper();
                        approval.CantidadAnexo =Convert.ToInt32(reader["CantidadAnexo"].ToString());
                        approval.MargenGanancia = 0;
                        listUsuario.Add(approval);
                    }
                }
                listDocumentosBO.Data = listUsuario;
                connection.Close();
            }
            catch (Exception ex)
            {

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Documentos DAL Vistony", ex.Message.ToString());
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Documents - " + ex.Message + " " + " " + status);
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
            return listDocumentosBO;
        }

        public ListDeudaBo Get_Deuda (string cardCode)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            List<DeudaBo> listUsuario = new List<DeudaBo>();

            string strSQL  = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DATOS_DEUDA','')", DataSource.bd(), cardCode);
           
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

                        DeudaBo deuda = new();
                        deuda.Deuda_Corriente = reader["Deuda_Corriente"].ToString();
                        deuda.DIAS_1_8 = reader["1-8_DIAS"].ToString().ToUpper();
                        deuda.DIAS_9_15= reader["9-15_DIAS"].ToString().ToUpper();
                        deuda.DIAS_16_30 = reader["16-30_DIAS"].ToString().ToUpper();
                        deuda.DIAS_31_60 = reader["31-60_DIAS"].ToString().ToUpper();
                        deuda.DIAS_61_90 = reader["61-90_DIAS"].ToString().ToUpper();
                        deuda.DIAS_91_120 = reader["91-120_DIAS"].ToString().ToUpper();
                        deuda.MAYOR_120_DIAS = reader["MAYOR_120_DIAS"].ToString().ToUpper();
                        deuda.Deuda = reader["Deuda"].ToString().ToUpper();
                        deuda.LineaCredito = reader["LineaCredito"].ToString().ToUpper();
                        deuda.LineaComprometida = reader["LineaComprometida"].ToString().ToUpper();
                        deuda.SaldoLinea = reader["SaldoLinea"].ToString().ToUpper();
                        deuda.Deuda_Vencida = reader["Deuda_Vencida"].ToString().ToUpper();

                        listUsuario.Add(deuda);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_Deuda DAL Vistony", ex.Message.ToString());

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Deuda - " + ex.Message + " " + cardCode);
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
            return new ListDeudaBo() { Data= listUsuario };
        }
        public ListLineaBo Get_Linea(string cardCode)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<LineaBo> listUsuario = new List<LineaBo>();

            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DEUDA_CORPORATIVA','')", DataSource.bd(), cardCode);


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
                        LineaBo linea = new();
                        linea.Deuda = reader["Deuda"].ToString();
                        linea.Entregas = reader["Entregas"].ToString().ToUpper();
                        linea.Pedido = reader["Pedido"].ToString().ToUpper();
                        linea.Oportunidades = reader["Oportunidades"].ToString().ToUpper();
                        linea.Cheques = reader["Cheques"].ToString().ToUpper();
                        linea.UltPago = reader["UltPago"].ToString().ToUpper();
                        linea.UltPagDoc = reader["UltPagDoc"].ToString().ToUpper();
                        linea.UltFact = reader["UltFact"].ToString().ToUpper();
                        linea.UltFacDoc = reader["UltFacDoc"].ToString().ToUpper();
                        linea.SumaProtesto = reader["SumaProtesto"].ToString().ToUpper();
                        linea.LineaCredito = reader["LineaCredito"].ToString().ToUpper();
                        linea.LineaComprometida = reader["LineaComprometida"].ToString().ToUpper();
                        linea.SaldoLinea = reader["SaldoLinea"].ToString().ToUpper();

                        listUsuario.Add(linea);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {

                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_Linea DAL Vistony", ex.Message.ToString());

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Deuda - " + ex.Message + " " + cardCode);
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
            return new ListLineaBo() { Data = listUsuario };
        }
        public ListClienteBo Get_Cliente(string cardCode,string user)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<ClienteBo> listUsuario = new List<ClienteBo>();

            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DATOS_CLIENTE','')", DataSource.bd(), cardCode);

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

                        ClienteBo cliente = new();
                        cliente.PEDocumentID = reader["PEDocumentID"].ToString();
                        cliente.Name = reader["Name"].ToString().ToUpper();
                        cliente.RUC = reader["RUC"].ToString().ToUpper();
                        cliente.direccion = reader["direccion"].ToString().ToUpper();
                        cliente.ciudad = reader["ciudad"].ToString().ToUpper();
                        cliente.CodVendedor = reader["CodVendedor"].ToString().ToUpper();
                        cliente.NomVendedor = reader["NomVendedor"].ToString().ToUpper();
                        cliente.Supervisor = reader["Supervisor"].ToString().ToUpper();
                        cliente.AnalistaCreditos = reader["AnalistaCreditos"].ToString().ToUpper();
                        cliente.SectoristaVenta = reader["SectoristaVenta"].ToString().ToUpper();
                        cliente.nomudn = reader["nomudn"].ToString().ToUpper();
                        cliente.Prioridad = reader["Prioridad"].ToString().ToUpper();
                        cliente.Castigado = reader["Castigado"].ToString().ToUpper();
                        ListPedidoBo listPedido= Get_Pedidos(reader["PEDocumentID"].ToString(),user);

                        //cliente.Pedidos = (listPedido.Data.Count == 0 ) ? null : listPedido.Data;
                        cliente.Pedidos = listPedido.Data;

                        listUsuario.Add(cliente);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_Cliente DAL Vistony", ex.Message.ToString());

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Cliente - " + ex.Message + " " + cardCode + " " + user);
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
            return new ListClienteBo() { Data = listUsuario };
        }
        private ListPedidoBo Get_Pedidos(string cardCode,string user)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<PedidoBo> listUsuario = new List<PedidoBo>();

            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DATOS_PEDIDO','{2}')", DataSource.bd(), cardCode,user);

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
                        PedidoBo pedido = new();
                        pedido.EntryDraft = reader["EntryDraft"].ToString();
                        pedido.Pedido = reader["Pedido"].ToString().ToUpper();
                        pedido.DocDate = reader["Fecha de Documento"].ToString().ToUpper();
                        pedido.DocTime = reader["Hora"].ToString().ToUpper();
                        pedido.MargenGanancia = reader["MargenGanancia"].ToString().ToUpper();
                        pedido.PaymentTerminal = reader["Condición de Pago"].ToString().ToUpper();
                        pedido.Moneda = reader["Moneda"].ToString().ToUpper();
                        pedido.DocTotal = reader["Total de Documento"].ToString().ToUpper();
                        pedido.SalesPerson = reader["Empleado de Ventas"].ToString().ToUpper();
                        pedido.Coment = reader["Comentarios"].ToString().ToUpper();
                        pedido.DocType = reader["TipoDocumento"].ToString().ToUpper();
                       

                        listUsuario.Add(pedido);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_Pedidos DAL Vistony", ex.Message.ToString());

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Cliente - " + ex.Message + " " + cardCode + " " + user);
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
            return new ListPedidoBo() { Data = listUsuario };
        } 
        public ListPedidoDetalleBo Get_PedidosDetalle(string docEntry,string Tipo)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<PedidoDetalleBo> listUsuario = new List<PedidoDetalleBo>();
            string strSQL = string.Empty;

                strSQL= string.Format("CALL {0}.P_VIS_APPROV_DETAIL_DRAFT('{1}','{2}')", DataSource.bd(), docEntry,Tipo);
           
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
                        PedidoDetalleBo pedido = new();
                        pedido.numArticle = reader["Número de Artículo"].ToString();
                        pedido.ItemName = reader["Descripción de Artículo"].ToString().ToUpper();
                        pedido.Quantity = reader["Cantidad"].ToString().ToUpper();
                        pedido.OnHand = reader["Cantidad En Almacén"].ToString().ToUpper();
                        pedido.PriceUnit = reader["Precio Unit"].ToString().ToUpper();
                        pedido.DescontPercent = reader["% Desc"].ToString().ToUpper();
                        pedido.LineTotal = reader["Total Linea"].ToString().ToUpper();
                        pedido.issetImpuesto = reader["Sólo Impuesto"].ToString().ToUpper();
                        pedido.CodImpuesto = reader["Cod Impuesto"].ToString().ToUpper();
                        pedido.Warehouse = reader["Almacén"].ToString().ToUpper();
                        pedido.MarginGain = reader["Margen Ganancia"].ToString().ToUpper() + "%";
                        if (Tipo!="")
                        {
                            pedido.USUARIO_ID = reader["USUARIO_ID"].ToString();
                            pedido.USUARIO = reader["USUARIO"].ToString();
                        }
                        pedido.PriceHistory = Convert.ToDouble(reader["PriceHistory"].ToString());
                        pedido.PriceReference = Convert.ToDouble(reader["PriceReference"].ToString());
                        pedido.PorcUltComExc = Convert.ToDouble(reader["PorcUltComExc"].ToString());
                        listUsuario.Add(pedido);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_PedidosDetalle DAL Vistony", ex.Message.ToString());
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Cliente - " + ex.Message + " " + docEntry);
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
            return new ListPedidoDetalleBo() { Data = listUsuario };
        }
        public ListAprovacionBo Get_Rules(string docEntry,string Tipo)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<AprobacionBo> listUsuario = new List<AprobacionBo>();

            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_DETAIL_RULE('{1}','{2}')", DataSource.bd(), docEntry, Tipo);

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
                        AprobacionBo aprobacion = new();
                        aprobacion.Codigo = reader["Código"].ToString();
                        aprobacion.CodRegla = reader["CodRegla"].ToString();
                        aprobacion.Etapa = reader["Etapa"].ToString();
                        aprobacion.Modelo = reader["Modelo"].ToString();
                        aprobacion.Autorizador = reader["Autorizador"].ToString();
                        aprobacion.Decisión = reader["Decisión"].ToString();
                        aprobacion.Comment = reader["Comentario de Aprobación"].ToString();
                        aprobacion.DecBKP = reader["DecBKP"].ToString();

                        listUsuario.Add(aprobacion);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_Rules DAL Vistony", ex.Message.ToString());

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_Get_Cliente - " + ex.Message + " " + docEntry);
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
            return new ListAprovacionBo() { Data = listUsuario };
        }



        public LstAnexo Get_Anexos(string DocEntry)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<Anexo> listAnexo = new List<Anexo>();
            LstAnexo lstAnexo = new LstAnexo();
            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_ANEXO('{1}')", DataSource.bd(), DocEntry);

           
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
                        Anexo PriceHistoy = new();
                        PriceHistoy.Documento = reader["Archivo"].ToString();
                        PriceHistoy.Date = reader["Date"].ToString();
                        PriceHistoy.Link = reader["subPath"].ToString();

                        listAnexo.Add(PriceHistoy);
                    }
                }
                lstAnexo.Data = listAnexo;

                connection.Close();
            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_Anexos DAL Vistony", ex.Message.ToString());

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Get_Anexos - " + ex.Message + " " + DocEntry);
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
            return lstAnexo;
        }

        /*
        public static async Task DownloadFileFromOneDriveAsync(string tenantId, string clientId, string clientSecret, string accessToken, string filePathOnOneDrive, string localFilePath)
        {

            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();

            var scopes = new string[] { "https://graph.microsoft.com/.default" };

            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            accessToken = result.AccessToken;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Reemplaza '/me' con 'users/{userId}' si conoces el ID del usuario propietario del archivo
                string userId = "f9faf62a-a2b3-40a1-90f7-e732063841fb"; //"user-id";  // Reemplaza esto con el ID real del usuario
                string downloadUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/drive/root:/{filePathOnOneDrive}:/content";

                HttpResponseMessage response = await httpClient.GetAsync(downloadUrl);

                if (response.IsSuccessStatusCode)
                {
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(localFilePath, fileBytes);
                    Console.WriteLine($"Archivo descargado correctamente en: {localFilePath}");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al descargar el archivo. Código de estado: {response.StatusCode}, Detalles: {content}");
                }
            }
        }

        public static async Task<string> GetAccessTokenWithClientCredentialsAsync(string tenantId, string clientId, string clientSecret,string filePathOnOneDrive,string localFilePath)
        {
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                 .WithClientSecret(clientSecret)
                 .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                 .Build();

            var scopes = new string[] { "https://graph.microsoft.com/.default" }; // Asegúrate de que estos permisos estén configurados en Azure AD

            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

          //  return result.AccessToken;

           string accessToken = result.AccessToken;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Reemplaza '/me' con 'users/{userId}' si conoces el ID del usuario propietario del archivo
                string userId = "f9faf62a-a2b3-40a1-90f7-e732063841fb"; //"user-id";  // Reemplaza esto con el ID real del usuario
                string downloadUrl = $"https://graph.microsoft.com/v1.0/users/{userId}";///drive/root:/{filePathOnOneDrive}:/content";

                HttpResponseMessage response = await httpClient.GetAsync(downloadUrl);

                if (response.IsSuccessStatusCode)
                {
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(localFilePath, fileBytes);
                    Console.WriteLine($"Archivo descargado correctamente en: {localFilePath}");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al descargar el archivo. Código de estado: {response.StatusCode}, Detalles: {content}");
                }
            }

            return accessToken;
        }


        public async Task GetTokenAsync(string tenant, string clientId, string clientSecret, string username, string password)
        {
            HttpResponseMessage resp;
            using (var httpClient = new HttpClient())
            {
                // Configura el cliente HTTP para aceptar el tipo de contenido application/x-www-form-urlencoded
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // Crea el mensaje de solicitud para obtener el token
                var req = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{tenant}/oauth2/token/");

                // Establece el contenido de la solicitud con los parámetros necesarios para el flujo de credenciales de contraseña
                req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", clientId},
                    {"client_secret", clientSecret},
                    {"resource", "https://graph.microsoft.com"},
                    {"username", username},
                    {"password", password}
                });

                // Envía la solicitud y espera la respuesta
                resp = await httpClient.SendAsync(req);

                // Lee el contenido de la respuesta como una cadena
                string content = await resp.Content.ReadAsStringAsync();

                // Deserializa el contenido JSON para obtener el token de acceso
                var jsonObj = JsonConvert.DeserializeObject<dynamic>(content);

                // Accede a la propiedad "access_token" del objeto JSON
                string token = jsonObj["access_token"];

              

                // Imprime el token en la consola (puedes reemplazar esto con cualquier otra acción que necesites)
                Console.WriteLine(token);
            }
        }

        */

        public List<PriceHistoy> Get_PriceHistory(string ItemCode)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            List<PriceHistoy> listPriceHistoy = new List<PriceHistoy>();
            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_PRICE_HISTORY('{1}')", DataSource.bd(), ItemCode);

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
                        PriceHistoy PriceHistoy = new();
                        PriceHistoy.CardName = reader["CardName"].ToString();
                        PriceHistoy.DocDate = reader["DocDate"].ToString().ToUpper();
                        PriceHistoy.Price = Convert.ToDouble(reader["Price"].ToString());

                        listPriceHistoy.Add(PriceHistoy);
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval Get_PriceHistory DAL Vistony", ex.Message.ToString());

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Get_PriceHistory - " + ex.Message + " " + ItemCode);
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
            return listPriceHistoy;
        }

        public async Task<ResponseData> decicion(string json,string docEntry,string sessionId, string Tipo)
        {

            ResponseData response = new ResponseData();

            HanaConnection connection = GetConnection();
            if (Tipo=="")
            {
                 response = await serviceLayer.Request("/b1s/v1/ApprovalRequests(" + docEntry + ")", Method.PATCH, json, sessionId);
                //HanaConnection connection = GetConnection();
                HanaDataReader reader;

                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        response.Data = "Actualización guardada exitosamente";
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var responseBody = await response.Data.Content.ReadAsStringAsync();
                        response.Data = responseBody;
                    }
                    else
                    {
                        var responseBody = await response.Data.Content.ReadAsStringAsync();
                        response.Data = responseBody;
                        response.StatusCode = HttpStatusCode.FailedDependency;
                    }
                }
                catch (Exception ex)
                {

                    correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval decicion DAL Vistony", ex.Message.ToString());

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    string strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_decicion - " + ex.Message);
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


               
            }
            else
            {
                try
                {
                    // Parsear el JSON
                    var jsonObject = JObject.Parse(json);

                    // Acceder al valor de "Status"
                    string status = jsonObject["ApprovalRequestDecisions"][0]["Status"].ToString();
                    string staRemarkstus = jsonObject["ApprovalRequestDecisions"][0]["Remarks"].ToString();
                    string A = string.Empty;
                    if (status == "ardPending")
                    {
                        A = "P";
                    }
                    else if (status == "ardNotApproved")
                    {
                        A = "N";
                    }
                    else
                    {   
                        A = "Y";
                    }
                    connection.Open();
                    string strSQL = string.Format("CALL {0}.P_VIS_APPROV_DECISION('{1}','{2}','{3}','{4}')", DataSource.bd(), docEntry, Tipo, A, staRemarkstus);
                    HanaCommand command = new HanaCommand(strSQL, connection);
                    HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    connection.Close();
                    response = new ResponseData();
                    response.Data = "OK";
                    response.StatusCode = HttpStatusCode.OK;

                }
                catch (Exception ex)
                {
                    correoAlert.EnviarCorreoOffice365("Error API Ventas " + "Approval decicion DAL Vistony 2", ex.Message.ToString());

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    string strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_decicion - " + ex.Message);
                    HanaCommand command = new HanaCommand(strSQL, connection);

                    HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    connection.Close();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return response;

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
        ~ApprovalDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion


    }// fin de la clase

}
