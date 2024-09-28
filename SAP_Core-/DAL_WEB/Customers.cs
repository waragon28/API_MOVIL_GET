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
    public class CustomersDAL : Connection
    {
        public Customer getCustomer(string pais, string email)
        {
            HanaDataReader reader;
            HanaConnection connection;
            Customer c = new Customer();
            List<Customer_Header_BO> cs = new List<Customer_Header_BO>();
            string strSQL = string.Empty;
            string bd = string.Empty;
            switch (pais)
            {
                case "PE":
                    bd = "B1H_VIST_QA";
                    break;
            }


            strSQL = string.Format("CALL {0}.WEB_CUSTOMER ('{1}')", bd, email);
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
                        Customer_Header_BO customer = new Customer_Header_BO();
                        AddressDAL ad = new AddressDAL();
                        customer.token = "_";
                        customer.CardCode = reader["CardCode"].ToString();
                        customer.CardName = reader["CardName"].ToString();
                        customer.DocType = reader["DocType"].ToString();
                        customer.LicTradNum = reader["LicTradNum"].ToString();
                        customer.Phone = reader["Phone"].ToString();
                        customer.Cellphone = reader["Cellphone"].ToString();
                        customer.Email = reader["Email"].ToString();
                        customer.Balance = Convert.ToDecimal(reader["Balance"]);
                        customer.OrdersBal = Convert.ToDecimal(reader["OrdersBal"]);
                        customer.CreditLine = Convert.ToDecimal(reader["CreditLine"]);
                        customer.PymntGroup = reader["PymntGroup"].ToString();
                        customer.Category = reader["Category"].ToString();
                        customer.SalesCategory = reader["SalesCategory"].ToString();
                        customer.PunishedClient = reader["PunishedClient"].ToString();
                        customer.Image = "/Documents/Logo.jpg";
                        customer.Addresses = ad.getAddress(pais, customer.CardCode);
                        cs.Add(customer);
                    }
                }
                c.Customers = cs;
            }
            catch (Exception ex)
            {
               
            }
            return c;

        }

    }
}
