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
    public partial class MisTurnos : System.Web.UI.Page
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
                case "Aceptado": return "bg-success";
                case "Rechazado": return "bg-danger";
                case "Pendiente": return "bg-warning text-dark";
                default: return "bg-secondary";
            }
        }
    }
}