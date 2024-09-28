using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SAP_Core.BO
{
    public class VisitBO
    {
        public string Code { get; set; }
        public string U_VIS_CardCode { get; set; }
        public string U_VIS_Address { get; set; }
        public string U_VIS_SlpCode { get; set; }
        public string U_VIS_UserID { get; set; }
        public string U_VIS_Date { get; set; }
        public string U_VIS_Hour { get; set; }
        public string U_VIS_Type { get; set; }
        public string U_VIS_Observation { get; set; }
        public string U_VIS_Longitude { get; set; }
        public string U_VIS_Latitude { get; set; }
        public string U_VIS_AppVersion { get; set; }
        public string U_VIS_Intent { get; set; }
        public string U_VIS_Brand { get; set; }
        public string U_VIS_Model { get; set; }
        public string U_VIS_Version { get; set; }
        public string U_VIS_MobileID { get; set; }
        public decimal U_VIS_Amount { get; set; }
        public string U_VIS_StatusRoute { get; set; }
        public string U_VIS_VisitID { get; set; }
        public string U_VIS_HoraText { get; set; }
        public string U_VIS_HourBefore { get; set; }
    }
    public class VisitsBO
    {
        public List<VisitBO> Visits { get; set; }
    }
    public class VisitBOs
    {
        public string Code { get; set; }
        public string CardCode { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Observation { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
    public class VisitResponse
    {
        public string IdVisit { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
    public class ListVisit
    {
        public List<VisitResponse> Visits { get; set; }
    }
}
