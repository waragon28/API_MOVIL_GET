using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class RutaTrabajoBO
    {
        public string Territory { get; set; }
        public string TerritoryId { get; set; }
        public string Day { get; set; }
        public string Frequency { get; set; }
        public string VisitDate { get; set; }
        public string Status { get; set; }
        public string SlpCode { get; set; }
        public string InitDate { get; set; }
    }
    public class ListaRutaTrabajo
    {
        public List<RutaTrabajoBO> WorkPath { get; set; }
    }
}
