using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class FacturaCBO
    {
        public string SalesOrderID { get; set; }
        public double DocTotalOV { get; set; }
        public string CardCode { get; set; }
        public string RucDni { get; set; }
        public string CardName { get; set; }
        public string DocID { get; set; }
        public string NumAtCard { get; set; }
        public DateTime TaxDate { get; set; }
        public double DocTotalFT { get; set; }
        public double Balance { get; set; }
        public string DriverName { get; set; }
        public string PackOffStatus { get; set; }
        public string ReasonPackOff { get; set; }
        public string PymntGroup { get; set; }
        public string InvoiceType { get; set; }
        public string SchedulingDate { get; set;}
    }
    public class ListaFactura
    {
        public List<FacturaCBO> Invoices { get; set; }
    }
}
