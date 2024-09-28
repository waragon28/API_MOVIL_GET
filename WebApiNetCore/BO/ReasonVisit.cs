using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ReasonVisitBO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    public class ListReasonVisit
    {
        public List<ReasonVisitBO> Reasons { get; set; }
    }
}
