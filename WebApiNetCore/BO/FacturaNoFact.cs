using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class FacturaNoFact
    {
        public string LineNum { get; set; }
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom { get; set; }
        public decimal Quantity { get; set; }
    }
    public class ListaFacturaNoFact
    {
        public List<FacturaNoFact> UnbilledInvoices { get; set; }
    }
}
