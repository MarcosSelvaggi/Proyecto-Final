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
    public partial class ModificarPerfil : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                CargarTxTs();
            }
        }

        protected void CargarTxTs()
        {
            txtNombre.Text = Usuario.NombreUsuario;
            txtApellido.Text = Usuario.ApellidoUsuario;
            txtTeléfono.Text = Usuario.TelefonoUsuario;
            txtEmail.Text = Usuario.EmailUsuario;
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilUsuario.aspx", false);
            return;
        }

        protected void BtnModificarPerfil_Click(object sender, EventArgs e)
        {

            if (!RevisarInformacionIngresada())
            {
                return;
            }

            Usuario.NombreUsuario = txtNombre.Text;
            Usuario.ApellidoUsuario = txtApellido.Text;
            Usuario.EmailUsuario = txtEmail.Text;
            Usuario.TelefonoUsuario = txtTeléfono.Text; 

            UsuarioManager UsuarioManager = new UsuarioManager();

            if (UsuarioManager.ModificarUsuario(Usuario))
            {
                Session["Usuario"] = Usuario;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModificarUsuarioModal",
                    "var modal = new bootstrap.Modal(document.getElementById('ModificarUsuarioModal')); modal.show();" +
                    "setTimeout(function() { window.location.href = '/PerfilUsuario.aspx'; }, 5000);", true);
            }

        }

        protected bool RevisarInformacionIngresada()
        {
            if (!RevisarTxTs())
            {
                lblError.Text = "Hay campos incompletos.";
                lblError.Visible = true;
                return false;
            }

            if (!RealizarValidaciones())
            {
                return false; 
            }

            return true;
        }

        protected bool RevisarTxTs()
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) &&
                !string.IsNullOrWhiteSpace(txtApellido.Text) &&
                !string.IsNullOrWhiteSpace(txtEmail.Text) &&
                !string.IsNullOrWhiteSpace(txtTeléfono.Text))
            {
                return true;
            }

            return false;
        }

        protected bool RealizarValidaciones()
        {
            if (!Validaciones.ValidarTelefono(txtTeléfono.Text))
            {
                lblError.Text = "El teléfono debe tener exactamente 10 números juntos.";
                lblError.Visible = true;
                return false;
            }

            if (Usuario.TelefonoUsuario != txtTeléfono.Text)
            {
                if (!Validaciones.ValidarTelefonoExiste(txtTeléfono.Text))
                {
                    lblError.Text = "El teléfono ya está registrado.";
                    lblError.Visible = true;
                    return false;
                }
            }

            if (!Validaciones.ValidarEmail(txtEmail.Text))
            {
                lblError.Text = "El email no tiene un formato válido.";
                lblError.Visible = true;
                return false;
            }
            if (Usuario.EmailUsuario != txtEmail.Text)
            {
                if (!Validaciones.ValidarEmailExiste(txtEmail.Text))
                {
                    lblError.Text = "El email ya está registrado.";
                    lblError.Visible = true;
                    return false;
                }
            }
            return true; 
        }

    }
}