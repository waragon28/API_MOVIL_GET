using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ColoresOV
    {
        public string Id { get; set; }
        public double Margen { get; set; }
        public string Color { get; set; }
    }

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


    public class ColorDocumentLine
    {
        public string AcctCode { get; set; }
        public string COGSAccountCode { get; set; }
        public string ColorApproval { get; set; }
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }
        public string DiscountPercent { get; set; }
        public string Dscription { get; set; }
        public string ItemCode { get; set; }
        public string LineTotal { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string TaxCode { get; set; }
        public string TaxOnly { get; set; }
        public string U_SYP_FECAT07 { get; set; }
        public string U_VIST_CTAINGDCTO { get; set; }
        public string U_VIS_CommentText { get; set; }
        public string U_VIS_PromID { get; set; }
        public string U_VIS_PromLineID { get; set; }
        public string WarehouseCode { get; set; }
        public string Line_ID { get; set; }
        
    }

    public class ColorSalesOrder
    {
        public string ApCredit { get; set; }
        public string ApDues { get; set; }
        public string ApPrcnt { get; set; }
        public string ApTPag { get; set; }
        public string AppVersion { get; set; }
        public string Branch { get; set; }
        public string Brand { get; set; }
        public string CardCode { get; set; }
        public string ColorApproval { get; set; }
        public string Comments { get; set; }
        public string DocCurrency { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string DocRate { get; set; }
        public string DocType { get; set; }
        public List<ColorDocumentLine> DocumentLines { get; set; }
        public string DocumentsOwner { get; set; }
        public string Draft { get; set; }
        public string FederalTaxID { get; set; }
        public string Intent { get; set; }
        public string Model { get; set; }
        public string OSVersion { get; set; }
        public string PayToCode { get; set; }
        public string PaymentGroupCode { get; set; }
        public string Route { get; set; }
        public string SalesPersonCode { get; set; }
        public string ShipToCode { get; set; }
        public string TaxDate { get; set; }
        public string U_SYP_DOCEXPORT { get; set; }
        public string U_SYP_FEEST { get; set; }
        public string U_SYP_FEMEX { get; set; }
        public string U_SYP_FETO { get; set; }
        public string U_SYP_MDMT { get; set; }
        public string U_SYP_PDTCRE { get; set; }
        public string U_SYP_PDTREV { get; set; }
        public string U_SYP_TVENTA { get; set; }
        public string U_SYP_VIST_TG { get; set; }
        public string U_VIST_SUCUSU { get; set; }
        public string U_VIS_AgencyCode { get; set; }
        public string U_VIS_AgencyDir { get; set; }
        public string U_VIS_AgencyName { get; set; }
        public string U_VIS_AgencyRUC { get; set; }
        public string U_VIS_CompleteOV { get; set; }
        public string U_VIS_DeliveryDateOptional { get; set; }
        public string U_VIS_DiscountPercent { get; set; }
        public string U_VIS_Flete { get; set; }
        public string U_VIS_MOTAPLDESC { get; set; }
        public string U_VIS_ReasonDiscountPercent { get; set; }
        public string U_VIS_SalesOrderID { get; set; }
        public string U_VIT_VENMOS { get; set; }
        public string quotation { get; set; }
    }

    public class ColorListSalesOrder
    {
        public List<ColorSalesOrder> SalesOrders { get; set; }
    }

    public class ListColorRange
    {
        public List<ColorRange> ColorRange { get; set; }
    }

    public class ColorRange
    {
        public string Range { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
