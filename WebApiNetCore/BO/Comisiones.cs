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
        public int Peso { get; set; }
        public decimal Combinada { get; set; }
        public decimal Escala { get; set; }
        public decimal Comision { get; set; }
        public decimal Premio { get; set; }
        public decimal Total { get; set; }
        public string Premio_Pregunta { get; set; }
        public string Detalle { get; set; }
        public string VariableComisional_Pregunta { get; set; }
    }
    public class ListaComisiones
    {
        public List<ComisionesBO> Commissions { get; set; }
    }

    public class CommissionDetals
    {
        public List<ListComisionDetalle> listComisionDetalles { get; set; }
    }
    public class ListComisionDetalle
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Category { get; set; }
        public string NextVisitDate { get; set; }
        public string LastPurchaseDate { get; set; }
        public string Covered { get; set; }
    }
}
