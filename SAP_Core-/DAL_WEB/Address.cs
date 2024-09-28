using Sap.Data.Hana;
using SAP_Core.BO_WEB;
using SAP_Core.DAL;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.DAL_WEB
{
    public class AddressDAL : Connection
    {
        public List<Customer_Address> getAddress(string pais, string CardCode)
        {
            HanaDataReader reader;
            HanaConnection connection;
            List<Customer_Address> ca = new List<Customer_Address>();

            string strSQL = string.Empty;
            string bd = string.Empty;
            switch (pais)
            {
                case "PE":
                    bd = "B1H_VIST_QA";
                    break;
            }

            strSQL = string.Format("CALL {0}.WEB_CUSTOMER_ADDRESS ('{1}')", SAP_Core.DAL.Utils.bd(), CardCode);
            connection = GetConnection();

            try
            {
                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Customer_Address c = new Customer_Address();
                        c.AddressType = reader["AddressType"].ToString();
                        c.Address = reader["Address"].ToString();
                        c.Street = reader["Street"].ToString();
                        c.Block = reader["Block"].ToString();
                        c.City = reader["City"].ToString();
                        c.County = reader["County"].ToString();
                        c.SlpName = reader["SlpName"].ToString();
                        ca.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return ca;

        }
    }
}
