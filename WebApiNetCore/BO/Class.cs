using System.Collections.Generic;

namespace WebApiNetCore.BO
{
    public class Cabecera_Mensaje
    {
        public List<Data> Data { get; set; }
    }
    public class Data
    {
        public string Mensaje { get; set; }
        public string NumeroTelf { get; set; }
    }
}
