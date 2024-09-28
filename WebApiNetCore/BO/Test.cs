using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{
    public class BoM
    {
        public string TreeCode { get; set; }
        public List<BoMDetail> ProductTreeLines { get; set; }
    }
    public class BoMDetail
    {
        public int ChildNum { get; set; }
        public decimal Quantity { get; set; }
    }
    public class ListBoM
    {
        public List<BoM> lbom { get; set; }
    }
    public class Test
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string U_ValidFrom { get; set; } = "";
        public string U_ValidTo { get; set; } = "";
        public decimal U_Amount { get; set; } = 0;
        public string U_SessionID { get; set; } = "";
        public string U_WindowsSession { get; set; } = "";
        public string U_IP { get; set; } = "";
        public string U_Host { get; set; } = "";
        public string U_UserSign { get; set; } = "";
        public string U_ShortCode { get; set; } = "";
        public string U_DocEntry { get; set; } = "";
    }
    public class ListTest
    {
        public List<Test> ltest { get; set; }
    }
    public class Geolocate
    {
        public string CardCode { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
    public class ListGeolocate
    {
        public List<Geolocate> geo { get; set; }
    }

}
