using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ListaPromoD
    {
        public string PromotionListID { get; set; }
        public string PromotionID { get; set; }
        public string PromotionDetail { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountPrcent { get; set; }
    }
    public class ListarPromoD
    {
        public List<ListaPromoD> PromotionListDetail { get; set; }
    }
}
