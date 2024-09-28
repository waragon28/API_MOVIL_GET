using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class OrdenVentaC
    {
        public string DocNum { get; set; }
        public string Object { get; set; }
        public string CardCode { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        public decimal DocTotal { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalCommentary { get; set; }
        public string SalesOrderID { get; set; }
        public string SlpCode { get; set; }
    }
    public class ListaOrdenVentaC
    {
        public List<OrdenVentaC> SalesOrder { get; set; }
    }
    public class OrdenVentaBO
    {
        public string json { get; set; }
    }
}
