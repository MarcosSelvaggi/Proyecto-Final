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

        public bool EnviarMailAlPrestador(string NombrePrestador, string MailPrestador, string Mensaje)
        {
            bool BoolMensaje = false; 

            var Message = new MimeMessage();

            var FromEmail = new MailboxAddress("Fixnet", ConfigurationManager.AppSettings["Email"]);
            var ToMail = new MailboxAddress("MailUsuario", MailPrestador);

            Message.From.Add(FromEmail);
            Message.To.Add(ToMail);

            Message.Subject = "Solicitud de trabajo";
            Message.Body = new TextPart(TextFormat.Html)
            {
                Text = "<h3>Hola, " + NombrePrestador + "</h3>" +
                "<p>Uno de nuestros clientes tiene una nueva solicitud para ti.</p>" + 
                "<p>Puedes reviarla en la sección de Solicitudes de trabajo en la plataforma. <br>" + 
                "<strong>Mensaje del cliente</strong>" +
                "<p>" + Mensaje + "</p>"
            };

            using var Smpt = new SmtpClient();

            try
            {
                Smpt.Connect("smtp.gmail.com", 465, true);
                Smpt.Authenticate(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["Password"]);
                Smpt.Send(Message);
                BoolMensaje = true;
            }
            catch (Exception)
            {
                BoolMensaje = false;
            }
            finally
            {
                Smpt.Disconnect(true);
                Smpt.Dispose();
            }

            return BoolMensaje; 
        }
        public bool EnviarMailAlCliente(string NombreCliente, string NombrePrestador, string MailCliente)
        {
            bool BoolMensaje = false; 

            var Message = new MimeMessage();

            var FromEmail = new MailboxAddress("Fixnet", ConfigurationManager.AppSettings["Email"]);
            var ToMail = new MailboxAddress("MailUsuario", MailCliente);

            Message.From.Add(FromEmail);
            Message.To.Add(ToMail);

            Message.Subject = "Solicitud de trabajo";
            Message.Body = new TextPart(TextFormat.Html)
            {
                Text = "<h3>Hola, " + NombreCliente + "</h3>" +
                "<strong> " + NombrePrestador + "</strong> <p> ha aceptado tu solicitud de trabajo en la plataforma y en breve se pondrá en contacto contigo.</p>" + 
                "<p>Puedes reviarla en la sección de Solicitudes en la plataforma."
            };

            using var Smpt = new SmtpClient();

            try
            {
                Smpt.Connect("smtp.gmail.com", 465, true);
                Smpt.Authenticate(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["Password"]);
                Smpt.Send(Message);
                BoolMensaje = true;
            }
            catch (Exception)
            {
                BoolMensaje = false;
            }
            finally
            {
                Smpt.Disconnect(true);
                Smpt.Dispose();
            }

            return BoolMensaje; 
        }
    }
}
