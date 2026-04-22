using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            if (Session["Usuario"] != null)
            {
                Response.Redirect("~/PerfilUsuario.aspx");
                return;
            }

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            UsuarioManager usuarioManager = new UsuarioManager();

            
            if (string.IsNullOrWhiteSpace(txtMail.Text))
            {
                lblError.Text = "Debe ingresar un mail.";
                lblError.Visible = true;
                return;
            }

            
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                lblError.Text = "Debe ingresar una contraseña.";
                lblError.Visible = true;
                return;
            }

            
            if (Validaciones.ValidarEmailExiste(txtMail.Text))
            {
                lblError.Text = "No existe mail.";
                lblError.Visible = true;
                return;
            }

            Usuario usuarioLogeado = usuarioManager.LogearUsuario(txtMail.Text, txtPass.Text);

            if (usuarioLogeado != null)
            {
                Session.Add("Usuario", usuarioLogeado);
                Response.Redirect("/SeleccionarPerfil.aspx", false);
            }
            else
            {
                lblError.Text = "Contraseña incorrecta.";
                lblError.Visible = true;
            }
        }




    }
}