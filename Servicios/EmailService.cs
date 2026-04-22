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
using static System.Net.Mime.MediaTypeNames;

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
                Text = @"
                <div style='font-family: Arial, sans-serif; color: #333;'>

                    <div style='text-align:center; margin-bottom:20px;'>
                        <img src='https://i.ibb.co/QvY9Kgwt/logo.png' alt='Logo' style='max-width:150px;' />
                    </div>

                    <h2 style='color:#2c3e50;'>Hola</h2>

                    <p>Recibimos una solicitud para recuperar tu cuenta.</p>

                    <p>Tu clave de recuperación es:</p>

                    <div style='background:#f4f6f8; padding:15px; border-radius:5px; text-align:center; font-size:18px; font-weight:bold; letter-spacing:2px;'>
                        " + ClaveParaRecuperar + @"
                    </div>

                    <br/>

                    <p>Ingresá esta clave en la plataforma para continuar con el proceso.</p>

                    <hr style='border:none; border-top:1px solid #eee; margin:20px 0;' />

                    <p style='font-size:12px; color:#888;'>
                        Este es un mensaje automático del equipo de Fixnet. No respondas este correo.
                    </p>

                </div>"
            };

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
                Text = @"
                <div style='font-family: Arial, sans-serif; color: #333;'>

                    <div style='text-align:center; margin-bottom:20px;'>
                        <img src='https://i.ibb.co/QvY9Kgwt/logo.png' alt='Logo' style='max-width:150px;' />
                    </div>

                    <h2 style='color:#2c3e50;'>Hola, " + NombrePrestador + @"</h2>

                    <p>Uno de nuestros clientes tiene una <strong>nueva solicitud de trabajo</strong> para vos.</p>

                    <p>
                        Podés revisarla en la sección 
                        <strong>Turnos solicitados</strong> dentro de la plataforma.
                    </p>

                    <hr style='border:none; border-top:1px solid #eee; margin:20px 0;' />

                    <p><strong>Mensaje del cliente:</strong></p>

                    <div style='background:#f9f9f9; padding:15px; border-radius:5px;'>
                        " + Mensaje + @"
                    </div>

                    <br/>

                    <p style='font-size:12px; color:#888;'>
                        Este es un mensaje automático, por favor no responder a este email.
                    </p>

                </div>"
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
        public bool EnviarRechazoMailAlCliente(string NombreCliente, string NombrePrestador, string MailCliente)
        {
            bool BoolMensaje = false;

            var Message = new MimeMessage();

            var FromEmail = new MailboxAddress("Fixnet", ConfigurationManager.AppSettings["Email"]);
            var ToMail = new MailboxAddress("MailUsuario", MailCliente);

            Message.From.Add(FromEmail);
            Message.To.Add(ToMail);

            Message.Subject = "Actualización sobre tu solicitud de trabajo";
            Message.Body = new TextPart(TextFormat.Html)
        {
            Text = @"
            <div style='font-family: Arial, sans-serif; color: #333;'>

                <div style='text-align:center; margin-bottom:20px;'>
                    <img src='https://i.ibb.co/QvY9Kgwt/logo.png' alt='Logo' style='max-width:150px;' />
                </div>

                <h2 style='color:#2c3e50;'>Hola, " + NombreCliente + @"</h2>

                <p>
                    Lamentamos informarte que <strong>" + NombrePrestador + @"</strong> ha rechazado tu 
                    <strong>solicitud de trabajo</strong>.
                </p>

                <p>
                    Podés volver a publicar tu solicitud o buscar otro prestador disponible en la plataforma.
                </p>

                <p>
                    Te recomendamos revisar otras opciones en la sección 
                    <strong>Buscar Prestadores</strong>.
                </p>

                <hr style='border:none; border-top:1px solid #eee; margin:20px 0;' />

                <p style='font-size:12px; color:#888;'>
                    Este es un mensaje automático del equipo de Fixnet. No respondas este correo.
                </p>

            </div>"
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

            Message.Subject = "Actualización sobre tu solicitud de trabajo";
            Message.Body = new TextPart(TextFormat.Html)
            {
                Text = @"
                <div style='font-family: Arial, sans-serif; color: #333;'>

                    <div style='text-align:center; margin-bottom:20px;'>
                        <img src='https://i.ibb.co/QvY9Kgwt/logo.png' alt='Logo' style='max-width:150px;' />
                    </div>

                    <h2 style='color:#2c3e50;'>Hola, " + NombreCliente + @"</h2>

                    <p>
                        <strong>" + NombrePrestador + @"</strong> ha aceptado tu 
                        <strong>solicitud de trabajo</strong>.
                    </p>

                    <p>
                        En breve se pondrá en contacto con vos para coordinar los detalles.
                    </p>

                    <p>
                        Podés revisar el estado en la sección 
                        <strong>Mis Turnos</strong> dentro de la plataforma.
                    </p>

                    <hr style='border:none; border-top:1px solid #eee; margin:20px 0;' />

                    <p style='font-size:12px; color:#888;'>
                        Este es un mensaje automático del equipo de Fixnet. No respondas este correo.
                    </p>

                </div>"
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
