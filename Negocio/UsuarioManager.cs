using System;
using System.Collections.Generic;
using System.Data;
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
        BD Conexion = new BD();
        public bool RegistrarUsuario(Usuario Usuario)
        {
            Usuario.PasswordUsuario = BCrypt.Net.BCrypt.EnhancedHashPassword(Usuario.PasswordUsuario, 13);  
            return Conexion.RegistrarUsuarioBD(Usuario) > 0;
        }


        public Usuario LogearUsuario(string Email, string Password)
        {
            return Conexion.LogearUsuario(Email, Password);
        }

        public bool ActualizarDireccionCliente(Usuario usuario)
        {
            return Conexion.actualizarDireccionCliente(usuario);
        }
        public DataTable TraerTurnosCliente(int idCliente)
        {
            return Conexion.TraerTurnosCliente(idCliente);
        }
        public DataTable TraerTurnosPrestador(int idPrestador)
        {
            return Conexion.TraerTurnosPrestador(idPrestador);
        }
        public int ActualizarDatosPrestador(Usuario usuario)
        {
            return Conexion.ActualizarPrestadorBD(usuario);
        }
        public List<Servicio> TraerServicios()
        {  
            return Conexion.TraerServiciosBD();
        }

        //public bool ActualizarEstadoTurno(int idTurno, string estado)
        //{
        //    return Conexion.ActualizarEstadoTurno(idTurno, estado);
        //}
        public bool ActualizarEstadoTurno(int idTurno, string estado, string NombrePrestador)
        {
            return Conexion.ActualizarEstadoTurno(idTurno, estado, NombrePrestador);
        }
        public bool CrearSolicitudTurno(int idCliente, int idPrestador, int idServicio, string mensaje)
        {
            return Conexion.CrearSolicitudTurno(idCliente, idPrestador, idServicio, mensaje);
        }
        public List<ServiciosPrestador> TraerServiciosPrestador(int idPrestador)
        {
            return Conexion.TraerServiciosPrestador(idPrestador);
        }
        public int BuscarUsuarioMail(Usuario usuario)
        {
            return Conexion.ObtenerIdUsuarioPorEmail(usuario.EmailUsuario);
        }

        public List<Usuario> TraerPrestadores(Usuario Usuario, int Servicio)
        {
            return Conexion.DevolverPrestadores(Usuario, Servicio);
        }
    
        public int TraerIdUsuario(string Email)
        {
            return Conexion.ObtenerIdUsuarioPorEmail(Email);
        }

        public bool CambiarContraseña(string EmailUsuario, string PasswordNueva)
        {
            return Conexion.CambiarPassword(EmailUsuario, BCrypt.Net.BCrypt.EnhancedHashPassword(PasswordNueva, 13)); 
        }

        public bool ModificarUsuario(Usuario Usuario)
        {
            return Conexion.ActualiarInformacionUsuario(Usuario);
        }

        public bool CargarCalificacion(string IdTurno, string Comentario, string Calificacion)
        {
            return false;
            //return Conexion.CalificarTurno(IdTurno, Comentario, Calificacion); 
        }
    }
}
