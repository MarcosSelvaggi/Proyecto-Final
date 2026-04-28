using System;
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
                Response.Redirect("~/Logearse.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarTurnos();
                CargarDisponibilidad();
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

        private void CargarDisponibilidad()
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario.Prestador == null || usuario.Prestador.IdPrestador <= 0)
                return;

            UsuarioManager bd = new UsuarioManager();
            string disp = bd.TraerDisponibilidadPrestador(usuario.Prestador.IdPrestador);

            // Lo guardamos en el hidden field para que el JS lo lea
            hfDisponibilidad.Value = disp;
            hfIdPrestador.Value = usuario.Prestador.IdPrestador.ToString();
        }

        protected void RechazarTurno(object sender, CommandEventArgs e)
        {
            int idTurno = Convert.ToInt32(e.CommandArgument);
            CambiarEstadoTurno(idTurno, "Rechazado");
        }

        private void CambiarEstadoTurno(int idTurno, string estado)
        {
            Usuario usuario = (Usuario)Session["Usuario"];
            UsuarioManager bd = new UsuarioManager();
            bd.ActualizarEstadoTurno(idTurno, estado, usuario.NombreUsuario);
            CargarTurnos();
        }

        protected void btnConfirmarAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                int idTurno = int.Parse(hfIdTurnoAceptar.Value);
                int idPrestador = int.Parse(hfIdPrestador.Value);
                DateTime fecha = DateTime.Parse(Request.Form["inputFecha"]);

                // Instancia de tu clase de negocio (asegurate que el nombre sea el correcto)
                UsuarioManager bd = new UsuarioManager();

                bool ok = bd.AceptarTurnoConFecha(idTurno, idPrestador, fecha);

                if (ok)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "modalOk",
                        "var m = bootstrap.Modal.getInstance(document.getElementById('modalFecha')); if(m) m.hide();", true);
                    CargarTurnos();
                }
            }
            catch (Exception ex)
            {
                // Manejo de error básico
                Response.Write("<script>alert('Error al procesar: " + ex.Message + "');</script>");
            }
        }

        private void MostrarError(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "err",
                $"alert('{msg}');", true);
        }

        protected string ObtenerClaseEstado(string estado)
        {
            switch (estado)
            {
                case "Aceptado": return "bg-success";
                case "Rechazado": return "bg-danger";
                case "Pendiente": return "bg-warning text-dark";
                case "Calificado": return "bg-primary";
                default: return "bg-secondary";
            }
        }

        protected string ObtenerEstadoDelTurno(string Estado)
        {
            switch (Estado)
            {
                case "Aceptado": return "turno-card turno-aceptado";
                case "Rechazado": return "turno-card turno-rechazado";
                case "Pendiente": return "turno-card turno-pendiente";
                case "Calificado": return "turno-card turno-calificado"; 
                default: return "turno-card turno-aceptado";
            }
        }
    }
}
