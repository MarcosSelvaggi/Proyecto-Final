using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Fixnet
{
    public partial class TurnosSolicitados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Logearse.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarTurnos();
            }
        }

        private void CargarTurnos()
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario.Prestador == null || usuario.Prestador.IdPrestador <= 0)
                return;

            UsuarioManager bd = new UsuarioManager();
            var tabla = bd.TraerTurnosPrestador(usuario.Prestador.IdPrestador);

            rptTurnos.DataSource = tabla;
            rptTurnos.DataBind();
        }

        protected void AceptarTurno(object sender, CommandEventArgs e)
        {
            int idTurno = Convert.ToInt32(e.CommandArgument);
            CambiarEstadoTurno(idTurno, "Aceptado");
        }

        protected void RechazarTurno(object sender, CommandEventArgs e)
        {
            int idTurno = Convert.ToInt32(e.CommandArgument);
            CambiarEstadoTurno(idTurno, "Rechazado");
        }

        private void CambiarEstadoTurno(int idTurno, string estado)
        {
            UsuarioManager bd = new UsuarioManager();
            bd.ActualizarEstadoTurno(idTurno, estado);
            CargarTurnos();
        }

        protected string ObtenerClaseEstado(string estado)
        {
            switch (estado)
            {
                case "Aceptado": return "bg-success";
                case "Rechazado": return "bg-danger";
                case "Pendiente": return "bg-warning text-dark";
                default: return "bg-secondary";
            }
        }
    }
}