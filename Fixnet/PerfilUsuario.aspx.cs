using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
                //Desactiva este warning
                #pragma warning disable 
                CargarUsuario();
            }
        }

        private async Task CargarUsuario()
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario == null)
            {
                Response.Redirect("/Logearse.aspx");
                return;
            }

            // =========================
            // DATOS GENERALES
            // =========================

            lblNombreCompleto.Text = usuario.NombreUsuario + " " + usuario.ApellidoUsuario;
            lblEmail.Text = usuario.EmailUsuario;
            lblTelefono.Text = usuario.TelefonoUsuario;

            // Foto o iniciales
            if (!string.IsNullOrEmpty(usuario.FotoPerfil))
            {
                imgFotoPerfil.ImageUrl = usuario.FotoPerfil;
                imgFotoPerfil.Visible = true;
                divIniciales.Visible = false;
            }
            else
            {
                string inicialNombre = !string.IsNullOrEmpty(usuario.NombreUsuario) ? usuario.NombreUsuario.Substring(0, 1) : "";
                string inicialApellido = !string.IsNullOrEmpty(usuario.ApellidoUsuario) ? usuario.ApellidoUsuario.Substring(0, 1) : "";
                lblIniciales.Text = (inicialNombre + inicialApellido).ToUpper();
                imgFotoPerfil.Visible = false;
                divIniciales.Visible = true;
            }


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

                Session.Add("ListaLocalidades", usuario.Prestador.ZonasPrestador);

                RegisterAsyncTask(new PageAsyncTask(ListarLocalidades));
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

        // BOTÓN MODIFICAR PERFIL

        protected void ModificarInformacionPersonal_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ModificarPerfil.aspx");
        }
        protected void ModificarContraseña_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ModificarContraseña.aspx");
        }

        // LISTAR LAS LOCALIDADES 
        protected async Task ListarLocalidades()
        {
            string ListaLocalidades = (string)Session["ListaLocalidades"];
            var url = "https://apis.datos.gob.ar/georef/api/localidades?id=" + ListaLocalidades + "&campos=basico";

            using (HttpClient httpClient = new HttpClient())
            {
                var Respuesta = await httpClient.GetAsync(url);

                if (Respuesta.IsSuccessStatusCode)
                {
                    var Data = await Respuesta.Content.ReadAsStringAsync();
                    var Localidad = JsonSerializer.Deserialize<ListaDeLocalidades>(Data,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    var Lista = Localidad;

                    foreach (var item in Lista.Localidades)
                    {
                        lblZonas.Text += item.Nombre + ", ";
                    }

                    lblZonas.Text = lblZonas.Text.Remove(lblZonas.Text.Count() - 2);
                }
            }
        }

    }
}
