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

            // =========================
            // DATOS GENERALES
            // =========================

            lblNombreCompleto.Text = usuario.NombreUsuario + " " + usuario.ApellidoUsuario;
            lblEmail.Text = usuario.EmailUsuario;
            lblTelefono.Text = usuario.TelefonoUsuario;

            // Iniciales
            string inicialNombre = !string.IsNullOrEmpty(usuario.NombreUsuario) ? usuario.NombreUsuario.Substring(0, 1) : "";
            string inicialApellido = !string.IsNullOrEmpty(usuario.ApellidoUsuario) ? usuario.ApellidoUsuario.Substring(0, 1) : "";
            lblIniciales.Text = (inicialNombre + inicialApellido).ToUpper();


            // =========================
            // CLIENTE
            // =========================

            bool tieneCliente =
                usuario.Cliente != null &&
                TieneTextoValido(usuario.Cliente.DireccionCliente);

            pnlClienteDatos.Visible = tieneCliente;
            pnlClienteVacio.Visible = !tieneCliente;

            // Badge cliente
            pnlBadgeClienteOk.Visible = tieneCliente;
            pnlBadgeClienteVacio.Visible = !tieneCliente;

            if (tieneCliente)
            {
                lblProvincia.Text = usuario.Cliente.Provincia;
                lblDepartamento.Text = usuario.Cliente.Departamento;
                lblLocalidad.Text = usuario.Cliente.Localidad;
                lblDireccion.Text = usuario.Cliente.DireccionCliente;
            }


            // =========================
            // PRESTADOR
            // =========================

            bool tienePrestador =
                usuario.Prestador != null &&
                TieneTextoValido(usuario.Prestador.DescripcionPrestador);

            pnlPrestadorDatos.Visible = tienePrestador;
            pnlPrestadorVacio.Visible = !tienePrestador;

            // Badge prestador
            pnlBadgePrestadorOk.Visible = tienePrestador;
            pnlBadgePrestadorVacio.Visible = !tienePrestador;

            if (tienePrestador)
            {
                lblDescripcion.Text = usuario.Prestador.DescripcionPrestador;
                lblZonas.Text = usuario.Prestador.ZonasPrestador;
            }
        }

        private bool TieneTextoValido(string valor)
        {
            return !string.IsNullOrWhiteSpace(valor) && valor != "No ingresado";
        }

        // =========================
        // BOTONES CLIENTE
        // =========================

        protected void btnEditarCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilCliente.aspx");
        }

        protected void btnCrearCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PerfilCliente.aspx");
        }

        // =========================
        // BOTONES PRESTADOR
        // =========================

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