using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class KardexBO
    {
        public string CardCode { get; set; }
        public string DocCur { get; set; }
        public string NumAtCard { get; set; }
        public string TaxDate { get; set; }
        public string DocDueDate { get; set; }
        public decimal DocTotal { get; set; }
        public string DocEntry { get; set; }
        public decimal Balance { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string Mobile { get; set; }
        public string SlpCode { get; set; }
        public string Street { get; set; }
        public string PymntGroup { get; set; }
        public string Block { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public decimal AmountCharged { get; set; }
        public string IncomeDate { get; set; }
        public string OperationNumber { get; set; }
        public string DocNum { get; set; }
        public string JrnlMemo { get; set; }
        public string Comments { get; set; }
        public string Bank { get; set; }
        public string SalesInvoice { get; set; }
        public string CollectorInvoice { get; set; }

    }
    public class ListaKardex
    {
        public List<KardexBO> Kardex { get; set; }
    }

}
