using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{
    public class SoliDevBO
    {
        public string DocEntryFact { get; set; }
        public string MotivoNotaCredito { get; set; }
    }

    public class SLD
    {
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string NumAtCard { get; set; }
        public string DocCurrency { get; set; }
        public string Comments { get; set; }
        public int PaymentGroupCode { get; set; }
        public int SalesPersonCode { get; set; }
        public int DocumentsOwner { get; set; }
        public int ContactPersonCode { get; set; }
        public string TaxDate { get; set; }
        public string DocObjectCode { get; set; }
        public string ShipToCode { get; set; }
        public string PayToCode { get; set; }
        public string U_SYP_MDMT { get; set; }
        public string U_SYP_MDTD { get; set; }
        public string U_SYP_MDSD { get; set; }
        public string U_SYP_MDCD { get; set; }
        public string U_SYP_STATUS { get; set; }
        public string U_SYP_DETPAGADO { get; set; }
        public string U_SYP_AUTODET { get; set; }
        public string U_SYP_NGUIA { get; set; }
        public string U_SYP_MDFN { get; set; }
        public string U_SYP_MDFC { get; set; }
        public string U_SYP_MDVN { get; set; }
        public string U_SYP_MDVC { get; set; }
        public string U_SYP_MDTS { get; set; }
        public string U_SYP_CONSIGNADOR { get; set; }
        public string U_SYP_GRFT { get; set; }
        public string U_SYP_TIPO_TRANSF { get; set; }
        public string U_SYP_CONMON { get; set; }
        public string U_SYP_ANTPEN { get; set; }
        public string U_SYP_DOCAPR { get; set; }
        public string U_SYP_OCTRA { get; set; }
        public string U_SYP_TIPEXP { get; set; }
        public string U_SYP_PNACT { get; set; }
        public string U_PZCreated { get; set; }
        public string U_SYP_INC { get; set; }
        public string U_SYP_CON_STOK { get; set; }
        public string U_SYP_PDTREV { get; set; }
        public string U_SYP_PDTCRE { get; set; }
        public string U_VIT_VENMOS { get; set; }
        public string U_VIST_PROMADIC { get; set; }
        public string U_VIST_SUCUSU { get; set; }
        public string U_VIST_APSOLV { get; set; }
        public string U_VIST_DIS { get; set; }
        public string U_SYP_TVENTA { get; set; }
        public string U_VIS_SalesOrderID { get; set; }
        public string U_VIS_CommentApproval { get; set; }
        public string U_VIS_OVCommentary { get; set; }
        public string U_VIS_EVCommentary { get; set; }
        public string U_VIS_INCommentary { get; set; }
        public string U_VIS_CompleteOV { get; set; }
        public string U_VIS_OVRejected { get; set; }
        public string U_VIS_ApprovedBy { get; set; }
        public string U_SYP_DT_CONSOL { get; set; }
        public string U_SYP_DT_FCONSOL { get; set; }
        public string U_SYP_DT_HCONSOL { get; set; }
        public string U_SYP_DT_CORRDES { get; set; }
        public string U_SYP_DT_FCDES { get; set; }
        public string U_SYP_DT_OCUR { get; set; }
        public string U_SYP_DT_AYUDANTE { get; set; }
        public string U_SYP_DT_ESTDES { get; set; }
        public string U_VIS_AgencyCode { get; set; }
        public string U_VIS_AgencyRUC { get; set; }
        public string U_VIS_AgencyName { get; set; }
        public string U_VIS_AgencyDir { get; set; }
        public string U_VIST_REVASIDSCTO { get; set; }
        public object U_VIS_AgencyUbigeo { get; set; }
        public string U_SYP_VIST_TG { get; set; }
        public string U_SYP_FEHASH { get; set; }
        public string U_SYP_FERESP { get; set; }
        public string U_SYP_FEEST { get; set; }
        public int U_SYP_FEESUNAT { get; set; }
        public string U_SYP_FECDR { get; set; }
        public string U_SYP_FEXML { get; set; }
        public string U_SYP_FETO { get; set; }
        public int U_SYP_FEEE { get; set; }
        public int U_SYP_FEMEX { get; set; }
        public string U_SYP_FEMB { get; set; }
        public int U_SYP_FEANT { get; set; }
        public int U_SYP_FEGIT { get; set; }
        public double U_SYP_FEGPB { get; set; }
        public string U_SYP_FEGMT { get; set; }
        public string U_SYP_FEGFI { get; set; }
        public string U_SYP_FEGFE { get; set; }
        public string U_VIS_TypeRequest { get; set; }
        public string U_VIS_TransferType { get; set; }
        public string U_VIS_MTDev { get; set; }
        public string U_VIS_MTReq { get; set; }
        public string U_LB_WITHCC { get; set; }
        public string U_VIS_AppVersion { get; set; }
        public double U_VIS_Intent { get; set; }
        public string U_VIS_Brand { get; set; }
        public string U_VIS_Model { get; set; }
        public string U_VIS_Version { get; set; }
        public string U_SYP_GUIARESU { get; set; }
        public string U_SYP_FEPDF { get; set; }
        public string U_CancelReason { get; set; }
        public string U_CancelReasonOV { get; set; }
        public string U_Transfered { get; set; }
        public string U_TaskMigrate { get; set; }
        public string U_VIS_Alert1 { get; set; }
        public string U_VIS_Alert2 { get; set; }
        public string U_VIS_Alert3 { get; set; }
        public string U_SYP_FEGUC { get; set; }
        public int U_VIS_Flete { get; set; }
        public string U_VIS_Draft1 { get; set; }
        public string U_SYP_FEEB { get; set; }
        public string U_SYP_FEGES { get; set; }
        public string U_SYP_FEGAF { get; set; }
        public string U_SYP_FEGRD { get; set; }
        public string U_Confirma_Pedido_Dup { get; set; }
        public string U_SYP_MOTND { get; set; }
        public string U_SYP_TPOND { get; set; }
        public List<DocumentLineSLD> DocumentLines { get; set; }
    }

    public class DocumentLineSLD
    {
        public int BaseType { get; set; }
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double DiscountPercent { get; set; }
        public string WarehouseCode { get; set; }
        public string AccountCode { get; set; }
        public string CostingCode { get; set; }
        public string TaxCode { get; set; }
        public string COGSCostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }
        public string COGSCostingCode2 { get; set; }
        public string COGSCostingCode3 { get; set; }
        public string U_SYP_HASPROV { get; set; }
        public string U_SYP_FECAT07 { get; set; }
        public string U_VIST_CTAINGDCTO { get; set; }
        public string U_VIS_PromID { get; set; }
        public string U_VIS_PromLineID { get; set; }
    }
}
