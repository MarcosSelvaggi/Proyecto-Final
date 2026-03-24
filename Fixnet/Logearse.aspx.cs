using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fixnet
{
    public partial class Logearse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            Usuario UsuarioLogeado = new Usuario();
            UsuarioManager usuarioManager = new UsuarioManager();

            UsuarioLogeado = usuarioManager.LogearUsuario(txtMail.Text, txtPass.Text);

            if (UsuarioLogeado != null)
            {
                Session.Add("Usuario", UsuarioLogeado);
                Response.Redirect("/SeleccionarPerfil.aspx", false);
            }
        }
    }
}