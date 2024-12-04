using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class CountDispatchSuccess
    {
        public double CountSuccess { get; set; }
        public double CountFailed { get; set; }
   
    }

    public class DespachoHeaderBO
    {
        public string Assistant { get; set; }
        public string ControlCode { get; set; }
        public string Brand { get; set; }
        public decimal OverallWeight { get; set; }
        public string LicensePlate { get; set; }
        public Object Detail { get; set; }
    }
    public class ListaDespachoHeaderBO
    {
        public List<DespachoHeaderBO> Obtener_DespachoCResult { get; set; }
    }
    public class DetailDespacho
    {
        public string Item { get; set; }
        public string CardCode { get; set; }
        public string ShipToCode { get; set; }
        public string Address { get; set; }
        public string InvoiceNum { get; set; }
        public string DeliveryNum { get; set; }
        public string DeliveryLegalNumber { get; set; }
        public string InvoiceLegalNumber { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string SlpCode { get; set; }
        public string SlpName { get; set; }
        public string PymntGroup { get; set; }
        public decimal Weight { get; set; }
    }

    /*public class DispatchList
    {
        [Required]
        public List<Dispatch> Dispatch { get; set; }
    }*/


    public class InDispatchList
    {
        public InDispatch[] Dispatch { get; set; }
    }

    public class InDispatch
    {
        [Required]
        public int DocEntry { get; set; }

        [Required]
        public InDetail[] Details { get; set; }
    }

    public class InDetail
    {
        [Required]
        public int LineId { get; set; }
        public string Comments { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string PhotoDocument { get; set; }
        public string PhotoStore { get; set; }
        public string PersonContact { get; set; }
        public string ReturnReason { get; set; }
        public string ReturnReasonText { get; set; }
        public string DeliveryNotes { get; set; }
        public string Delivered { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    /*
    public class Dispatch
    {
        [Required]
        public int DocEntry { get; set; }
        [Required]
        public int LineId { get; set; }
        public string Comments { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string PhotoDocument { get; set; }
        public string PhotoStore { get; set; }
        public string PersonContact { get; set; }
        public string PaymentCondition { get; set; }
        public string ReturnReason { get; set; }
        public string Delivered { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
    */

    public class VIS_DIS_Drt1
    {
        public List<VIS_DIS_Drt1collection> VIS_DIS_DRT1Collection { get; set; }
    }
    public class VIS_DIS_Drt1collection
    {
        public int DocEntry { get; set; }
        public int LineId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_Comments { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_DocEntry { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_CheckInTime { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_CheckOutTime { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_PhotoDocument { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_PhotoStore { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_PersonContact { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_ReturnReason { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_U_ReturnReasonText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_Delivered { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_UserCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_UserName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_Latitude { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string U_Longitude { get; set; }
    }

    public class DispatchResponseList
    {
        public List<DispatchResponse> Dispatch { get; set; }
    }
    public class DispatchResponse
    {
        public int DocEntry { get; set; }
        public List<DispatchResponseDetalle> Details { get; set; }
    }

    public class DispatchResponseDetalle
    {
        public int LineId { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
    }


    ////////////////


    public class RootDispatch
    {
        public List<Dispatch_SAP> Dispatch { get; set; }
    }

    public class Dispatch_SAP
    {
        public string DriverCode { get; set; }
        public string DriverMobile { get; set; }
        public string AssistantCode { get; set; }
        public string VehicleCode { get; set; }
        public string VehiclePlate { get; set; }
        public string DriverName { get; set; }
        public string AssistantName { get; set; }
        public int DocEntry { get; set; }
        public string Brand { get; set; }
        public float OverallWeight { get; set; }
        public decimal TotalDocument { get; set; }
        public dynamic Details { get; set; }
    }

    public class Detail
    {
        public string Address2 { get; set; }
        public double Balance { get; set; }
        public string CardCode { get; set; }
        public string DeliveryLegalNumber { get; set; }
        public string DeliveryNum { get; set; }

        public int DocNumSLD { get; set; } //QA
        public string InvoiceLegalNumber { get; set; }
        public string InvoiceNum { get; set; }
        public int Item { get; set; }
        public string StatusCode { get; set; }
        public string Comments { get; set; }
        public string OcurrencyCode { get; set; }
        public string PhotoDocument { get; set; }
        public string PhotoStore { get; set; }
        public string PymntGroup { get; set; }
        public string ShipToCode { get; set; }
        public string SlpCode { get; set; }
        public string SlpName { get; set; }
        public string Status { get; set; }
        public string Weight { get; set; }
    }

    
}
