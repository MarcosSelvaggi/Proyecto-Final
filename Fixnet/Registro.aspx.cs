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

            if (!Validaciones.ValidarTelefono(txtTelefono.Text))
            {
                lblError.Text = "El teléfono debe tener exactamente 10 números juntos.";
                lblError.Visible = true;
                return;
            }

            if (!Validaciones.ValidarTelefonoExiste(txtTelefono.Text))
            {
                lblError.Text = "El teléfono ya está registrado.";
                lblError.Visible = true;
                return;
            }


            if (!Validaciones.ValidarEmailExiste(txtEmail.Text))
            {
                lblError.Text = "El email ya está registrado.";
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
            Usuario.TelefonoUsuario = txtTelefono.Text;
            Usuario.EmailUsuario = txtEmail.Text;
            Usuario.PasswordUsuario = txtPassword.Text;

            string passwordPlano = Usuario.PasswordUsuario;

            if (UsuarioManager.RegistrarUsuario(Usuario) == true)
            {
                //Usuario.IdUsuario = UsuarioManager.BuscarUsuarioMail(Usuario);

                /*Usuario = UsuarioManager.LogearUsuario(Usuario.EmailUsuario, Usuario.PasswordUsuario);

                Session.Add("Usuario", Usuario);
                */

                Usuario usuarioCompleto = UsuarioManager.LogearUsuario(Usuario.EmailUsuario, passwordPlano);

                Session["Usuario"] = usuarioCompleto;

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