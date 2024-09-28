using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SAP_Core.BO
{

    public class LoginSL
    {
        public string token { get; set; }
        public DateTime expityTime { get; set; }
    }
    public  class UsuarioBO 
    {
        [Required]
        public string Imei { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Languaje { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string WarehouseCode { get; set; }
        public string SalesPersonCode { get; set; }
        public string Profile { get; set; }
        public decimal Rate { get; set; }
        public string CogsAcct { get; set; }
        public string DiscAccount { get; set; }
        public string DocumentsOwner { get; set; }
        public int Receipt { get; set; }
        public string Branch { get; set; }
        public string CostCenter { get; set; }
        public string BusinessUnit { get; set; }
        public string ProductionLine { get; set; }
        
        
    }// fin de la clase
    public class ListUsuario
    {
        public List<UsuarioBO> Users { get; set; }
    }
}
