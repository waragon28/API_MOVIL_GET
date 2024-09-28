using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class StockBO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom { get; set; }
        public decimal OnHand { get; set; }
        public decimal Committed { get; set; }
        public decimal Ordered { get; set; }
        public decimal Available { get; set; }
        public string WhsCode { get; set; }
    }
    public class ListaStock
    {
        public List<StockBO> Stock { get; set; }
    }
}
