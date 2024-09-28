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
        public List<ApprovalBO> Document_ApprovalRequests { get; set; }
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
        public List<ApprovalBO> Document_ApprovalRequests { get; set; }

    }
    public class SalesOrdersPE
    {
        public List<SalesOrderPE> SalesOrders { get; set; }
    }
    public class ApprovalBO
    {
        public string ApprovalTemplatesID { get; set; }
        public string Remarks { get; set; }
    }
    public class ApprovalsBO
    {
        public List<ApprovalBO> Document_ApprovalRequests { get; set; }
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
}
