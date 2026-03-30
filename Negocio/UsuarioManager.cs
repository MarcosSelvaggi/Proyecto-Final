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

        public bool RegistrarUsuario(Usuario Usuario)
        {
            
            Usuario.PasswordUsuario = BCrypt.Net.BCrypt.EnhancedHashPassword(Usuario.PasswordUsuario, 13);
            BD conexion = new BD();
            return conexion.RegistrarUsuarioBD(Usuario) > 0; 
        }

        
        public Usuario LogearUsuario(string Email, string Password)
        {
            BD conexion = new BD();

            return conexion.LogearUsuario(Email, Password);
        }

        public bool ActualizarDireccionCliente(Usuario UsuarioActualizado)
        {
            BD conexion = new BD();

            return conexion.actualizarDireccionCliente(UsuarioActualizado);
        }

    }
}
