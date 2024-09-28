using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.Utils
{
    public class ShortUrl
    {
        public static string CompressURL(string strURL)
        {
            WebRequest objRequest = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + strURL);
            string strResponse = null;

            try
            {
                HttpWebResponse objResponse = objRequest.GetResponse() as HttpWebResponse;
                StreamReader stmReader = new StreamReader(objResponse.GetResponseStream());

                strResponse = stmReader.ReadToEnd();
            }
            catch { }
            return strResponse;
        }
    }
}
