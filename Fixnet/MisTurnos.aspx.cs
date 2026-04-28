using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Fixnet
{
    public partial class MisTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Logearse.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarTurnos();
            }
        }
        protected int ObtenerOCrearConv(int idTurno, int idCliente, int idPrestador)
        {
            UsuarioManager bd = new UsuarioManager();
            return bd.ObtenerOCrearConversacion(idTurno, idCliente, idPrestador);
        }
        private void CargarTurnos()
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario.Cliente == null || usuario.Cliente.IdCliente <= 0)
                return;

            UsuarioManager bd = new UsuarioManager();
            var tabla = bd.TraerTurnosCliente(usuario.Cliente.IdCliente);

            rptTurnos.DataSource = tabla;
            rptTurnos.DataBind();
        }

        protected string ObtenerClaseEstado(string estado)
        {
            switch (estado)
            {
                case "Aceptado": return "estado-aceptado";
                case "Rechazado": return "estado-rechazado";
                case "Pendiente": return "estado-pendiente";
                default: return "estado-pendiente";
            }
        }

        protected void CalificarPrestador(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Calificar")
            {
                Session.Add("TurnoCalificado", e.CommandArgument.ToString());

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalCalificarPrestador",
                "var modal = new bootstrap.Modal(document.getElementById('ModalCalificarPrestador')); modal.show();", true);
            }
        }

        protected void BtnCalificar_Click(object sender, EventArgs e)
        {
            UsuarioManager UsuarioManager = new UsuarioManager();
            if (UsuarioManager.CargarCalificacion(Session["TurnoCalificado"].ToString(), TxtComentario.InnerText, PuntuacionDelPrestador.Value))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalTurnoCalificado",
                "var modal = new bootstrap.Modal(document.getElementById('ModalTurnoCalificado')); modal.show();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalTurnoNoCalificado",
                "var modal = new bootstrap.Modal(document.getElementById('ModalTurnoNoCalificado')); modal.show();", true);
                return;
            }

        }

        protected void BtnVolverAlPerfil_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilUsuario.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        protected bool HabilitarBotonCalificar(string Estado)
        {
            switch (Estado)
            {
                case "Aceptado": return true;
                case "Rechazado": return false;
                case "Pendiente": return false;
                case "Calificado": return false; 
                default: return false;
            }
        }

        protected void BtnConfirmarCalificacion_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}