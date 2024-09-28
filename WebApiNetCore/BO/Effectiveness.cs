using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{
    public class EffectivenessBO
    {
        public string Route { get; set; }
        public string Customers { get; set; }
        public string Visit { get; set; }
        public string SalesOrder { get; set; }
        public string Collection { get; set; }
        public string Debtor { get; set; }
        public string AmountSO { get; set; }
        public string AmountCll { get; set; }
        public string VisitEff { get; set; }
        public string OrdersEff { get; set; }
        public string CollctnEff { get; set; }
        public string CusCoverage { get; set; }
        public string Coverage { get; set; }
        public string CoverageEff { get; set; }
    }
    public class ListEffectiveness
    {
        public List<EffectivenessBO> Effecetiveness { get; set; }
    }
}
