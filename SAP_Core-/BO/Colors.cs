using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ColorsDetailBO
    {
        public string ColorMax { get; set; }
        public string ColorMin { get; set; }
        public string Degrade { get; set; }
        public decimal RangeMax { get; set; }
        public decimal RangeMin { get; set; }
    }
    public class ColorHeaderBO
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Object Detail { get; set; }
    }
    public class ListaColores
    {
        public List<ColorHeaderBO> Colors { get; set; }
    }
}
