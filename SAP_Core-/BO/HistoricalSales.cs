using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class HistoricalSalesBO
    {
        public string SlpCode { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Variable { get; set; }
        public decimal TotalMN { get; set; }
        public decimal Galones { get; set; }
        public string UOM { get; set; }
    }
    public class ListHistoricalSalesBO
    {
        public List<HistoricalSalesBO> RecordSales { get; set; }
    }
}
