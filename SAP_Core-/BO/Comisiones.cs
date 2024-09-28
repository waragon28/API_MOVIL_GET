using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ComisionesBO
    {
        public string Variable { get; set; }
        public string Uom { get; set; }
        public decimal Advance { get; set; }
        public decimal Quota { get; set; }
        public decimal Percentage { get; set; }
        public string CodeColor { get; set; }
        public string HideData { get; set; }
    }
    public class ListaComisiones
    {
        public List<ComisionesBO> Commissions { get; set; }
    }
}
