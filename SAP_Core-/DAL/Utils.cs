#define PE
//#define EC
//#define BO
//#define PY
//#define CL
//#define PY_QA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP_Core.BO;
using Sap.Data.Hana;
using SAP_Core.Utils;
using System.Configuration;

namespace SAP_Core.DAL
{
    public class Utils
    {
        public static string bd()
        {
            string bd = "";
#if PE
                bd = "B1H_VIST_PE";
#elif EC
                bd = "B1H_VIST_EC";
#elif BO
                bd = "B1H_VIST_BO";
#elif PY
                bd = "B1H_VIST_PY";
#elif CL
                bd = "B1H_VIST_CL";
#elif PY_QA
                bd = "B1H_VIST_PY";

#endif
            return bd;
        }
    }
}

