//#define VISTONY

using SalesForce.BO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiNetCore;
using Newtonsoft.Json.Linq;
using SAP_Core.DAL;

namespace SalesForce.Util
{   
    public class LoginSAP
    {
        private static readonly string UriServiceLayer = Startup.Configuration.GetValue<string>("ServiceLayer:PathUri");
        private static readonly double MinuteCacheServer = Startup.Configuration.GetValue<double>("ServiceLayer:MinuteCacheServer");

         static  UsuarioDAL user = new UsuarioDAL();

        public static ResponseLogin Login(int countLogin, IMemoryCache _memoryCache,Acceso acceso,ref string SessionId)
        {
            var response = LoginRequest(acceso).GetAwaiter().GetResult();

            if (response.StatusCode==200)
            {
                SessionId = response.SessionId;
                // _memoryCache.Set("PE-SessionId", response.SessionId/*, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteCacheServer))*/);
                // _memoryCache.Set("PE-CountLogin", "" + countLogin/*, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5))*/);
            }

            return response;
        }
        public static ResponseLoginV2 LoginApprov(int countLogin, IMemoryCache _memoryCache, Acceso acceso, ref string SessionId)
        {
            var response = LoginRequestApprov(acceso).GetAwaiter().GetResult();

            if (response.StatusCode == 200)
            {
                SessionId = response.SessionId;
                // _memoryCache.Set("PE-SessionId", response.SessionId/*, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteCacheServer))*/);
                // _memoryCache.Set("PE-CountLogin", "" + countLogin/*, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5))*/);
            }

            return response;
        }

        private static async Task<ResponseLoginV2> LoginRequestApprov(Acceso acceso)
        {
            try
            {
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient cliente = new(clientHandler);
                StringContent content = new(JsonSerializer.Serialize(acceso), Encoding.UTF8, "application/json");

                var respuesta = await cliente.PostAsync("https://ecs-dbs-vistony:50000/b1s/v1/Login", content);

                var responseBody = await respuesta.Content.ReadAsStringAsync();

                ResponseLoginV2 obj = new ResponseLoginV2();
                // Parsear el JSON
                JObject jsonObject = JObject.Parse(responseBody);

                 // Leer el valor de SessionId;
                obj.SessionId= jsonObject["SessionId"]?.ToString();
                obj.odata= jsonObject["odata.metadata"]?.ToString();
                obj.Version= jsonObject["Version"]?.ToString();
                obj.SessionTimeout= 30;

                return obj;
            }
            catch (Exception ex)
            {
                return new ResponseLoginV2() { Response = ex.Message, StatusCode = 500 };
            }
        }
        private static async Task<ResponseLogin> LoginRequest(Acceso acceso)
        {
            try
            {
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient cliente = new(clientHandler);
                StringContent content = new(JsonSerializer.Serialize(acceso), Encoding.UTF8, "application/json");

                var respuesta = await cliente.PostAsync("https://ecs-dbs-vistony:50000/b1s/v1/Login", content);

                var responseBody = await respuesta.Content.ReadAsStringAsync();
                
                ResponseLogin obj = new ResponseLogin();
                // Parsear el JSON
                JObject jsonObject = JObject.Parse(responseBody);

                 // Leer el valor de SessionId;
                obj.SessionId= jsonObject["SessionId"]?.ToString();
                obj.odata= jsonObject["odata.metadata"]?.ToString();
                obj.Version= jsonObject["Version"]?.ToString();
                obj.SessionTimeout= 30;


                return obj;
            }
            catch (Exception ex)
            {
                return new ResponseLogin() { Response = ex.Message, StatusCode = 500 };
            }
            finally
            {
                user.LogoutServiceLayer().GetAwaiter().GetResult();
            }
        }

    }
}
