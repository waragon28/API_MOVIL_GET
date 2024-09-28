using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class SalesOrder
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string SalesOrderID { get; set; }
    }
    public class ListOrders
    {
        public List<SalesOrder> SalesOrders { get; set; }
    }
    public class SalesOrderBO
    {
        public string CardCode { get; set; }
        public string Comments { get; set; }
        public string DocCurrency { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        //public string DocRate { get; set; }
        public string DocType { get; set; }
        public string U_VIS_SalesOrderID { get; set; }
        public string DocumentsOwner { get; set; }
        public string FederalTaxID { get; set; }
        public string PayToCode { get; set; }
        public string DocObjectCode { get; set; }
        public string PaymentGroupCode { get; set; }
        public string SalesPersonCode { get; set; }
        public string ShipToCode { get; set; }
        //public string TaxDate { get; set; }
        public string DiscountPercent { get; set; }
        public string U_VIST_SUCUSU { get; set; }
        public string U_VIS_AppVersion { get; set; }
        public string quotation { get; set; }
        public string Draft { get; set; }
        public List<DocumentLineBO> DocumentLines { get; set; }
        public List<Approval> Document_ApprovalRequests { get; set; }
    }
    public class SalesOrdersBO
    {
        public List<SalesOrderBO> SalesOrders { get; set; }
    }
    public class SalesOrderPE
    {
        public string CardCode { get; set; }
        public string Comments { get; set; }
        public string DocCurrency { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string DocRate { get; set; }
        public string DocType { get; set; }
        public string DocumentsOwner { get; set; }
        public string FederalTaxID { get; set; }
        public string PayToCode { get; set; }
        public string PaymentGroupCode { get; set; }
        public string SalesPersonCode { get; set; }
        public string ShipToCode { get; set; }
        public string TaxDate { get; set; }
        public string U_SYP_VIST_TG { get; set; }
        public string U_VIST_SUCUSU { get; set; }
        public string U_VIS_AgencyCode { get; set; }
        public string U_VIS_AgencyDir { get; set; }
        public string U_VIS_AgencyName { get; set; }
        public string U_VIS_AgencyRUC { get; set; }
        public string U_VIS_OVCommentary { get; set; }
        public string U_VIS_SalesOrderID { get; set; }
        public string DiscountPercent { get; set; }
        public string U_VIS_AppVersion { get; set; }
        public string U_SYP_MDMT { get; set; }
        public string U_SYP_TVENTA { get; set; }
        public string quotation { get; set; }
        public string Draft { get; set; }
        public string DocObjectCode { get; set; }
        public string U_SYP_DOCEXPORT { get; set; }
        public string U_SYP_FEEST { get; set; }
        public string U_SYP_FEMEX { get; set; }
        public string U_SYP_FETO { get; set; }
        public List<DocumentLinePE> DocumentLines { get; set; }
        public List<Approval> Document_ApprovalRequests { get; set; }

    }
    public class SalesOrdersPE
    {
        public List<SalesOrderPE> SalesOrders { get; set; }
    }
    public class Approval
    {
        public string ApprovalTemplatesID { get; set; }
        public string Remarks { get; set; }
    }
    public class ApprovalsBO
    {
        public List<Approval> Document_ApprovalRequests { get; set; }
    }
    public class DocumentLineBO
    {
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }
        public string DiscountPercent { get; set; }
        public string Dscription { get; set; }
        public string ItemCode { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string TaxCode { get; set; }
        public string TaxOnly { get; set; }
        public string WarehouseCode { get; set; }
    }
    public class DocumentLinePE
    {
        public string AcctCode { get; set; }
        public string COGSAccountCode { get; set; }
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }
        public string DiscountPercent { get; set; }
        public string Dscription { get; set; }
        public string ItemCode { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string TaxCode { get; set; }
        public string TaxOnly { get; set; }
        public string WarehouseCode { get; set; }
        public string U_SYP_FECAT07 { get; set; }
        public string U_VIST_CTAINGDCTO { get; set; }
        public string U_VIS_CommentText { get; set; }
        public string U_VIS_PromID { get; set; }
        public string U_VIS_PromLineID { get; set; }

    }
    public class DocumentLinesBO
    {
        public List<DocumentLineBO> DocumentLines { get; set; }
    }


    /*Object SalesOrder*/
    public class SalesOrderObject
    {
        public Salesorder[] SalesOrders { get; set; }
    }

    public class Salesorder
    {
        public string U_VIS_DeliveryDateOptional { get; set; }
        public string ApCredit { get; set; }
        public string ApDues { get; set; }
        public string ApPrcnt { get; set; }
        public string ApTPag { get; set; }
        public string AppVersion { get; set; }
        public string Branch { get; set; }
        public string Brand { get; set; }
        public string CardCode { get; set; }
        public string Comments { get; set; }
        public string DocCurrency { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string DocRate { get; set; }
        public string DocType { get; set; }
        public List<DocumentlineSalesOrder> DocumentLines { get; set; }
        public string DocumentsOwner { get; set; }
        public string FederalTaxID { get; set; }
        public string Intent { get; set; }
        public string Model { get; set; }
        public string OSVersion { get; set; }
        public string PayToCode { get; set; }
        public string PaymentGroupCode { get; set; }
        public string SalesPersonCode { get; set; }
        public string ShipToCode { get; set; }
        public string TaxDate { get; set; }
        public string U_SYP_DOCEXPORT { get; set; }
        public string U_SYP_FEEST { get; set; }
        public string U_SYP_FEMEX { get; set; }
        public string U_SYP_FETO { get; set; }
        public string U_SYP_MDMT { get; set; }
        public string U_SYP_TVENTA { get; set; }
        public string U_SYP_VIST_TG { get; set; }
        public string U_VIS_AgencyCode { get; set; }
        public string U_VIS_AgencyDir { get; set; }
        public string U_VIS_AgencyName { get; set; }
        public string U_VIS_AgencyRUC { get; set; }
        public string U_VIS_SalesOrderID { get; set; }
        public string quotation { get; set; }
        public string isQuotation { get; set; }
        public string U_VIS_DiscountPercent { get; set; }
        public string U_VIS_MOTAPLDESC { get; set; }
        public string U_VIS_ReasonDiscountPercent { get; set; }
        public string U_ID_Document { get; set; }
        public string U_SYP_PDTREV { get; set; }
    }

    public class DocumentlineSalesOrder
    {
        public string AcctCode { get; set; }
        public string COGSAccountCode { get; set; }
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
        public int BaseLine { get; set; }
        public string U_SYP_FECAT07 { get; set; }
        public string U_VIST_CTAINGDCTO { get; set; }
        public string U_VIS_CommentText { get; set; }
        public string U_VIS_PromID { get; set; }
        public string U_VIS_PromLineID { get; set; }
        public string WarehouseCode { get; set; }
    }
    public class ObjectBO
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class ListObjects
    {
        public List<ObjectBO> Objects { get; set; }
    }
    public class LisstBusinessLayerSalesDetail
    {
        public List<BusinessLayerSalesDetailBO> BusinessLayerDetail { get; set; }
    }
    public class BusinessLayerSalesDetailBO
    {
        public string Code { get; set; }
        public string Object { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public List<BusinessLayerSalesDetailDetailBO> Detail { get; set; }
    }
    public class BusinessLayerSalesDetailDetailBO
    {
        public string Field { get; set; }
        public string LineID { get; set; }
        public string Object { get; set; }
        public string RangeActive { get; set; }
        public string TypeObject { get; set; }
        public string ValueMax { get; set; }
        public string ValueMin { get; set; }
        public string Variable { get; set; }
    }
    public class ListBusinessLayer
    {
        public List<BusinessLayerBO> BusinessLayer { get; set; }
    }
    public class BusinessLayerBO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_VIS_Objetive { get; set; }
        public string U_VIS_VariableType { get; set; }
        public string U_VIS_Variable { get; set; }
        public string U_VIS_Trigger { get; set; }
        public string U_VIS_TriggerType { get; set; }
        public string U_VIS_Active { get; set; }
        public string U_VIS_ValidFrom { get; set; }
        public string U_VIS_ValidUntil { get; set; }
        public List<BusinessLayerDetailBO> Detail { get; set; }
    }
    public class BusinessLayerDetailBO
    {
        public string LineId { get; set; }
        public string U_VIS_Action { get; set; }
        public string U_VIS_Object { get; set; }
        public string U_VIS_Type { get; set; }
    }

}
