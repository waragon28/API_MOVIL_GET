using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO_WEB
{
    public class Customer_Header_BO
    {
        public string token { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string DocType { get; set; }
        public string LicTradNum { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public decimal OrdersBal{ get; set; }
        public decimal CreditLine { get; set; }
        public string PymntGroup{ get; set; }
        public string Category { get; set; }
        public string SalesCategory { get; set; }
        public string PunishedClient { get; set; }
        public string Image { get; set; }
        public List<Customer_Address> Addresses { get; set; }
    }
    public class Customer
    {
        public List<Customer_Header_BO> Customers { get; set; }
    }
}
