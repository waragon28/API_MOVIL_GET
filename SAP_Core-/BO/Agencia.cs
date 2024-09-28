using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class AgenciaBO
    {
        public string AgencyID { get; set; }
        public string Agency { get; set; }
        public string ZipCode { get; set; }
        public string LicTradNum { get; set; }
        public string Street { get; set; }
    }
    public class ListAgencia
    {
        public List<AgenciaBO> Agencies { get; set; }
    }
}
