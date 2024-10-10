using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Sap.Data.Hana;
namespace SAP_Core.DAL
{
    public class Connection
    {
        public HanaConnection GetConnection ()
        {
           // HanaConnection connection = new HanaConnection(@"Server=ecs-dbs-vistony:30015;UserName=B1ADMIN;Password=635kSIgVn2Uh5Q117Mxf;");
            HanaConnection connection = new HanaConnection(@"Server=192.168.254.28:30015;UserName=B1ADMIN;Password=bHR#xFA@7Uo7;");
            //HanaConnection connection = new HanaConnection(@"Server="+ ConfigurationSettings.AppSettings["ServerHana2"] + ";UserName=" + ConfigurationSettings.AppSettings["UserHana"] + ";Password=" + ConfigurationSettings.AppSettings["PasswordHana"] + ";");

            return connection;
        }
    }// fin de la clase

}// fin de la clase
