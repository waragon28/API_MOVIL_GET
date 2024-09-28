using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class AnalysisRouteBO
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int ShipToCode { get; set; }
        public string Street { get; set; }
        public string TerritoryID { get; set; }
        public string Territory { get; set; }
        public string Day { get; set; }
        public string CommercialClass { get; set; }
        public double GallonCurrentYearCurrentPeriod { get; set; }
        public double GallonCurrentYearPreviousPeriod { get; set; }
        public double GallonCurrentYearSecondPriorPeriod { get; set; }
        public double GallonPreviousYearCurrentPeriod { get; set; }
        public double GallonPreviousYearPreviousPeriod { get; set; }
        public double GallonPreviousYearSecondPreviousPeriod { get; set; }
        public double AverageQuarterCurrentYear { get; set; }
        public double AverageQuarterPreviousYear { get; set; }
        public double Indicator1 { get; set; }
        public double Indicator2 { get; set; }
        public double Quota { get; set; }
        public double Indicator3 { get; set; }
    }
    public class ListAnalysisRoute
    {
        public List<AnalysisRouteBO> AnalysisRoutes { get; set; }
    }
}
