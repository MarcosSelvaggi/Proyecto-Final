using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
            return Regex.IsMatch(telefono, @"^\d{8}$");
        }
    }
}
