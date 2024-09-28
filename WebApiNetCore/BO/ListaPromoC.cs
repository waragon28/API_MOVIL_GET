﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ListaPromoC
    {
        public string PromotionListID { get; set; }
        public string PromotionID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom { get; set; }
        public string Tipo_Malla { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountPrcent { get; set; }
        public decimal Cantidad_Maxima { get; set; }
        public string Combo { get; set; }
    }
    public class ListarPromoC
    {
        public List<ListaPromoC> PromotionListHeader { get; set; }
    }
}