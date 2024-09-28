using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ClienteBO
    {
        public string CardCode { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        //public string Currency { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string ZipCode { get; set; }
        public double CreditLimit { get; set; }
        public double Balance { get; set; }
        public string VisitOrder { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string PymntGroup { get; set; }
        public string PayToCode { get; set; }
        public string PriceList { get; set; }
        public string Currency { get; set; }
        public int DueDays { get; set; }
        public List<DireccionClienteBO> Addresses { get; set; }
        public List<DocumentoDeudaBO> Invoices { get; set; }
        public List<VisitBOs> Visits { get; set; }
    }
    public class ListaClientes
    {
        public List<ClienteBO> Customers { get; set; }
    }
    public class CustomersLead
    {
        public string Code { get; set; }
        public int U_VIS_UserID { get; set; }
        public string U_VIS_SlpCode { get; set; }
        public string U_VIS_Date { get; set; }
        public string U_VIS_LicTradNum { get; set; }
        public string U_VIS_CardName { get; set; }
        public string U_VIS_TradeName { get; set; }
        public string U_VIS_Phone { get; set; }
        public string U_VIS_CellPhone { get; set; }
        public string U_VIS_ContactPerson { get; set; }
        public string U_VIS_Email { get; set; }
        public string U_VIS_Web { get; set; }
        public string U_VIS_Country { get; set; }
        public string U_VIS_City { get; set; }
        public string U_VIS_Street { get; set; }
        public string U_VIS_Address { get; set; }
        public string U_VIS_Reference { get; set; }
        public string U_VIS_Latitud { get; set; }
        public string U_VIS_Longitud { get; set; }
        public string U_VIS_Commentary { get; set; }
        public string U_VIS_RutaImagen { get; set; }
    }
    public class Currency
    {
        public string Code { get; set; }
    }
    public class ListCurrency
    {
        public List<Currency> Currencies { get; set; }
    }
    public class ClienteBO2
    {
        public string CardCode { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        //public string Currency { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string ZipCode { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal Balance { get; set; }
        public string VisitOrder { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string PymntGroup { get; set; }
        public string PayToCode { get; set; }
        public string PriceList { get; set; }
        public string Currency { get; set; }
        public int DueDays { get; set; }
        public string LineOfBusiness { get; set; }
        public string LastPurchase { get; set; }
        public Object Addresses { get; set; }
        public Object Invoices { get; set; }
    }
    public class ListaClientes2
    {
        public List<ClienteBO2> Customers { get; set; }
    }
}
