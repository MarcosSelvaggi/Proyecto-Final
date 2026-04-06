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

        public bool ActualizarDireccionCliente(Usuario usuario)
        {
            BD conexion = new BD();
            return conexion.actualizarDireccionCliente(usuario);
        }

        public int ActualizarDatosPrestador(Usuario usuario)
        {
            BD conexion = new BD();
            return conexion.ActualizarPrestadorBD(usuario);
        }
        public List<Servicio> TraerServicios()
        {
            BD conexion = new BD();
            return conexion.TraerServiciosBD();
        }
        public List<ServiciosPrestador> TraerServiciosPrestador(int idPrestador)
        {
            BD conexion = new BD();
            return conexion.TraerServiciosPrestador(idPrestador);
        }
        public int BuscarUsuarioMail(Usuario usuario)
        {
            BD conexion = new BD();
            return conexion.ObtenerIdUsuarioPorEmail(usuario.EmailUsuario);
        }

    }
}
