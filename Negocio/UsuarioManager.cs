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

        public int ObtenerIdServicio(string nombreServicio)
        {
            return Conexion.ObtenerIdServicio(nombreServicio);
        }
        // ── Mensajes
        public bool EliminarConversacion(int idConversacion, int idUsuario)
        {
            return Conexion.EliminarConversacion(idConversacion, idUsuario);
        }
        public int ObtenerOCrearConversacion(int idTurno, int idCliente, int idPrestador)
        {
            return Conexion.ObtenerOCrearConversacion(idTurno, idCliente, idPrestador);
        }

        public bool EnviarMensaje(int idConversacion, int idEmisor, string texto)
        {
            return Conexion.EnviarMensaje(idConversacion, idEmisor, texto);
        }
        public string ObtenerFotoPerfil(int idUsuario)
        {
            return Conexion.ObtenerFotoPerfil(idUsuario);
        }
        public DataTable TraerMensajesConversacion(int idConversacion)
        {
            return Conexion.TraerMensajesConversacion(idConversacion);
        }

        public DataTable TraerConversacionesUsuario(int idUsuario)
        {
            return Conexion.TraerConversacionesUsuario(idUsuario);
        }

        public bool MarcarMensajesLeidos(int idConversacion, int idUsuarioLector)
        {
            return Conexion.MarcarMensajesLeidos(idConversacion, idUsuarioLector);
        }

        public int ContarMensajesNoLeidos(int idUsuario)
        {
            return Conexion.ContarMensajesNoLeidos(idUsuario);
        }
        public DataRow TraerPerfilPrestador(int idUsuario)
        {
            return Conexion.TraerPerfilPrestador(idUsuario);
        }
        public int ObtenerOtroUsuarioDeConversacion(int idConversacion, int idEmisor)
        {
            return Conexion.ObtenerOtroUsuarioDeConversacion(idConversacion, idEmisor);
        }
        public DataTable TraerServiciosDePrestador(int idPrestador)
        {
            return Conexion.TraerServiciosDePrestador(idPrestador);
        }
        public Usuario LogearUsuario(string Email, string Password)
        {
            return Conexion.LogearUsuario(Email, Password);
        }

        public bool ActualizarDireccionCliente(Usuario usuario)
        {
            return Conexion.ActualizarDireccionCliente(usuario);
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
        public int ContarPrestadores(int idServicio, string idLocalidad)
        {
            return Conexion.ContarPrestadores(idServicio, idLocalidad);
        }

        //public bool ActualizarEstadoTurno(int idTurno, string estado)
        //{
        //    return Conexion.ActualizarEstadoTurno(idTurno, estado);
        //}
        public bool ActualizarEstadoTurno(int idTurno, string estado, string NombrePrestador)
        {
            return Conexion.ActualizarEstadoTurno(idTurno, estado, NombrePrestador);
        }
        public int CrearSolicitudTurno(int idCliente, int idPrestador, int idServicio, string mensaje)
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
            return Conexion.CalificarTurno(IdTurno, Comentario, Calificacion);
        }

        public bool GuardarFotoPerfil(int idUsuario, string base64)
        {
            return Conexion.GuardarFotoPerfilBD(idUsuario, base64);
        }

        public bool AceptarTurnoConFecha(int idTurno, int idPrestador, DateTime fechaProgramada)
        {
            return Conexion.AceptarTurnoConFecha(idTurno, idPrestador, fechaProgramada);
        }

        public DataTable TraerTurnosPrestadorPorMes(int idPrestador, int anio, int mes)
        {
            return Conexion.TraerTurnosPrestadorPorMes(idPrestador, anio, mes);
        }

        public string TraerDisponibilidadPrestador(int idPrestador)
        {
            return Conexion.TraerDisponibilidadPrestador(idPrestador);
        }


    }
}
