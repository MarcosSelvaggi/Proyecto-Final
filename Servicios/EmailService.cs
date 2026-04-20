using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Servicios
{
    public class EmailService
    {
        public bool EnviarMail(string MailParaRecuperar, int ClaveParaRecuperar)
        {
            var Message = new MimeMessage();

            var FromEmail = new MailboxAddress("Fixnet", ConfigurationManager.AppSettings["Email"]);
            var ToMail = new MailboxAddress("MailUsuario", MailParaRecuperar);

            Message.From.Add(FromEmail);
            Message.To.Add(ToMail);

            Message.Subject = "Clave para recuperar tu cuenta";
            Message.Body = new TextPart(TextFormat.Html)
            {
                Text = "Hola,  <br>" +
                "La clave para recuperar tu cuenta es: " + ClaveParaRecuperar + "<br>" +
                "De parte del equipo de Fixnet." 
            };//Si subimos el logo online podemos ponerlo en el cuerpo del mail con un <img src=.../> 

            using var Smpt = new SmtpClient();

            try
            {
                Smpt.Connect("smtp.gmail.com", 465, true);
                Smpt.Authenticate(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["Password"]); 
                Smpt.Send(Message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Smpt.Disconnect(true);
                Smpt.Dispose();
            }
        }
    }
}
