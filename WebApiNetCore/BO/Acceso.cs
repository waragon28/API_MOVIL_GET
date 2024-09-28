using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesForce.BO
{

    public class Acceso
    {
        [JsonPropertyName("CompanyDB")]
        public string CompanyDB { get; set; }

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }

    public class PerfilApproval
    {
        public string Option { get; set; }
    }
    public class ResponseLogin
    {
        [JsonPropertyName("odata.metadata")]
        public string odata { get; set; }

        [JsonPropertyName("SessionId")]
        public string SessionId { get; set; }

        [JsonPropertyName("Version")]
        public string Version { get; set; } 
        
        [JsonPropertyName("SessionTimeout")]
        public int SessionTimeout { get; set; }
        public string Response { get; set; }
        public int StatusCode { get; set; } = 200;
    }

    public class ResponseLoginV2
    {
        [JsonPropertyName("odata.metadata")]
        public string odata { get; set; } = "";

        [JsonPropertyName("SessionId")]
        public string SessionId { get; set; }

        [JsonPropertyName("Version")]
        public string Version { get; set; }

        [JsonPropertyName("SessionTimeout")]
        public int SessionTimeout { get; set; }
        public dynamic Accesos { get; set; }
        public string Response { get; set; }
        public int StatusCode { get; set; } = 200;
    }

    public class ResponseData
    {
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Data { get; set; }
    }


    public class Quotation
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string Message { get; set; }
        public string SalesOrderID { get; set; }
       // public string cardCode { get; set; }
       // public string docDate { get; set; }
       // public string slpCode { get; set; }
        public string ErrorCode { get; set; }
    }

    public class QuotationList
    {
        public List<Quotation> SalesOrders { get; set; }
    }


}