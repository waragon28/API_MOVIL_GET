using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class InvoicesBO
    {
        public string DocNum { get; set; }
        public string CardCode { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        public decimal DocTotal { get; set; }
        public string LegalNumber { get; set; }
        public string SlpCode { get; set; }
    }
    public class ListInvoices
    {
        public List<InvoicesBO> Invoices { get; set; }
    }
}
