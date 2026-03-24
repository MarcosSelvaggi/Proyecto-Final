using Dominio;
using Negocio;
using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace Fixnet
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnRegistro_Click(object sender, EventArgs e)
        {
            if (!RevisarTxTs())
            {
                lblError.Text = "Nombre y Apellido son campos obligatorios.";
                lblError.Visible = true;
                return;
            }

            if (!Validaciones.ValidarTelefono(txtTeléfono.Text))
            {
                lblError.Text = "El teléfono debe tener exactamente 8 números juntos.";
                lblError.Visible = true;
                return;
            }

            if (!Validaciones.ValidarEmail(txtEmail.Text))
            {
                lblError.Text = "El email no tiene un formato válido.";
                lblError.Visible = true;
                return;
            }

            if (!Validaciones.ValidarPassword(txtPassword.Text))
            {
                lblError.Text = "La contraseña debe tener al menos 8 caracteres, una mayúscula y un número.";
                lblError.Visible = true;
                return;
            }
                    
            Usuario Usuario = new Usuario();
            UsuarioManager UsuarioManager = new UsuarioManager();

            Usuario.NombreUsuario = txtNombre.Text;
            Usuario.ApellidoUsuario = txtApellido.Text;
            Usuario.TelefonoUsuario = txtTeléfono.Text;
            Usuario.EmailUsuario = txtEmail.Text;
            Usuario.PasswordUsuario = txtPassword.Text;
            //Usuario.PasswordUsuario = Usuario.PasswordUsuario = Hasher.HashPassword(txtPassword.Text);

            //if (UsuarioManager.RegistrarUsuario(Usuario) != 0)
            if (UsuarioManager.RegistrarUsuario(Usuario) == 1)
            {
                Session.Add("Usuario", Usuario);
                Response.Redirect("/SeleccionarPerfil.aspx");
            }
            else
            {
                lblError.Text = "Ocurrió un error al registrar el usuario.";
                lblError.Visible = true;
            }
        }

        public bool RevisarTxTs()
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) &&
                !string.IsNullOrWhiteSpace(txtApellido.Text) )
                
            {
                return true;
            }

            return false;
        }
    }
}