using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class TipoDespachoBO
    {
        public string Value { get; set; }
        public string Dscription { get; set; }
    }
    public class ListTipoDespacho
    {
        public List<TipoDespachoBO> DispatchTypes { get; set; }
    }
}
