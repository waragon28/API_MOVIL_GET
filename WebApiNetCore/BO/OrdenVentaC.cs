using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class OrdenVentaC
    {
        public string DocNum { get; set; }
        public string Object { get; set; }
        public string CardCode { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }

        [Display(Name = "Monto")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DocTotal { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalCommentary { get; set; }
        public string SalesOrderID { get; set; }
        public string SlpCode { get; set; }
    }
    public class ListaOrdenVentaC
    {
        public List<OrdenVentaC> SalesOrder { get; set; }
    }
    public class OrdenVentaBO
    {
        public string json { get; set; }
    }

    public class Coti_To_OV
    {
        /*  public string CardCode { get; set; }
          public object U_VIS_TipTransGrat { get; set; }
          public string U_VIS_CompleteOV { get; set; }
          public string U_VIT_VENMOS { get; set; }
          public string U_VIS_Flete { get; set; }
          public string Comments { get; set; }
          public string DocCurrency { get; set; }
          public string DocDate { get; set; }
          public string DocDueDate { get; set; }
          public string DocType { get; set; }
          public string DocumentsOwner { get; set; }
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
          public object U_VIS_OVCommentary { get; set; }
          public string U_VIS_SalesOrderID { get; set; }
          public string U_SYP_MDMT { get; set; }
          public string U_SYP_TVENTA { get; set; }
          public string DocObjectCode { get; set; }
          public string U_SYP_DOCEXPORT { get; set; }
          public string U_SYP_FEEST { get; set; }
          public string U_SYP_FEMEX { get; set; }
          public string U_SYP_FETO { get; set; }
          public string U_VIS_Intent { get; set; }
          public string U_VIS_Brand { get; set; }
          public string U_VIS_Model { get; set; }
          public string U_VIS_Version { get; set; }
          public string U_VIS_EVCommentary { get; set; }
          public string U_VIS_INCommentary { get; set; }
          public string U_VIS_DiscountPercent { get; set; }
          public string U_VIS_ReasonDiscountPercent { get; set; }
          public string U_VIS_DeliveryDateOptional { get; set; }
          public string U_SYP_PDTREV { get; set; }
          public string U_ID_Document { get; set; }
          public string U_Confirma_Pedido_Dup { get; set; }
          public string U_SYP_PDTCRE { get; set; }
          public List<DocumentLine> DocumentLines { get; set; }*/
        public string DocType { get; set; }
        public string Comments { get; set; }
        public string DocDate { get; set; }
        public object DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string DocCurrency { get; set; }
        public int PaymentGroupCode { get; set; }
        public int SalesPersonCode { get; set; }
        public int DocumentsOwner { get; set; }
        public string TaxDate { get; set; }
        public string DocObjectCode { get; set; }
        public string ShipToCode { get; set; }
        public string PayToCode { get; set; }
        public string U_VIST_SUCUSU { get; set; }
        public string U_VIS_AgencyCode { get; set; }
        public string U_VIS_AgencyRUC { get; set; }
        public string U_VIS_AgencyName { get; set; }
        public string U_VIS_AgencyDir { get; set; }
        public string U_VIS_SalesOrderID { get; set; }
        public string U_SYP_TVENTA { get; set; }
        public double U_VIS_Intent { get; set; }
        public string U_VIS_Brand { get; set; }
        public string U_VIS_Model { get; set; }
        public string U_VIS_Version { get; set; }

        public string U_CODCTRL { get; set; }
        public string U_SERIECOD { get; set; }
        public string U_NROAUTOR { get; set; }
        public string U_NIT { get; set; }
        public string U_LB_WITHCC { get; set; }
        public int U_TIPODOC { get; set; }
        public int U_TIPOCOM { get; set; }
        public string U_RAZSOC { get; set; }
        public string U_CODFORPI { get; set; }
        public string U_NROTRAM { get; set; }
        public string U_NROPOL { get; set; }
        public string U_BOLBSP { get; set; }
        public double U_ICE { get; set; }
        public double U_EXENTO { get; set; }
        public string U_ESTADOFC { get; set; }
        public string U_FACANU { get; set; }
        public double U_TASACERO { get; set; }
        public string U_SO1_01RETAILONE { get; set; }
        public string U_ORIGIN { get; set; }
        public string U_SYP_TPOENTME { get; set; }
        public string U_SYP_TPOSALME { get; set; }
        public string U_VIS_AppVersion { get; set; }
        public string U_B_State { get; set; }
        public string U_B_type { get; set; }
        public string U_B_invtype { get; set; }
        public string U_B_invalidltn { get; set; }
        public string U_TIPOVENTA { get; set; }
        public string U_Status { get; set; }
        public string U_AproCoti { get; set; }
        
        public List<DocumentLine> DocumentLines { get; set; }
    }


    public class DocumentLine
    {
        public string AcctCode { get; set; }
        public string COGSAccountCode { get; set; }
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
        public string U_VIS_PromID { get; set; }
        public string U_VIS_PromLineID { get; set; }
        public int BaseType { get; set; }        
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
}

}
