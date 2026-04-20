using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;

namespace Negocio
{
    public class EmailManager
    {
        public bool EnviarCodigo(string EmailUsuario, int CodigoGenerado)
        {
            EmailService EmailService = new EmailService(); 
            return EmailService.EnviarMail(EmailUsuario, CodigoGenerado);
        }
    }
}
