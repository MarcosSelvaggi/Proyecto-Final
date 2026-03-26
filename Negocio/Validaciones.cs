using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Negocio
{
    public class Validaciones
    {
        public static bool ValidarEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool ValidarPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d).{8,}$");
        }

        public static bool ValidarTelefono(string telefono)
        {
            return Regex.IsMatch(telefono, @"^\d{10}$");
        }

        public static bool ValidarEmailExiste(string email)
        {
            BD conexion = new BD();
            return !conexion.EmailExiste(email);
        }

        public static bool ValidarTelefonoExiste(string telefono)
        {
            BD conexion = new BD();
            return !conexion.TelefonoExiste(telefono);
        }

    }

  
    
}
