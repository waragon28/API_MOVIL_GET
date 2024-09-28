using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class DespachoHeaderBO
    {
        public string Assistant { get; set; }
        public string ControlCode { get; set; }
        public string Brand { get; set; }
        public decimal OverallWeight { get; set; }
        public string LicensePlate { get; set; }
        public Object Detail { get; set; }
    }
    public class ListaDespachoHeaderBO
    {
        public List<DespachoHeaderBO> Obtener_DespachoCResult { get; set;}
    }
    public class DetailDespacho
    {
        public string Item { get; set; }
        public string CardCode { get; set; }
        public string ShipToCode { get; set; }
        public string Address { get; set; }
        public string InvoiceNum { get; set; }
        public string DeliveryNum { get; set; }
        public string DeliveryLegalNumber { get; set; }
        public string InvoiceLegalNumber { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string SlpCode { get; set; }
        public string SlpName { get; set; }
        public string PymntGroup { get; set; }
        public decimal Weight { get; set; }
    }
}
