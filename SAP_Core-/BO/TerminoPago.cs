using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class TerminoPagoBO
    {
        public string PymntGroup { get; set; }
        public string PymntTerm { get; set; }
        public string Cash { get; set; }
        public string DueDays { get; set; }
    }
    public class ListaTerminoPago
    {
        public List<TerminoPagoBO> PaymentTerms { get; set; }
    }
}
