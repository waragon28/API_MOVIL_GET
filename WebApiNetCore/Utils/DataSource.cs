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
        public static string bd()
        {
            string bd = Startup.Configuration.GetValue<string>("ServiceLayer:PE:CompanyDB");
            return bd;
        }
    }
}

