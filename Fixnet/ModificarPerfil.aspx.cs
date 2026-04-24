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
                CargarFotoActual();
            }
        }

        protected void CargarTxTs()
        {
            txtNombre.Text = Usuario.NombreUsuario;
            txtApellido.Text = Usuario.ApellidoUsuario;
            txtTelefono.Text = Usuario.TelefonoUsuario;
            txtEmail.Text = Usuario.EmailUsuario;
        }
        protected void CargarFotoActual()
        {
            bool tieneFoto = !string.IsNullOrEmpty(Usuario.FotoPerfil);

            imgFotoActual.Visible = tieneFoto;
            divInicialesWrapper.Visible = !tieneFoto;

            if (tieneFoto)
            {
                imgFotoActual.ImageUrl = Usuario.FotoPerfil;
            }
            else
            {
                string ini =
                    (Usuario.NombreUsuario?.FirstOrDefault().ToString() ?? "") +
                    (Usuario.ApellidoUsuario?.FirstOrDefault().ToString() ?? "");

                lblIniciales.Text = ini.ToUpper();
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilUsuario.aspx", false);
        }

        protected void BtnModificarPerfil_Click(object sender, EventArgs e)
        {
            if (!RevisarInformacionIngresada())
                return;

            if (fuFoto.HasFile)
            {
                string tipo = fuFoto.PostedFile.ContentType;
                if (tipo != "image/jpeg" && tipo != "image/png" && tipo != "image/gif")
                {
                    lblErrorFoto.Text = "Solo se permiten imágenes JPG, PNG o GIF.";
                    lblErrorFoto.Visible = true;
                    return;
                }

                if (fuFoto.PostedFile.ContentLength > 2 * 1024 * 1024)
                {
                    lblErrorFoto.Text = "La imagen no puede superar los 2 MB.";
                    lblErrorFoto.Visible = true;
                    return;
                }

                byte[] bytes = fuFoto.FileBytes;
                string base64 = $"data:{tipo};base64,{Convert.ToBase64String(bytes)}";

                UsuarioManager mgr = new UsuarioManager();
                if (mgr.GuardarFotoPerfil(Usuario.IdUsuario, base64))
                {
                    Usuario.FotoPerfil = base64;
                }
            }

            // ── Datos personales ──────────────────────────────────────────
            Usuario.NombreUsuario = txtNombre.Text;
            Usuario.ApellidoUsuario = txtApellido.Text;
            Usuario.EmailUsuario = txtEmail.Text;
            Usuario.TelefonoUsuario = txtTelefono.Text; 

            UsuarioManager UsuarioManager = new UsuarioManager();

            if (UsuarioManager.ModificarUsuario(Usuario))
            {
                Session["Usuario"] = Usuario;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "successModal",
                "var modal = new bootstrap.Modal(document.getElementById('successModal')); modal.show();" +
                "setTimeout(function() { window.location.href = '/PerfilUsuario.aspx'; }, 3000);", true);
            }
        }

        // ── Validaciones (igual que antes) ────────────────────────────────

        protected bool RevisarInformacionIngresada()
        {
            if (!RevisarTxTs())
            {
                lblError.Text = "Hay campos incompletos.";
                lblError.Visible = true;
                return false;
            }
            return RealizarValidaciones();
        }

        protected bool RevisarTxTs()
        {

            return !string.IsNullOrWhiteSpace(txtNombre.Text) &&
                   !string.IsNullOrWhiteSpace(txtApellido.Text) &&
                   !string.IsNullOrWhiteSpace(txtEmail.Text) &&
                   !string.IsNullOrWhiteSpace(txtTelefono.Text);
        }

        protected bool RealizarValidaciones()
        {
            if (!Validaciones.ValidarTelefono(txtTelefono.Text))
            {
                lblError.Text = "El teléfono debe tener exactamente 10 números juntos.";
                lblError.Visible = true;
                return false;
            }

            if (Usuario.TelefonoUsuario != txtTelefono.Text)
            {
                if (!Validaciones.ValidarTelefonoExiste(txtTelefono.Text))
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
