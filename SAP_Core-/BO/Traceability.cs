using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class TraceabilityBO
    {
        public string Object { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string Canceled { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string SalesOrderID { get; set; }
        public string SlpCode { get; set; }
        public string PymntGroup { get; set; }
        public List<DocumentoDeudaBO> Invoices { get; set; }
    }
    public class ListTraceability
    {
        public List<TraceabilityBO> Traceabilities { get; set; }
    }
}
