using SalesForce.BO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiNetCore;
using static WebApiNetCore.Utils.Other;

namespace SalesForce.Util
{
    public static class ServiceApi
    {
        public static async Task<ResponseData> Request(string uriServiceApi,string endPoint, Method method, string jsonBody="")
        {
            ResponseData response = new();

            try
            {
                var baseAddress = new Uri(uriServiceApi);

                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient cliente = new(clientHandler)
                {
                    BaseAddress = baseAddress                       
                };

                //cliente.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=2000");

                var respuesta = (dynamic) null;
                StringContent jsonContent = new(jsonBody, Encoding.UTF8, "application/json");

                switch (method)
                {
                    case Method.GET:
                        respuesta = await cliente.GetAsync(endPoint);
                        break;
                    case Method.POST:
                         respuesta = await cliente.PostAsync(endPoint, jsonContent);
                        break;
                    case Method.PATCH:
                         respuesta = await cliente.PatchAsync(endPoint, jsonContent);
                        break;
                    case Method.DELETE:
                         respuesta = await cliente.DeleteAsync(endPoint);
                        break;
                }

                response.StatusCode = respuesta.StatusCode;
                response.Data = respuesta;

            }catch (Exception ex){
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Data = ex.Message;
            }

            return response;
        }
    }
}
