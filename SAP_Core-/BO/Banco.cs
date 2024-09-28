using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class BancoBO
    {
        public string BankId { get; set; }
        public string BankName { get; set; }
        public string SingleDeposit { get; set; }
    }
    public class ListaBanco
    {
        public List<BancoBO> Banks { get; set; }
    }
}
