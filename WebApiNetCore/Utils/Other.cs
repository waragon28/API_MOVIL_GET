using Newtonsoft.Json;
using SalesForce.BO;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiNetCore.Utils
{
    public class Other
    {
        public class LoginRequest
        {
            [JsonPropertyName("CompanyDB")]
            public string CompanyDB { get; set; }
            [JsonPropertyName("Password")]
            public string Password { get; set; }
            [JsonPropertyName("UserName")]
            public string UserName { get; set; }
        }

        public class Response
        {
            public HttpStatusCode statusCode { get; set; }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
            public dynamic data { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        }

        public static string sqlDatoToJson(HanaDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            string JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }

        public enum Method { GET, POST, PATCH, DELETE };
        public static string ComputeMD5(string s)
        {
            StringBuilder sb = new StringBuilder();

            // Initialize a MD5 hash object
            using (MD5 md5 = MD5.Create())
            {
                // Compute the hash of the given string
                byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(s));

                // Convert the byte array to string format
                foreach (byte b in hashValue)
                {
                    sb.Append($"{b:X2}");
                }
            }

            return sb.ToString();
        }

        public static string GetStringValue(HanaDataReader reader, string columnName)
        {
          //  if (!ColumnExists(reader, columnName))
              //  return string.Empty;

            return reader[columnName] != DBNull.Value ? reader[columnName].ToString() : string.Empty;
        }

        public static decimal GetDecimalValue(HanaDataReader reader, string columnName)
        {
            //if (!ColumnExists(reader, columnName))
              //  return 0;
            return reader[columnName] != DBNull.Value ? Convert.ToDecimal(reader[columnName]) : 0m;
        }
        public static double GetDoubleValue(HanaDataReader reader, string columnName)
        {
            //if (!ColumnExists(reader, columnName))
            //   return 0;
            double xd = Convert.ToDouble(reader[columnName]);

            //return reader[columnName] != DBNull.Value ? Convert.ToDouble(reader[columnName]) : 0;
            return  Convert.ToDouble(reader[columnName].ToString());
        }
        public static int GetIntValue(HanaDataReader reader, string columnName)
        {
            //if (!ColumnExists(reader, columnName))
               // return 0;
            return reader[columnName] != DBNull.Value ? Convert.ToInt32(reader[columnName]) : 0;
        }
        public static T GetJsonValue<T>(HanaDataReader reader, string columnName) where T : class
        {
            if (reader[columnName] == DBNull.Value)
            {
                return null;
            }

            // Verificar si la columna existe
            if (!ColumnExists(reader, columnName))
                return null;

            var json = reader[columnName].ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void EnviarCorreoOffice365(string Titulo, string Cuerpo)
        {
            //Conexión a a la Plataforma de Microsofot Office 365 para enviar correo.
            var smtp = new System.Net.Mail.SmtpClient("smtp.office365.com");
            var mail = new System.Net.Mail.MailMessage();
            string userFrom = "notificaciones@nogasa.com.pe"; //Mi cuenta de Office 365.
                                                              // IMPORTANTE : Este Usuario mail.From, debe coincidir con el de NetworkCredential(), sino se genera error.
            mail.From = new System.Net.Mail.MailAddress(userFrom);


            mail.To.Add("william.aragon@nogasa.com.pe");
            //mail.To.Add("ronald.otarola@nogasa.com.pe");
            mail.Subject = Titulo;
            mail.Body = Cuerpo;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            //Credenciales que se utilizan, cuando se autentica al correo de Office 365.
            smtp.Credentials = new System.Net.NetworkCredential(userFrom, "Vistony2020**");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            smtp.Send(mail);
        }

        public static bool ColumnExists(IDataReader reader, string columnName)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader), "El lector de datos no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("El nombre de la columna no puede ser nulo o vacío.", nameof(columnName));

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }


    }

}
