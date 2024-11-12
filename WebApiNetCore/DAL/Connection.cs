using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Sap.Data.Hana;
using WebApiNetCore;
using Microsoft.Extensions.Configuration;
namespace SAP_Core.DAL
{
    public class Connection
    {
        public HanaConnection GetConnection ()
        {
            HanaConnection connection = new HanaConnection(@"Server="+ Startup.Configuration.GetValue<string>("Hana:Conection") + ";UserName=" + 
                Startup.Configuration.GetValue<string>("Hana:UserHana") + ";Password=" + Startup.Configuration.GetValue<string>("Hana:PasswordHana") + 
                ";Max Pool Size=500;Connection Lifetime=500;");

            return connection;
        }
    }// fin de la clase

}// fin de la clase
