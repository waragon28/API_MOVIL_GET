using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SAP_Core.BO
{
    public class ListDocumentosBO
    {
        public List<DocumentosBO> Data { get; set; }
    }
    public class ListStatusAprobadores
    {
        public string Aprobador { get; set; }
        public string Estado { get; set; }
        public string Comentario { get; set; }

    }

    public class DocumentosBO
    {
        public string ID { get; set; }
        public string CarCode { get; set; }
        public string CardName { get; set; }
        public string DocDate { get; set; }
        public string DocTime { get; set; }
        public double DocTotal { get; set; }
        public string TypeDocument { get; set; }
        public string DocNumBorrador { get; set; }
        public string ConditionalPayment { get; set; }
        public string SalesPerson { get; set; }
        public double MargenDocumento { get; set; }
        public string DocEntry { get; set; }
        public int CantidadAnexo { get; set; }
        public int MargenGanancia { get; set; }
    }

    public class OPriceHistoy
    {
        public List<PriceHistoy> Data { get; set; }
    }


    public class PriceHistoy
    {
        public string CardName { get; set; }
        public double Price { get; set; }
        public string DocDate { get; set; }
    }

    public class LstAnexo
    {
        public List<Anexo> Data { get; set; }
    }
    public class Anexo
    {
        public string Documento { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        

    }
    public class Filtro
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
    }

    public class AprobacionBo
    {
        public string Codigo { get; set; }
        public string CodRegla { get; set; }
        public string Etapa { get; set; }
        public string Modelo { get; set; }
        public string Autorizador { get; set; }
        public string Decisión { get; set; }
        public string Comment { get; set; }
        public string DecBKP { get; set; }
    }

    public class ListAprovacionBo
    {
        public List<AprobacionBo> Data { get; set; }
    }

    public class PedidoDetalleBo
    {
        public string numArticle { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string OnHand { get; set; }
        public string PriceUnit { get; set; }
        public string DescontPercent { get; set; }
        public string LineTotal { get; set; }
        public string issetImpuesto { get; set; }
        public string CodImpuesto { get; set; }
        public string Warehouse { get; set; }
        public string MarginGain { get; set; }
        public string USUARIO_ID { get; set; }
        public string USUARIO { get; set; }
        public double PriceHistory { get; set; }
        public double PriceReference { get; set; }
        public double PorcUltComExc { get; set; }
    }

    public class ListPedidoDetalleBo
    {
        public List<PedidoDetalleBo> Data { get; set; }
    }

    public class PedidoBo
    {
        public string EntryDraft { get; set; }
        public string Pedido { get; set; }
        public string DocDate { get; set; }
        public string DocTime { get; set; }
        public string MargenGanancia { get; set; }
        public string PaymentTerminal { get; set; }
        public string Moneda { get; set; }
        public string DocTotal { get; set; }
        public string SalesPerson { get; set; }
        public string Coment { get; set; }
        public string DocType { get; set; }
    }

    public class ListPedidoBo
    {
        public List<PedidoBo> Data { get; set; }
    }

    public class ClienteBo
    {
        public string PEDocumentID { get; set; }
        public string Name { get; set; }
        public string RUC { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string CodVendedor { get; set; }
        public string NomVendedor { get; set; }
        public string Supervisor { get; set; }
        public string AnalistaCreditos { get; set; }
        public string SectoristaVenta { get; set; }
        public string nomudn { get; set; }
        public string Prioridad { get; set; }
        public string Castigado { get; set; }
        public List<PedidoBo> Pedidos { get; set; }
    }
    public class ListClienteBo
    {
        public List<ClienteBo> Data { get; set; }
    }

    public class ListUpdateB2B_Approval
    {
        public List<UpdateB2B_Approval> Data { get; set; }
    }
    public class UpdateB2B_Approval
    {
        public string LineNum { get; set; }
        public string Desicion { get; set; }
    }

    public class LineaBo
    {
        public string Deuda { get; set; }
        public string Entregas { get; set; }
        public string Pedido { get; set; }
        public string Oportunidades { get; set; }
        public string Cheques { get; set; }
        public string UltPago { get; set; }
        public string UltPagDoc { get; set; }
        public string UltFact { get; set; }
        public string UltFacDoc { get; set; }
        public string SumaProtesto { get; set; }
        public string LineaCredito { get; set; }
        public string LineaComprometida { get; set; }
        public string SaldoLinea { get; set; }
    }

    public class ListLineaBo
    {
        public List<LineaBo> Data { get; set; }
    }
    public class DeudaBo
    {
        public string Deuda_Corriente { get; set; }
        public string DIAS_1_8 { get; set; }
        public string DIAS_9_15 { get; set; }
        public string DIAS_16_30 { get; set; }
        public string DIAS_31_60 { get; set; }
        public string DIAS_61_90 { get; set; }
        public string DIAS_91_120 { get; set; }
        public string MAYOR_120_DIAS { get; set; }
        public string Deuda { get; set; }
        public string LineaCredito { get; set; }
        public string LineaComprometida { get; set; }
        public string SaldoLinea { get; set; }
        public string Deuda_Vencida { get; set; }
    }

    public class ListDeudaBo
    {
        public List<DeudaBo> Data { get; set; }
    }

    public  class ApprovalBo 
    {
        public string ID { get; set; }
        public string CarCode { get; set; }
        public string CardName { get; set; }
        public string DocDate { get; set; }
        public string DocTime { get; set; }
        public string DocTotal { get; set; }
        public string TypeDocument { get; set; }
        public string DocNumBorrador { get; set; }
        public string ConditionalPayment { get; set; }
        public string SalesPerson { get; set; }
        public string MargenDocumento { get; set; }
        public string DocEntry { get; set; }
    }

    public class ListApprovalBo
    {
        public List<ApprovalBo> Data { get; set; }
    }


}
