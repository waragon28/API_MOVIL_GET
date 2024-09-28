using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class TipoListaPromoBO
    {
        public string ListTypeID { get; set; }
        public string ListTypeName { get; set; }
        public string CashDscnt { get; set; }
        public string CustomerRecovery { get; set; }
    }
    public class ListaTipoListaPromo
    {
        public List<TipoListaPromoBO> PromotionListType { get; set; }
    }
}
