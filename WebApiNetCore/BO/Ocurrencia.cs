using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class OcurrencyBO
    {
        public string Value { get; set; }
        public string Dscription { get; set; }
    }
    public class ListOcurrencies
    {
        public List<OcurrencyBO> Ocurrencies { get; set; }
    }
}
