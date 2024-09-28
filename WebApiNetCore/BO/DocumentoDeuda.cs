using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class DocumentoDeudaBO
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string ShipToCode { get; set; }
        public string LegalNumber { get; set; }
        public string TaxDate { get; set; }
        public string DueDate { get; set; }
        public string DueDays { get; set; }
        public string Currency { get; set; }
        public decimal DocTotal { get; set; }
        public decimal Balance { get; set; }
        public decimal RawBalance { get; set; }
        public string Driver { get; set; }
        public string IDDriver { get; set; }
        public string Mobile { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string SalesOrderID { get; set; }
        public string DeliveryDate { get; set; }
        public string PymntGroup { get; set; }
        public string DeliveryStatus { get; set; }
        public string Ocurrency { get; set; }
        public string LegalNumberDelivery { get; set; }
        public string Additionaldiscount { get; set; }
    }
    public class ListaDocumentoDeuda
    {
        public List<DocumentoDeudaBO> Documents { get; set; }
    }
}
