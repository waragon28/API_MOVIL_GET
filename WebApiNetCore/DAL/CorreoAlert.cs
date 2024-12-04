using System.Net;

namespace WebApiNetCore.DAL
{
    public class CorreoAlert
    {

        public  void EnviarCorreoOffice365(string Titulo, string Cuerpo)
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

    }
}
