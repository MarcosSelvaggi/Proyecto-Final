using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Negocio;

namespace Fixnet
{
    public class MensajesHub : Hub
    {
        private readonly UsuarioManager _mgr = new UsuarioManager();

        public override async Task OnConnected()
        {
            var userId = Context.QueryString["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.Add(Context.ConnectionId, "user-" + userId);
            }
            await base.OnConnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.QueryString["userId"];
            await base.OnDisconnected(stopCalled);
        }

        public async Task UnirseAConversacion(int idConversacion)
        {
            await Groups.Add(Context.ConnectionId, "conv-" + idConversacion);
        }

        public async Task SalirDeConversacion(int idConversacion)
        {
            await Groups.Remove(Context.ConnectionId, "conv-" + idConversacion);
        }

        public async Task EnviarMensaje(int idConversacion, int idEmisor, string texto, string nombre, string apellido)
        {
            _mgr.EnviarMensaje(idConversacion, idEmisor, texto);

            // Obtengo la foto del usuario desde la BD
            string foto = _mgr.ObtenerFotoPerfil(idEmisor) ?? "";

            var mensaje = new
            {
                idConversacion = idConversacion,
                idEmisor = idEmisor,
                texto = texto,
                hora = System.DateTime.Now.ToString("HH:mm"),
                leido = false,
                nombre = nombre,
                apellido = apellido,
                foto = foto
            };

            await Clients.Group("conv-" + idConversacion).nuevoMensaje(mensaje);

            int otroUsuarioId = _mgr.ObtenerOtroUsuarioDeConversacion(idConversacion, idEmisor);
            if (otroUsuarioId > 0)
            {
                int noLeidos = _mgr.ContarMensajesNoLeidos(otroUsuarioId);
                await Clients.Group("user-" + otroUsuarioId).actualizarBadge(noLeidos);
            }
        }

        public async Task MarcarLeidos(int idConversacion, int idEmisor)
        {
            _mgr.MarcarMensajesLeidos(idConversacion, idEmisor);
            await Clients.Group("conv-" + idConversacion).mensajesLeidos();
        }

        public async Task NotificarNoLeidos(int idUsuario)
        {
            int noLeidos = _mgr.ContarMensajesNoLeidos(idUsuario);
            await Clients.Group("user-" + idUsuario).actualizarBadge(noLeidos);
        }

        public static void NotificarMensajeExterno(int idConversacion, int idEmisor, string texto, string nombre, string apellido, UsuarioManager mgr)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MensajesHub>();
            string foto = mgr.ObtenerFotoPerfil(idEmisor) ?? "";
            var mensaje = new
            {
                idConversacion = idConversacion,
                idEmisor = idEmisor,
                texto = texto,
                hora = System.DateTime.Now.ToString("HH:mm"),
                leido = false,
                nombre = nombre,
                apellido = apellido,
                foto = foto
            };

            int otroUsuario = mgr.ObtenerOtroUsuarioDeConversacion(idConversacion, idEmisor);
            if (otroUsuario > 0)
            {
                int noLeidos = mgr.ContarMensajesNoLeidos(otroUsuario);
                context.Clients.Group("user-" + otroUsuario).actualizarBadge(noLeidos);
            }
        }
    }
}