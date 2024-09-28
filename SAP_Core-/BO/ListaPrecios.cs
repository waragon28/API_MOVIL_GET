using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ListaPreciosBO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom { get; set; }
        public decimal WhsStock { get; set; }
        public decimal StockTotal { get; set; }
        public decimal Cash { get; set; }
        public decimal Credit { get; set; }
        public decimal Gallons { get; set; }
        public string Type { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string CashDscnt { get; set; }
        public int Units { get; set; }
    }
    public class ListaPrecios
    {
        public List<ListaPreciosBO> PriceList { get; set; }
    }
}
