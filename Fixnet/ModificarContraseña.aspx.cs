using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fixnet
{
    public partial class CambiarContraseña : System.Web.UI.Page
    {
        Usuario Usuario = new Usuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario = (Usuario)Session["Usuario"];

            if (Usuario == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilUsuario.aspx", false);
            return;
        }



        protected void ModificarContraseña_Click(object sender, EventArgs e)
        {
            if (!RevisarInformacionIngresada())
            {
                return; 
            }
            else
            {
                Usuario.PasswordUsuario = txtPassword.Text;
                UsuarioManager UsuarioManager = new UsuarioManager();

                if (UsuarioManager.CambiarContraseña(Usuario.EmailUsuario, Usuario.PasswordUsuario))
                {
                    Session["Usuario"] = Usuario;

                    ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "ModificarContraseñaModal",
                    "var myModal = new bootstrap.Modal(document.getElementById('ModificarContraseñaModal')); myModal.show();" +
                    "setTimeout(function() { window.location.href = '/PerfilUsuario.aspx'; }, 5000);",
                    true
                );
                }
                else
                {
                    throw new Exception("Algo salió mal"); 
                }
            }
        }

        protected bool RevisarInformacionIngresada()
        {
            if (!RevisarTxTs())
            {
                lblError.Text = "La contraseña no puede quedar incompleta.";
                lblError.Visible = true;
                return false;
            }

            if (!Validaciones.ValidarPassword(txtPassword.Text))
            {
                lblError.Text = "La contraseña debe tener al menos 8 caracteres, una mayúscula y un número.";
                lblError.Visible = true;
                return false;
            }

            return true;
        }

        protected bool RevisarTxTs()
        {
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                return true;
            }

            return false;
        }
    }
}