using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class QuotaBO
    {
        public string PymntGrp { get; set; }
        public string Tramo { get; set; }
        public decimal Balance { get; set; }
        public int Quota { get; set; }
    }
    public class ListQuotaBO
    {
        public List<QuotaBO> Quotas { get; set; }
    }
    public class QuotaDetailBO
    {
        public string QuotaNumber { get; set; }
        public decimal Order { get; set; }
        public decimal Corriente { get; set; }
        public decimal Due { get; set; }
        public decimal Total { get; set; }
        public string Date { get; set; }
    }
    public class ListQuotaDetailBO
    {
        public List<QuotaDetailBO> QuotasDetail { get; set; }
    }

    public class QuotaInvoiceBO
    {
        public string TaxDate { get; set; }
        public string DueDate { get; set; }
        public string NumInvoince { get; set; }
        public decimal DocTotal { get; set; }
        public string PymntGroup { get; set; }
        public string DueDays { get; set; }
        public decimal QuotaAmmount { get; set; }
        public int QuotaNumber { get; set; }
        public string Type { get; set; }
        public string Dues { get; set; }
        public decimal Balance { get; set; }
    }
    public class ListQuotaInvoiceBO
    {
        public List<QuotaInvoiceBO> QuotaInvoices { get; set; }
    }
}
