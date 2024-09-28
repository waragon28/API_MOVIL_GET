using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{
    public class SalesCalendarBO
    {
        public int Code { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Habil { get; set; }
    }
    public class SalesCalendars
    {
        public List<SalesCalendarBO> SalesCalendar { get; set; }
    }
    public class ServiceBO
    {
        public string Description { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int Interval { get; set; }
        public string Status { get; set; }
        public int DocEntry { get; set; }
    }
    public class Services
    {
        public List<ServiceBO> Service { get; set; }
    }
    public class GetQuotation
    {
        public string Imei { get; set; }
        public List<int> DocEntry { get; set; }
    }
    public class QuotationNotifications
    {
        public List<QuotationNotification> Quotation { get; set; }
    }
    public class QuotationNotification
    {
        public string Object { get; set; }
        public string Cliente_ID { get; set; }
        public string NombreCliente { get; set; }
        public string RucDni { get; set; }
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string DomEmbarque_ID { get; set; }
        public string DomEmbarque { get; set; }
        public string CANCELED { get; set; }
        public string MontoTotalOrden { get; set; }
        public string EstadoAprobacion { get; set; }
        public string OrdenVenta_ID { get; set; }
        public string SlpCode { get; set; }
        public string PymntGroup { get; set; }
    }
}
