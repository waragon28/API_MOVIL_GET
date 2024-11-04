using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using SalesForce.BO;
using SalesForce.Util;
using SAP_Core.BO;
using SAP_Core.DAL;
using static WebApiNetCore.Utils.Other;

namespace WebApiNetCore.Utils
{
  
    public class Notificacion: INotificationService
    {
        private readonly ServiceLayer serviceLayer;
        UsuarioDAL user = new UsuarioDAL();

        public Notificacion(ServiceLayer _serviceLayer)
        {
            serviceLayer = _serviceLayer;
        }

        public Task<ResponseData> SendEmail(string docEntry)
        {
            throw new NotImplementedException();
        }

        async Task<ResponseData> INotificationService.SendSms(string endPoint,string docEntry, string status, string returnReasonText)
        {
            ResponseData response = null;
            LoginSL sl = user.loginServiceLayer().GetAwaiter().GetResult();

            try
            {
                response = await serviceLayer.Request("/b1s/v1/" + endPoint + "(" + docEntry + ")?$select=NumAtCard,BusinessPartner&$expand=BusinessPartner($select=Phone1, CardName)", Method.GET, sl.token);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseBody = await response.Data.Content.ReadAsStringAsync();
                    //BussinesParnert customer = JsonSerializer.Deserialize<BussinesParnert>(responseBody);
                    DN customer = JsonSerializer.Deserialize<DN>(responseBody);

                    if (customer.BusinessPartner.Telefono != null)
                    {
                        string telefono = customer.BusinessPartner.Telefono;
                        string messageDetaill = string.Empty;
                        string numG = customer.NumAtCard;

                        int numericValue; //waragon
                        bool isNumber = int.TryParse(telefono, out numericValue);//waragon
                        if (isNumber)//waragon
                        {
                            if (telefono.Length == 9)
                            {
                                telefono = "51" + telefono;
                            }
                            else if (telefono.Length == 12)
                            {
                                telefono = telefono.Replace("+", string.Empty);
                            }

                            switch (status)
                            {
                                case "E":
                                    messageDetaill = "Entregado.";
                                    break;
                                case "V":
                                    messageDetaill = "Volver a Programar, debido a que " + returnReasonText + ".";
                                    break;
                                case "A":
                                    messageDetaill = "Anulado, debido a que " + returnReasonText + ".";
                                    break;
                                default:
                                    messageDetaill = string.Empty;
                                    break;
                            }

                            if (messageDetaill != string.Empty)
                            {
                                string mensaje = @"VISTONY: Hola " + customer.BusinessPartner.CardName + ", Nos complace informarle que hemos cambiado el estado de su pedido " + numG + " a " + messageDetaill;
                                string jsonPayload = "{\"Data\":[{\"Mensaje\":\"" + mensaje + "\",\"NumeroTelf\":\"" + telefono + "\"}]}";

                                ResponseData responseSms = await ServiceApi.Request("http://192.168.254.20:88", "/vs1.0/sms", Method.POST, jsonPayload);
                            }
                        }



                    }

                }
                else
                {
                    response.StatusCode = HttpStatusCode.FailedDependency;
                    response.Data = null;
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                user.LogoutServiceLayer().GetAwaiter().GetResult();
            }
           
            return response;
        }
    }

    public interface INotificationService
    {
        Task<ResponseData> SendSms(string endPoint, string docEntry, string status, string returnReasonText);
        Task<ResponseData> SendEmail(  string docEntry);
    }
}
