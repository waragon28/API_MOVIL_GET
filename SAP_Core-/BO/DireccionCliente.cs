using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class DireccionClienteBO
    {
        //public string CardCode { get; set; }
        public string ShipToCode { get; set; }
        public string Street { get; set; }
        public string TerritoryID { get; set; }
        public string Territory { get; set; }
        public string SlpCode { get; set; }
        public string SlpName { get; set; }
    }
    public class ListaDirecciones
    {
        public List<DireccionClienteBO> Addresses { get; set; }
    }
    
}
