using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class Ubigeo
    {
        public string Code { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string Block { get; set; }
        public int Flete { get; set; }
    }
    public class ListaUbigeo
    {
        public List<Ubigeo> Ubigeos { get; set; }
    }
}
