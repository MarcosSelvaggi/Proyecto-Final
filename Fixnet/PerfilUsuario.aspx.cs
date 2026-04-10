using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fixnet
{
    public partial class PerfilUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuario();
            }
        }

        private void CargarUsuario()
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

           
            // DATOS USUARIO
           
            lblNombre.Text = usuario.NombreUsuario;
            lblApellido.Text = usuario.ApellidoUsuario;
            lblEmail.Text = usuario.EmailUsuario;
            lblTelefono.Text = usuario.TelefonoUsuario;


            // CLIENTE

            bool tieneCliente =
     usuario.Cliente != null &&
     !string.IsNullOrEmpty(usuario.Cliente.DireccionCliente) &&
     usuario.Cliente.DireccionCliente != "No ingresado";

            pnlClienteDatos.Visible = tieneCliente;
            pnlClienteVacio.Visible = !tieneCliente;

            if (tieneCliente)
            {
                lblProvincia.Text = usuario.Cliente.Provincia;
                lblDepartamento.Text = usuario.Cliente.Departamento;
                lblLocalidad.Text = usuario.Cliente.Localidad;
                lblDireccion.Text = usuario.Cliente.DireccionCliente;
            }


            // PRESTADOR

            bool tienePrestador =
            usuario.Prestador != null &&
           !string.IsNullOrEmpty(usuario.Prestador.DescripcionPrestador) &&
           usuario.Prestador.DescripcionPrestador != "No ingresado";

            pnlPrestadorDatos.Visible = tienePrestador;
            pnlPrestadorVacio.Visible = !tienePrestador;

            if (tienePrestador)
            {
                lblDescripcion.Text = usuario.Prestador.DescripcionPrestador;
                lblZonas.Text = usuario.Prestador.ZonasPrestador;
            }
        }

        
        // BOTONES CLIENTE
       
        protected void btnEditarCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilCliente.aspx");
        }

        protected void btnCrearCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilCliente.aspx");
        }

        // BOTONES PRESTADOR
        
        protected void btnEditarPrestador_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilPrestador.aspx");
        }

        protected void btnCrearPrestador_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilPrestador.aspx");
        }
    }
}