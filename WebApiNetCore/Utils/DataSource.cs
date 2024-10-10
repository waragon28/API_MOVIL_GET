//#define PE
#define PE_QA
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
#if PE
                bd = "B1H_VIST_PE";
#elif ROFALAB
            bd = Company.B1H_ROFA_PE.ToString();
#elif PE_QA
            bd = Company.B1H_VIST_PE_QA.ToString();
#elif ES
        bd="B1H_VIST_ES";
#elif MA
        bd="B1H_VIST_MA_QA";
#elif EC
        bd = "B1H_VIST_EC";
#elif EC_QA
        bd = "B1H_VIST_EC_QA";
#elif BO
                bd = Company.B1H_VIST_BO.ToString();
#elif BO_QA
                bd = Company.B1H_VIST_BO_QA2.ToString();
#elif PY
            bd = Company.B1H_VIST_PY.ToString(); 
#elif CL
                bd = Company.B1H_VIST_CL.ToString();
#elif PY_QA
                bd = "B1H_VIST_PY_QA";
#elif IN
            bd = "B1H_VIST_IN";

#endif
            return bd;
        }
    }
}

