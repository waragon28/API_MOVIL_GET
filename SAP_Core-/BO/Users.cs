using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class User
    {
        public string Imei { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public string WhsCode { get; set; }
        public string SlpCode { get; set; }
        public string Profile { get; set; }
        public string Branch { get; set; }
        public decimal Rate { get; set; }
        public string  Sectorist { get; set; }
        public List<ConfigUser> Settings { get; set; }
    }
    public class ListUsers
    {
        public List<User> Users { get; set; }
    }
    public class ConfigUser
    {
        public string Language { get; set; }
        public string Receip { get; set; }
        public string CostCenter { get; set; }
        public string BusinessUnit { get; set; }
        public string ProductionLine { get; set; }
        public string DiscAccount { get; set; }
        public string CogsAcct { get; set; }
        public string TaxCode { get; set; }
        public decimal TaxRate { get; set; }
        public string OutStock { get; set; }
        public string Db { get; set; }
        public int MaxDateDeposit { get; set; }
        public string UsePrinter { get; set; }
        public string ChangeCurrency { get; set; }
        public decimal CashDscnt { get; set; }
    }
}
