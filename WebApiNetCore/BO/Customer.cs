using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        public string CustomerWhiteList { get; set; }
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
    public class Economic_Activity
    {
        public List<LineEconomic_Activity> Data { get; set; }
    }
    public class LineEconomic_Activity
    {
        public string Code { get; set; }
        public string Name { get; set; }
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
        public string SinTerminoContado { get; set; }
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
        public string CustomerWhiteList { get; set; }
        public Object Addresses { get; set; }
        public Object Invoices { get; set; }
        public string CustomerCriticalDistribution { get; set; }
        public string LastFreePurchase { get; set; }
        public string LastTradeMarketing { get; set; }
        public string EconomyActivity { get; set; }
        public string CustomerRecovery { get; set; }
    }
    public class ListaClientes2
    {
        public List<ClienteBO2> Customers { get; set; }
    }

    //Objeto de respuesta de serviceLayer
    public class BussinesParnert
    {
        [JsonPropertyName("Phone1")]
        public string Telefono { get; set; }
        public string CardName { get; set; }
    }
    public class DN
    {
        public string NumAtCard { get; set; }
        public BussinesParnert BusinessPartner { get; set; }
    }
    public class ValidateCredit
    {
        public string SlpCode { get; set; }
        public string SlpName { get; set; }
        public string ZonaID { get; set; }
        public string ZonaName { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string NumAtCard { get; set; }
        public string Asiento { get; set; }
        public string LineNum { get; set; }
        public string CtaContable { get; set; }
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        public decimal ImporteDoc { get; set; }
        public decimal Saldo { get; set; }
        public decimal Cobranza { get; set; }
        public string NroOP { get; set; }
        public string FechaPago { get; set; }
        public string TipoDoc { get; set; }
        public int DiasMora { get; set; }

    }
    public class OfBusiness
    {
        public List<LineBusiness> LineOfBusiness { get; set; }
    }
    public class LineBusiness
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string EconomicActivity { get; set; }
    }
    public class ListaValidateCredit
    {
        public List<ValidateCredit> listValidate { get; set; }
    }
}
