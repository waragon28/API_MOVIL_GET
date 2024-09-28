using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SalesForce.BO
{
    public class Odata
    {
        public class ODataResponse
        {
            [JsonPropertyName("value")]
            public dynamic Value { get; set; }
            
            [JsonPropertyName("@odata.nextLink")]
            public string nextLink { get; set; }
            public string Permitted { get; set; }
            public string Message { get; set; }
            public string Footer { get; set; }

        }     
    }
}
