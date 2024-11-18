
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
using Microsoft.Graph.Models;
using WebApiNetCore.Utils;


//using System.Configuration;


namespace SAP_Core.DAL
{
   public  class ApprovalDAL : Connection,IDisposable 
    {
        private ServiceLayer serviceLayer;
        CorreoAlert correoAlert = new CorreoAlert();
        UsuarioDAL user = new UsuarioDAL();

        public ApprovalDAL(IMemoryCache _memoryCache)
        {
            serviceLayer = new(_memoryCache);
        }

        public ListApprovalBo Get_Documents (string user,string status)
        {
            List<ApprovalBo> listUsuario = new List<ApprovalBo>();
            string strSQL  = string.Format("CALL {0}.SP_VIS_APPROV_LIST_WDD('1','','','','','{1}','{2}')", DataSource.bd(), user, status);

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
                                    ApprovalBo approval = new();
                                    approval.ID = Other.GetStringValue(reader, "ID");
                                    approval.CarCode = Other.GetStringValue(reader, "Código SN");
                                    approval.CardName = Other.GetStringValue(reader, "Nombre SN");
                                    approval.DocNumBorrador = Other.GetStringValue(reader, "Pedido Borrador");
                                    approval.TypeDocument = Other.GetStringValue(reader, "TipoDocumento");
                                    approval.DocDate = Other.GetStringValue(reader, "Fecha del Documento");
                                    approval.DocTime = Other.GetStringValue(reader, "Hora");
                                    approval.DocTotal = Other.GetStringValue(reader, "Total del Documento");
                                    approval.ConditionalPayment = Other.GetStringValue(reader, "Condición de Pago");
                                    approval.SalesPerson = Other.GetStringValue(reader, "Empleado de Ventas");
                                    approval.MargenDocumento = Other.GetStringValue(reader, "MargenDocumento");
                                    approval.DocEntry = Other.GetStringValue(reader, "DocEntry");

                                    listUsuario.Add(approval);
                                }
                            }
                        }
                    }

                }

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Documents", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Documents", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
           
            return new ListApprovalBo() { Data= listUsuario };
        }

        public ListDocumentosBO Documentos(string id, string User,string status)
        {
            List<DocumentosBO> listUsuario = new List<DocumentosBO>();
            ListDocumentosBO listDocumentosBO = new ListDocumentosBO();

            string strSQL = string.Format("CALL {0}.APP_APPROV_LIST_WDD('{1}','{2}','{3}')", DataSource.bd(), id,User ,status);

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
                                    DocumentosBO approval = new();
                                    approval.ID = Other.GetStringValue(reader, "ID");
                                    approval.CarCode = Other.GetStringValue(reader, "Código SN");
                                    approval.CardName = Other.GetStringValue(reader, "Nombre SN").ToUpper();
                                    approval.DocDate = Other.GetStringValue(reader, "Fecha del Documento");
                                    approval.DocTime = Other.GetStringValue(reader, "Hora");
                                    approval.DocTotal = Other.GetDoubleValue(reader, "Total del Documento");
                                    approval.TypeDocument = Other.GetStringValue(reader, "TipoDocumento");
                                    approval.DocNumBorrador = Other.GetStringValue(reader, "Pedido Borrador");
                                    approval.ConditionalPayment = Other.GetStringValue(reader, "Condición de Pago").ToUpper();
                                    approval.SalesPerson = Other.GetStringValue(reader, "Empleado de Ventas");
                                    approval.MargenDocumento = Other.GetDoubleValue(reader, "MargenDocumento");
                                    approval.DocEntry = Other.GetStringValue(reader, "DocEntry");
                                    approval.CantidadAnexo =Other.GetIntValue(reader, "CantidadAnexo");
                                    approval.MargenGanancia = Other.GetIntValue(reader, "MargenDocumento");
                                    listUsuario.Add(approval);
                                }
                            }
                            listDocumentosBO.Data = listUsuario;
                        }


                    }
                }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Documentos", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Documentos", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return listDocumentosBO;
        }

        public ListDeudaBo Get_Deuda (string cardCode)
        {
            List<DeudaBo> listUsuario = new List<DeudaBo>();
            string strSQL  = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DATOS_DEUDA','')", DataSource.bd(), cardCode);

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
                                    DeudaBo deuda = new();
                                    deuda.Deuda_Corriente = Other.GetStringValue(reader, "Deuda_Corriente");
                                    deuda.DIAS_1_8 = Other.GetStringValue(reader, "1-8_DIAS");
                                    deuda.DIAS_9_15 = Other.GetStringValue(reader, "9-15_DIAS");
                                    deuda.DIAS_16_30 = Other.GetStringValue(reader, "16-30_DIAS");
                                    deuda.DIAS_31_60 = Other.GetStringValue(reader, "31-60_DIAS");
                                    deuda.DIAS_61_90 = Other.GetStringValue(reader, "61-90_DIAS");
                                    deuda.DIAS_91_120 = Other.GetStringValue(reader, "91-120_DIAS");
                                    deuda.MAYOR_120_DIAS = Other.GetStringValue(reader, "MAYOR_120_DIAS");
                                    deuda.Deuda = Other.GetStringValue(reader, "Deuda");
                                    deuda.LineaCredito = Other.GetStringValue(reader, "LineaCredito");
                                    deuda.LineaComprometida = Other.GetStringValue(reader, "LineaComprometida");
                                    deuda.SaldoLinea = Other.GetStringValue(reader, "SaldoLinea");
                                    deuda.Deuda_Vencida = Other.GetStringValue(reader, "Deuda_Vencida");

                                    listUsuario.Add(deuda);
                                }
                            }
                        }

                    }
                }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Deuda", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Deuda", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return new ListDeudaBo() { Data= listUsuario };
        }

        public ListLineaBo Get_Linea(string cardCode)
        {
            List<LineaBo> listUsuario = new List<LineaBo>();

            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DEUDA_CORPORATIVA','')", DataSource.bd(), cardCode);

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
                                    LineaBo linea = new();
                                    linea.Deuda = Other.GetStringValue(reader, "Deuda");
                                    linea.Entregas = Other.GetStringValue(reader, "Entregas");
                                    linea.Pedido =  Other.GetStringValue(reader, "Pedido");
                                    linea.Oportunidades =  Other.GetStringValue(reader, "Oportunidades");
                                    linea.Cheques =  Other.GetStringValue(reader, "Cheques");
                                    linea.UltPago =  Other.GetStringValue(reader, "UltPago");
                                    linea.UltPagDoc =  Other.GetStringValue(reader, "UltPagDoc");
                                    linea.UltFact =  Other.GetStringValue(reader, "UltFact");
                                    linea.UltFacDoc =  Other.GetStringValue(reader, "UltFacDoc");
                                    linea.SumaProtesto =  Other.GetStringValue(reader, "SumaProtesto");
                                    linea.LineaCredito =  Other.GetStringValue(reader, "LineaCredito");
                                    linea.LineaComprometida =  Other.GetStringValue(reader, "LineaComprometida");
                                    linea.SaldoLinea =  Other.GetStringValue(reader, "SaldoLinea");

                                    listUsuario.Add(linea);
                                }
                            }
                        }
                    }
                }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Linea", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Linea", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return new ListLineaBo() { Data = listUsuario };
        }

        public ListClienteBo Get_Cliente(string cardCode,string user)
        {
            List<ClienteBo> listUsuario = new List<ClienteBo>();
            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DATOS_CLIENTE','')", DataSource.bd(), cardCode);
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
                                    ClienteBo cliente = new();
                                    cliente.PEDocumentID = Other.GetStringValue(reader, "PEDocumentID");
                                    cliente.Name = Other.GetStringValue(reader, "Name").ToUpper();
                                    cliente.RUC = Other.GetStringValue(reader, "RUC");
                                    cliente.direccion = Other.GetStringValue(reader, "direccion").ToUpper();
                                    cliente.ciudad = Other.GetStringValue(reader, "ciudad").ToUpper();
                                    cliente.CodVendedor = Other.GetStringValue(reader, "CodVendedor");
                                    cliente.NomVendedor = Other.GetStringValue(reader, "NomVendedor").ToUpper();
                                    cliente.Supervisor = Other.GetStringValue(reader, "Supervisor").ToUpper();
                                    cliente.AnalistaCreditos = Other.GetStringValue(reader, "AnalistaCreditos").ToUpper();
                                    cliente.SectoristaVenta = Other.GetStringValue(reader, "SectoristaVenta").ToUpper();
                                    cliente.nomudn = Other.GetStringValue(reader, "nomudn");
                                    cliente.Prioridad = Other.GetStringValue(reader, "Prioridad").ToUpper();
                                    cliente.Castigado = Other.GetStringValue(reader, "Castigado").ToUpper();
                                    ListPedidoBo listPedido = Get_Pedidos(Other.GetStringValue(reader, "PEDocumentID"), user);

                                    cliente.Pedidos = listPedido.Data;

                                    listUsuario.Add(cliente);
                                }
                            }
                        }
                    }
                }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Deuda", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Deuda", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return new ListClienteBo() { Data = listUsuario };
        }

        private ListPedidoBo Get_Pedidos(string cardCode,string user)
        {
            List<PedidoBo> listUsuario = new List<PedidoBo>();

            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_CUSTOMERCREDIT('{1}','','DATOS_PEDIDO','{2}')", DataSource.bd(), cardCode,user);

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
                                    PedidoBo pedido = new();
                                    pedido.EntryDraft = Other.GetStringValue(reader, "EntryDraft");
                                    pedido.Pedido = Other.GetStringValue(reader, "Pedido");
                                    pedido.DocDate = Other.GetStringValue(reader, "Fecha de Documento");
                                    pedido.DocTime = Other.GetStringValue(reader, "Hora");
                                    pedido.MargenGanancia = Other.GetStringValue(reader, "MargenGanancia");
                                    pedido.PaymentTerminal = Other.GetStringValue(reader, "Condición de Pago");
                                    pedido.Moneda = Other.GetStringValue(reader, "Moneda");
                                    pedido.DocTotal = Other.GetStringValue(reader, "Total de Documento");
                                    pedido.SalesPerson = Other.GetStringValue(reader, "Empleado de Ventas");
                                    pedido.Coment = Other.GetStringValue(reader, "Comentarios");
                                    pedido.DocType = Other.GetStringValue(reader, "TipoDocumento");

                                    listUsuario.Add(pedido);
                                }
                            }
                        }
                    }
                }

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Pedidos", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Pedidos", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return new ListPedidoBo() { Data = listUsuario };
        } 

        public ListPedidoDetalleBo Get_PedidosDetalle(string docEntry,string Tipo)
        {
            List<PedidoDetalleBo> listUsuario = new List<PedidoDetalleBo>();
            string strSQL = string.Empty;

            strSQL= string.Format("CALL {0}.P_VIS_APPROV_DETAIL_DRAFT('{1}','{2}')", DataSource.bd(), docEntry,Tipo);

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
                                    PedidoDetalleBo pedido = new();
                                    pedido.numArticle = Other.GetStringValue(reader, "Número de Artículo");
                                    pedido.ItemName = Other.GetStringValue(reader, "Descripción de Artículo").ToUpper();
                                    pedido.Quantity = Other.GetStringValue(reader, "Cantidad");
                                    pedido.OnHand = Other.GetStringValue(reader, "Cantidad En Almacén");
                                    pedido.PriceUnit = Other.GetStringValue(reader, "Precio Unit");
                                    pedido.DescontPercent = Other.GetStringValue(reader, "% Desc");
                                    pedido.LineTotal = Other.GetStringValue(reader, "Total Linea");
                                    pedido.issetImpuesto = Other.GetStringValue(reader, "Sólo Impuesto");
                                    pedido.CodImpuesto = Other.GetStringValue(reader, "Cod Impuesto");
                                    pedido.Warehouse = Other.GetStringValue(reader, "Almacén");
                                    pedido.MarginGain = Other.GetStringValue(reader, "Margen Ganancia") + "%";
                                    if (Tipo != "")
                                    {
                                        pedido.USUARIO_ID = Other.GetStringValue(reader, "USUARIO_ID");
                                        pedido.USUARIO = Other.GetStringValue(reader, "USUARIO");
                                    }
                                    pedido.PriceHistory = Other.GetDoubleValue(reader, "PriceHistory");
                                    pedido.PriceReference = Other.GetDoubleValue(reader, "PriceReference");
                                    pedido.PorcUltComExc = Other.GetDoubleValue(reader, "PorcUltComExc");
                                    listUsuario.Add(pedido);
                                }
                            }
                        }
                    }
                }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_PedidosDetalle", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_PedidosDetalle", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }

            return new ListPedidoDetalleBo() { Data = listUsuario };
        }

        public ListAprovacionBo Get_Rules(string docEntry,string Tipo)
        {
            List<AprobacionBo> listUsuario = new List<AprobacionBo>();

            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_DETAIL_RULE('{1}','{2}')", DataSource.bd(), docEntry, Tipo);

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
                                    AprobacionBo aprobacion = new();
                                    aprobacion.Codigo = Other.GetStringValue(reader, "Código");
                                    aprobacion.CodRegla = Other.GetStringValue(reader, "CodRegla").ToUpper();
                                    aprobacion.Etapa = Other.GetStringValue(reader, "Etapa").ToUpper();
                                    aprobacion.Modelo = Other.GetStringValue(reader, "Modelo").ToUpper();
                                    aprobacion.Autorizador = Other.GetStringValue(reader, "Autorizador").ToUpper();
                                    aprobacion.Decisión = Other.GetStringValue(reader, "Decisión").ToUpper();
                                    aprobacion.Comment = Other.GetStringValue(reader, "Comentario de Aprobación").ToUpper();
                                    aprobacion.DecBKP = Other.GetStringValue(reader, "DecBKP").ToUpper();

                                    listUsuario.Add(aprobacion);
                                }
                            }
                        }
                    }
                }


            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Rules", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Rules", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return new ListAprovacionBo() { Data = listUsuario };
        }

        public LstAnexo Get_Anexos(string DocEntry)
        {
            List<Anexo> listAnexo = new List<Anexo>();
            LstAnexo lstAnexo = new LstAnexo();
            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_ANEXO('{1}')", DataSource.bd(), DocEntry);

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
                                    Anexo PriceHistoy = new();
                                    PriceHistoy.Documento = Other.GetStringValue(reader, "Archivo");
                                    PriceHistoy.Date = Other.GetStringValue(reader, "Date");
                                    PriceHistoy.Link = Other.GetStringValue(reader, "subPath");

                                    listAnexo.Add(PriceHistoy);
                                }
                            }
                        }
                    }
                }
                lstAnexo.Data = listAnexo;

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Anexos", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_Anexos", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return lstAnexo;
        }

        public List<PriceHistoy> Get_PriceHistory(string ItemCode)
        {
            List<PriceHistoy> listPriceHistoy = new List<PriceHistoy>();
            string strSQL = string.Format("CALL {0}.P_VIS_APPROV_PRICE_HISTORY('{1}')", DataSource.bd(), ItemCode);

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
                                    PriceHistoy PriceHistoy = new();
                                    PriceHistoy.CardName = Other.GetStringValue(reader, "CardName");
                                    PriceHistoy.DocDate = Other.GetStringValue(reader, "DocDate").ToUpper();
                                    PriceHistoy.Price = Other.GetDoubleValue(reader,"Price");

                                    listPriceHistoy.Add(PriceHistoy);
                                }
                            }
                        }
                    }
                }
            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_PriceHistory", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - Get_PriceHistory", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return listPriceHistoy;
        }

        public async Task<ResponseData> decicion(string json,string docEntry,string sessionId, string Tipo)
        {
            string strSQL = string.Empty;

            ResponseData response = new ResponseData();
            if (Tipo=="")
            {
                 response = await serviceLayer.Request("/b1s/v1/ApprovalRequests(" + docEntry + ")", Method.PATCH, json, sessionId);

                try
                {
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
                catch (HanaException ex)
                {
                    Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - decicion","JSON : "+ json.ToString()+"\n"+" DocEntry" + docEntry+ "\n"+"Tipo: "+ Tipo + "\n" + $"Error de conexión a HANA: {ex.Message}");
                    SentrySdk.CaptureException(ex);
                    throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
                }
                catch (Exception ex)
                {
                    Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - decicion", "JSON : " + json.ToString() + "\n" + " DocEntry" + docEntry + "\n" + "Tipo: " + Tipo + "\n" + $"Error de conexión a HANA: {ex.Message}");
                    SentrySdk.CaptureException(ex);
                    throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
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
                        A = "R";
                    }
                    else
                    {   
                        A = "A";
                    }

                    strSQL=string.Format("CALL {0}.P_VIS_APPROV_DECISION('{1}','{2}','{3}','{4}')", DataSource.bd(), docEntry, Tipo, A, staRemarkstus);

                    using (HanaConnection connection = GetConnection())
                    {
                        connection.Open();
                        using (HanaCommand command = new HanaCommand(strSQL, connection))
                        {
                            using (HanaDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                            {
                                response = new ResponseData();
                                response.Data = "OK";
                                response.StatusCode = HttpStatusCode.OK;
                            } 
                        }
                    }

                }
                catch (HanaException ex)
                {
                    Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - decicion 2 ", "JSON : " + json.ToString() + "\n" + " DocEntry" + docEntry + "\n" + "Tipo: " + Tipo + "\n" + strSQL +"\n"+ $"Error de conexión a HANA: {ex.Message}");
                    SentrySdk.CaptureException(ex);
                    throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
                }
                catch (Exception ex)
                {
                    Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - decicion 2 ", "JSON : " + json.ToString() + "\n" + " DocEntry" + docEntry + "\n" + "Tipo: " + Tipo + "\n" + strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                    SentrySdk.CaptureException(ex);
                    throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
                }

            }

            return response;

        }

        public ResponseData ListadoAprobadores(string DocEntry)
        {
            ResponseData rs = new ResponseData();

            List<ListStatusAprobadores> LsStatusAprobadores = new List<ListStatusAprobadores>();
            ListStatusAprobadores ObjStatusAprobadores = new ListStatusAprobadores();
            string strSQL = string.Format("CALL {0}.APP_LSAPROBADORES('{1}')", DataSource.bd(), DocEntry);

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
                                    ObjStatusAprobadores = new ListStatusAprobadores();
                                    ObjStatusAprobadores.Aprobador = Other.GetStringValue(reader, "Aprobado");
                                    ObjStatusAprobadores.Estado = Other.GetStringValue(reader, "Estado").ToUpper();
                                    ObjStatusAprobadores.Comentario = Other.GetStringValue(reader, "Comentario").ToUpper();

                                    LsStatusAprobadores.Add(ObjStatusAprobadores);
                                }
                            }
                        }
                    }
                }

                rs.StatusCode = HttpStatusCode.Accepted;
                rs.Data  = LsStatusAprobadores;

            }
            catch (HanaException ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - ListadoAprobadores", strSQL + "\n" + $"Error de conexión a HANA: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("No se pudo establecer la conexión con la base de datos HANA.", ex);
            }
            catch (Exception ex)
            {
                Other.EnviarCorreoOffice365(DataSource.bd() + " Error - APP Movil - ListadoAprobadores", strSQL + "\n" + $"Ocurrió un error al ejecutar la consulta o procesar los datos: {ex.Message}");
                SentrySdk.CaptureException(ex);
                throw new Exception("Ocurrió un error al ejecutar la consulta o procesar los datos.", ex);
            }
            return rs;
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
