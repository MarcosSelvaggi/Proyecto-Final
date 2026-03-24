using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Servicios; 

namespace Negocio
{
    public class UsuarioManager
    {
        Usuario Usuario; 

        public int RegistrarUsuario(Usuario Usuario)
        {
            //No es necesario crear una clase adicional para hashear la contraseña si usamos Bcrypt
            Usuario.PasswordUsuario = BCrypt.Net.BCrypt.EnhancedHashPassword(Usuario.PasswordUsuario, 13);
            BD conexion = new BD();
            return conexion.RegistrarUsuarioBD(Usuario); 
        }


        
        public Usuario LogearUsuario(string Email, string Password)
        {
            BD BD = new BD();

            return BD.LogearUsuario(Email, Password);
        }

    }
}
