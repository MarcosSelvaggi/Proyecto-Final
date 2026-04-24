using Dominio;
using Negocio;
using System;
using System.Web.UI;

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
                Response.Redirect("/Logearse.aspx");
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
            string nuevaPass = txtPassword.Text;

            if (!Validaciones.ValidarPassword(nuevaPass))
            {
                lblError.Text = "La contraseña debe tener al menos 8 caracteres, una mayúscula y un número.";
                lblError.Visible = true;

                txtPassword.CssClass = "input input-error";
                return;
            }
            else
            {
                txtPassword.CssClass = "input input-ok";
            }

            Usuario.PasswordUsuario = nuevaPass;

            UsuarioManager UsuarioManager = new UsuarioManager();

            bool ok = UsuarioManager.CambiarContraseña(Usuario.EmailUsuario, nuevaPass);

            if (ok)
            {
                Session["Usuario"] = Usuario;

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "successModal",
                    "var modal = new bootstrap.Modal(document.getElementById('successModal')); modal.show();" +
                    "setTimeout(function(){ window.location.href='/PerfilUsuario.aspx'; }, 3000);",
                    true
                );
            }
        }

        /*protected bool RevisarInformacionIngresada()
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
        */
        /*protected bool RevisarTxTs()
        {
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                return true;
            }

            return false;
        }*/
    }
}