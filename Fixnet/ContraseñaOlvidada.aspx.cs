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
    public partial class ContraseñaOlvidada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ModificarContraseña"] != null)
            {
                divIngresarContraseña.Visible = true;
                divIngresarMail.Visible = false;
                divIngresarCodigo.Visible = false;
            }
            else if (Session["codigoRecuperacion"] != null)
            {
                divIngresarCodigo.Visible = true;
                divIngresarMail.Visible = false;
                divIngresarContraseña.Visible = false;
            }
            else
            {
                divIngresarContraseña.Visible = false;
                divIngresarCodigo.Visible = false;
            }
           
        }

        //Genera el número aleatorio que será enviado via mail
        private int numRandom()
        {
            Random Random = new Random();
            return Random.Next(111111, 999999);
        }

        //Envia el código al mail del usuario
        protected void btnRecuperar_Click(object sender, EventArgs e)
        {   
            UsuarioManager UsuarioManager = new UsuarioManager();
            Usuario UsuarioModificado = new Usuario()
            {
                IdUsuario = UsuarioManager.TraerIdUsuario(txtMail.Text),
                EmailUsuario = txtMail.Text
            }; 

            if ( UsuarioModificado.IdUsuario != 0)
            {
                mailNoEncontrado.InnerHtml = "";

                int NumRandom = numRandom(); 
                Session.Add("codigoRecuperacion", NumRandom);

                EmailManager EmailManager = new EmailManager();
                EmailManager.EnviarCodigo(txtMail.Text, NumRandom);

                Session.Add("UsuarioAModificar", UsuarioModificado);
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                mailNoEncontrado.InnerHtml = "Mail no encontrado";
            }
        }

        //Revisa que el código ingresado por el usuario esté correcto o al menos no esté vacio
        protected void btnCodigoMail_Click(object sender, EventArgs e)
        {
            if (txtCodigoMail.Text.Length == 0)
            {
                smallCodigoIncorrecto.InnerHtml = "Error, debes ingresar el código que fue enviado a tu mail";
            }
            else if (txtCodigoMail.Text != Session["codigoRecuperacion"].ToString())
            {
                smallCodigoIncorrecto.InnerHtml = "Código erroneo";
                return;
            }
            else
            {
                Session.Add("ModificarContraseña", (Usuario)Session["UsuarioAModificar"]);
                Response.Redirect(Request.RawUrl);
            }
        }

        //Cambia la contraseña del usuario y redirecciona al perfil del mismo
        protected void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            Usuario UsuarioModificado = (Usuario)Session["ModificarContraseña"];

            UsuarioManager UsuarioManager = new UsuarioManager();
            
            if (UsuarioManager.CambiarContraseña(UsuarioModificado.EmailUsuario, txtContraseñaNueva.Text))
            {
                UsuarioModificado = UsuarioManager.LogearUsuario(UsuarioModificado.EmailUsuario, txtContraseñaNueva.Text); 
            }

            Session.Remove("codigoRecuperacion");
            Session.Remove("UsuarioAModificar");
            Session.Remove("ModificarContraseña");
            Session.Add("Usuario", UsuarioModificado);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "contrasenaCambiadaModal",
              "var modal = new bootstrap.Modal(document.getElementById('contrasenaCambiadaModal')); modal.show();" +
              "setTimeout(function() { window.location.href = '/PerfilUsuario.aspx'; }, 5000);", true);
        }


        //Boton de volver, borra todos los objetos en la sesión para que tenga que empezar devuelta 
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session.Remove("codigoRecuperacion");
            Session.Remove("UsuarioAModificar");
            Session.Remove("ModificarContraseña");
            Response.Redirect("Logearse.aspx", false);
            return;
        }

    }
}