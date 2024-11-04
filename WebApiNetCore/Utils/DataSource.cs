//#define PE
//#define PE_QA
//#define ROFALAB
//#define ES
//#define MA
//#define EC
//#define EC_QA
//#define BO
//#define BO_QA
//#define PY    
//#define CL
//#define PY_QA
//#define IN

using Microsoft.Extensions.Configuration;
using WebApiNetCore;

namespace SAP_Core.Utils
{
    public class DataSource
    {
        public enum Company { B1H_ROFA_PE, B1H_VIST_BO,B1H_VIST_BO_QA2, B1H_VIST_CL, B1H_VIST_PE_QA, B1H_VIST_PY_QA1, B1H_VIST_PY }
        public static string bd()
        {
            string bd = Startup.Configuration.GetValue<string>("ServiceLayer:PE:CompanyDB");
            return bd;
        }
    }
}

