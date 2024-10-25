

using SalesForce.BO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiNetCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using static WebApiNetCore.Utils.Other;
using Sentry;
using SAP_Core.DAL;

namespace SalesForce.Util
{
    public class ServiceLayer
    {
        private IMemoryCache memoryCache;
        private static readonly string UriServiceLayer = Startup.Configuration.GetValue<string>("ServiceLayer:PathUri");
        private static readonly double MinuteCacheServer = Startup.Configuration.GetValue<double>("ServiceLayer:MinuteCacheServer");

        UsuarioDAL user = new UsuarioDAL();

        public ServiceLayer(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }

        string cookieKey;
        public async Task<ResponseData> Request(string endPoint, Method method, string jsonBody="", string sessionId="")
        {
            ResponseData response = new();

            try
            {
                var baseAddress = new Uri(UriServiceLayer);
                var cookieContainer = new CookieContainer();

                HttpClientHandler clientHandler = new()
                {
                    CookieContainer = cookieContainer,
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient cliente = new(clientHandler)
                {
                    BaseAddress = baseAddress
                };




                cliente.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=2000");


                var temp = memoryCache.Get("PE-CountLogin") as string;

                if (sessionId != "")
                {

                    cookieKey = sessionId;
                }
                else
                {
                    var credenciales = new Acceso
                    {
                        CompanyDB = Startup.Configuration.GetValue<string>("ServiceLayer:PE:CompanyDB"),
                        UserName = Startup.Configuration.GetValue<string>("ServiceLayer:PE:UserName"),
                        Password = Startup.Configuration.GetValue<string>("ServiceLayer:PE:Password"),
                    };

                    LoginSAP.Login(0, memoryCache, credenciales, ref cookieKey);

                    // cookieKey = sessionId;
                }

                int count = int.Parse(temp ?? "0");

                cookieContainer.Add(baseAddress, new Cookie("B1SESSION", cookieKey));
                count++;

                var respuesta = (dynamic)null;
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

                if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (count > 2)
                    {
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Data = null;

                        memoryCache.Set("PE-CountLogin", "" + 0, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteCacheServer)));
                    }
                    else
                    {
                    }
                }
                else
                {

                    response.StatusCode = respuesta.StatusCode;
                    response.Data = respuesta;

                    memoryCache.Set("PE-CountLogin", "" + 0, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteCacheServer)));

                }

            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Data = ex.Message;
            }

            finally
            {
               
            }

            return response;
        }

        public async Task<ResponseData> Batch(MultipartContent mpContent)
        {
            ResponseData response = new();

            try
            {
                var baseAddress = new Uri(UriServiceLayer);
                var cookieContainer = new CookieContainer();

                HttpClientHandler clientHandler = new()
                {
                    CookieContainer = cookieContainer,
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient cliente = new(clientHandler)
                {
                    BaseAddress = baseAddress,
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var credenciales = new Acceso
                {
                    CompanyDB = Startup.Configuration.GetValue<string>("ServiceLayer:PE:CompanyDB"),
                    UserName = Startup.Configuration.GetValue<string>("ServiceLayer:PE:UserName"),
                    Password = Startup.Configuration.GetValue<string>("ServiceLayer:PE:Password"),
                };
                LoginSAP.Login(0, memoryCache, credenciales, ref cookieKey);

                cliente.DefaultRequestHeaders.Add("Prefer", "odata.continue-on-error");

              //  var cookieKey = memoryCache.Get("PE-SessionId") as string;
                var temp = memoryCache.Get("PE-CountLogin") as string;
                int count = int.Parse(temp ?? "0");

                cookieContainer.Add(baseAddress, new Cookie("B1SESSION", cookieKey));
                count++;

                var respuesta = (dynamic)null;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "b1s/v1/$batch");
                request.Content = mpContent;

                respuesta = cliente.SendAsync(request).Result;

                if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (count > 1)
                    {
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Data = null;

                        memoryCache.Set("PE-CountLogin", "" + 0, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteCacheServer)));
                    }
                    else
                    {
                       
                    }
                }
                else
                {

                    response.StatusCode = respuesta.StatusCode;
                    //response.Data = getMultiPart(respuesta).GetAwaiter().GetResult();
                    response.Data = respuesta.ToString();

                    memoryCache.Set("PE-CountLogin", "" + 0, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteCacheServer)));

                }

            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Data = ex.Message;
            }
            finally
            {
                
            }

            return response;
        }
        /*private async Task<List<HttpResponseMessage>> getMultiPart(HttpResponseMessage response)
        {
            var multipartContent = await response.Content.ReadAsMultipartAsync();
            
            var multipartRespMsgs = new List<HttpResponseMessage>();
            foreach (HttpContent currentContent in multipartContent.Contents)
            {
                if (currentContent.Headers.ContentType.MediaType.Equals("application/http", StringComparison.OrdinalIgnoreCase))
                {
                    if (!currentContent.Headers.ContentType.Parameters.Any(parameter => parameter.Name.Equals("msgtype", StringComparison.OrdinalIgnoreCase) && parameter.Value.Equals("response", StringComparison.OrdinalIgnoreCase)))
                    {
                        currentContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("msgtype", "response"));
                    }
                    multipartRespMsgs.Add(await currentContent.ReadAsHttpResponseMessageAsync());
                }
                else
                {
                    var subMultipartContent = await currentContent.ReadAsMultipartAsync();
                    foreach (HttpContent currentSubContent in subMultipartContent.Contents)
                    {
                        currentSubContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("msgtype", "response"));
                        multipartRespMsgs.Add(await currentSubContent.ReadAsHttpResponseMessageAsync());
                    }
                }
            }

            return multipartRespMsgs;
        }*/

    }
}
