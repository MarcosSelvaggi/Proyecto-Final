using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace Fixnet
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] != null)
            {
                // Refresca el timeout cada vez que el usuario interactúa con la app
                Session.Timeout = 20;
            }
        }

        protected int ObtenerNoLeidos()
        {
            if (Session["Usuario"] == null) return 0;
            var u = (Dominio.Usuario)Session["Usuario"];
            return new UsuarioManager().ContarMensajesNoLeidos(u.IdUsuario);
        }
        protected bool EsPerfilValido(string valor)
        {
            return !string.IsNullOrEmpty(valor) && valor != "No ingresado";
        }

        protected void btn_CerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }
    }
}
