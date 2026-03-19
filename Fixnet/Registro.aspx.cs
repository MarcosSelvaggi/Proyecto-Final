using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

namespace Fixnet
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnRegistro_Click(object sender, EventArgs e)
        {
            if (RevisarTxTs())
            {
                Usuario Usuario = new Usuario();
                UsuarioManager UsuarioManager = new UsuarioManager();

                Usuario.NombreUsuario = txtNombre.Text;
                Usuario.ApellidoUsuario = txtApellido.Text;
                Usuario.TelefonoUsuario = txtTeléfono.Text;
                Usuario.EmailUsuario = txtEmail.Text;
                Usuario.PasswordUsuario = txtPassword.Text;

                if (UsuarioManager.RegistrarUsuario(Usuario) != 0)
                {
                    Session.Add("Usuario", Usuario);
                    Response.Redirect("/SeleccionarPerfil.aspx");
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }


        }
        public bool RevisarTxTs() //Si hay un txt null o vacío devuelve false, caso contrario devuelve true
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) || (!string.IsNullOrWhiteSpace(txtApellido.Text)) ||
                (!string.IsNullOrWhiteSpace(txtTeléfono.Text)) || (!string.IsNullOrWhiteSpace(txtEmail.Text)) ||
                (!string.IsNullOrWhiteSpace(txtPassword.Text)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}