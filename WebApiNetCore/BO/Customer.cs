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


    public class BPAddress
    {
        public string AddressName { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string AddressType { get; set; }
        public string U_VIS_TerritoryID { get; set; } = "Z005";
        public string U_VIS_VisitOrder { get; set; } = "1";
        public string U_VIS_SlpCode { get; set; }
        public string U_VIS_SlpName { get; set; }
        //public string U_VIS_LongitudeApp { get; set; } = "0";
        // public string U_VIS_LatitudeApp { get; set; } = "0";
        public int U_VIS_DeliveryDay { get; set; }
    }

    public class ContactEmployee
    {
        public string Name { get; set; }
        public object Address { get; set; }
        public object Phone1 { get; set; }
        public object Phone2 { get; set; }
        public object MobilePhone { get; set; }
        public string E_Mail { get; set; }
        public string Active { get; set; }
        public string FirstName { get; set; }
        public object MiddleName { get; set; }
        public object LastName { get; set; }
    }

    public class CustomerCreate
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public int GroupCode { get; set; }
        public int PayTermsGrpCode { get; set; }
        public string FederalTaxID { get; set; }
        public string Currency { get; set; }
        public string EmailAddress { get; set; }
        public string U_SYP_BPAP { get; set; }
        public string U_SYP_BPAM { get; set; }
        public string U_SYP_BPNO { get; set; }
        public string U_SYP_BPN2 { get; set; }
        public string U_SYP_BPTP { get; set; }
        public string U_SYP_BPTD { get; set; }
        public string U_SYP_PLVAR { get; set; }
        public string U_SYP_DOCMAS { get; set; }
        public string U_SYP_CATCLI { get; set; }
        public string U_EconomyActivity { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        //public double U_SF_LinieaCredSolic { get; set; }
        //public string U_SF_CondicionPago { get; set; }
        //public string U_SF_StatusMigration { get; set; }
       // public string U_U_SF_Cod { get; set; }
        public string U_VIS_SaleCategory { get; set; }
        public string U_VIS_Category { get; set; }
        // public string PriceListNum { get; set; }



        public List<BPAddress> BPAddresses { get; set; }
        public List<ContactEmployee> ContactEmployees { get; set; }
    }

}
