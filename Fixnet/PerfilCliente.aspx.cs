using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using System.Text.Json;
using Negocio;

namespace Fixnet
{
    public partial class PerfilCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Usuario usuario = (Usuario)Session["Usuario"];

                if (usuario == null)
                {
                    Response.Redirect("/Logearse.aspx");
                    return;
                }

                bool sinDatos = usuario.Cliente == null ||
                                string.IsNullOrEmpty(usuario.Cliente.DireccionCliente) ||
                                usuario.Cliente.DireccionCliente == "No ingresado";

                pnlFormulario.Visible = true;
                pnlOk.Visible = !sinDatos;

                RegisterAsyncTask(new PageAsyncTask(CargarProvincias));
                ddlDepartamento.Enabled = false;
                ddlLocalidad.Enabled = false;

                ddlDepartamento.Items.Insert(0, "-- Seleccionar Municipio --");
                ddlLocalidad.Items.Insert(0, "-- Seleccionar Localidad --");

                CargarDatosCliente();
            }
        }

        protected async Task CargarProvincias()
        {
            var url = "https://apis.datos.gob.ar/georef/api/provincias";

            using (HttpClient httpClient = new HttpClient())
            {
                var Respuesta = await httpClient.GetAsync(url);

                if (Respuesta.IsSuccessStatusCode)
                {
                    var Data = await Respuesta.Content.ReadAsStringAsync();
                    var ListaProvincias = JsonSerializer.Deserialize<ListaDeProvincias>(Data,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    var Provincias = ListaProvincias.Provincias.OrderBy(p => p.Nombre).ToList();


                    ddlProvincia.DataSource = Provincias;
                    ddlProvincia.DataValueField = "Nombre";

                    ddlProvincia.DataBind();

                    ddlProvincia.Items.Insert(0, "-- Seleccionar una Provincia --");
                }
            }
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProvincia.SelectedIndex == 0)
            {
                ddlDepartamento.Enabled = false; 
                ddlLocalidad.Enabled = false;
                pnlOk.Visible = false;
                LimpiarDdls();
            }
            else
            {
                LimpiarDdls();
                ddlDepartamento.Enabled = true; 
                RegisterAsyncTask(new PageAsyncTask(CargarDepartamentos));
            }

        }

        private void LimpiarDdls()
        {
            ddlDepartamento.Items.Clear();
            ddlLocalidad.Items.Clear();

            ddlDepartamento.Items.Insert(0, "-- Seleccionar Departamento --");
            ddlLocalidad.Items.Insert(0, "-- Seleccionar Localidad --");
        }

        protected async Task CargarDepartamentos()
        {
            var url = "https://apis.datos.gob.ar/georef/api/departamentos?provincia=" + ddlProvincia.SelectedValue + "&max=5000";
            using (HttpClient httpClient = new HttpClient())
            {
                var Respuesta = await httpClient.GetAsync(url);

                if (Respuesta.IsSuccessStatusCode)
                {
                    var Data = await Respuesta.Content.ReadAsStringAsync();
                    var ListaMunicipios = JsonSerializer.Deserialize<ListaMunicipios>(Data,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    var Municipios = ListaMunicipios.Departamentos.OrderBy(p => p.Nombre).ToList();

                    ddlDepartamento.DataSource = Municipios;
                    ddlDepartamento.DataValueField = "Nombre";
                    ddlDepartamento.DataBind();

                    ddlDepartamento.Items.Insert(0, "-- Selecccionar un Municipio --");
                }
            }
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlDepartamento.SelectedIndex == 0)
            {
                ddlLocalidad.Enabled = false;
                pnlOk.Visible = false;
                ddlLocalidad.Items.Clear();
                ddlLocalidad.Items.Insert(0, "-- Seleccionar una Localidad --");
            }
            else
            {
                ddlLocalidad.Enabled = true;
                RegisterAsyncTask(new PageAsyncTask(CargarLocalidades));
            }
        }

        protected void ddlLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocalidad.SelectedIndex == 0)
                pnlOk.Visible = false;
        }

        protected async Task CargarLocalidades()
        {
            var url = "https://apis.datos.gob.ar/georef/api/localidades?provincia=" + ddlProvincia.SelectedValue
                + "&departamento=" + ddlDepartamento.SelectedItem.ToString() 
                + "&max=200"; 
            using (HttpClient httpClient = new HttpClient())
            {
                var Respuesta = await httpClient.GetAsync(url);

                if (Respuesta.IsSuccessStatusCode)
                {
                    var Data = await Respuesta.Content.ReadAsStringAsync();
                    var ListaLocalidades = JsonSerializer.Deserialize<ListaDeLocalidades>(Data,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                    var Localidades = ListaLocalidades.Localidades.OrderBy(p => p.Nombre).ToList();

                    Session.Remove("Localidades");

                    //Con esto se eliminan duplicados, ya que hay departamentos con localidades "repetidas"
                    var LocalidadesSinDuplicadas = Localidades.GroupBy(x => x.Nombre).Select(x => x.First()).ToList();

                    Session.Add("Localidades", LocalidadesSinDuplicadas);

                    ddlLocalidad.DataSource = LocalidadesSinDuplicadas;
                    ddlLocalidad.DataValueField = "Nombre";
                    ddlLocalidad.DataBind();

                    ddlLocalidad.Items.Insert(0, "-- Selecccionar una Localidad --");
                }

            }
        }

        //No tiene validaciones todavía
        protected void btnActualizarInformacion_Click(object sender, EventArgs e)
        {
            Usuario usuarioSession = (Usuario)Session["Usuario"];

            if (usuarioSession == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            // VALIDO QUE SE HAYA SELECCIONADO PROVINCIA, MUNICIPIO Y LOCALIDAD
            if (ddlProvincia.SelectedIndex == 0 || ddlDepartamento.SelectedIndex == 0 || ddlLocalidad.SelectedIndex == 0)
            {
                lblErrorDireccion.Text = "Por favor, seleccione Provincia, Municipio y Localidad válidos.";
                lblErrorDireccion.Visible = true;
                return;
            }

            /*Usuario usuario = new Usuario();
            usuario.IdUsuario = usuarioSession.IdUsuario;

            //usuario.Cliente = new Cliente(); // 👈 CLAVE -> El constructor vacío ya declara un nuevo Cliente y prestador 

            usuario.Cliente.Provincia = ddlProvincia.SelectedValue;
            usuario.Cliente.Departamento = ddlDepartamento.SelectedValue;
            usuario.Cliente.Localidad = ddlLocalidad.SelectedValue;
            List<Localidades> localidades = (List<Localidades>)Session["Localidades"];
            usuario.Cliente.IdLocalidad = localidades[ddlLocalidad.SelectedIndex - 1].Id.ToString(); 
            usuario.Cliente.DireccionCliente = txtDireccion.Text;*/

            Usuario usuario = (Usuario)Session["Usuario"];

            usuario.Cliente.Provincia = ddlProvincia.SelectedValue;
            usuario.Cliente.Departamento = ddlDepartamento.SelectedValue;
            usuario.Cliente.Localidad = ddlLocalidad.SelectedValue;

            List<Localidades> localidades = (List<Localidades>)Session["Localidades"];
            usuario.Cliente.IdLocalidad = localidades[ddlLocalidad.SelectedIndex - 1].Id.ToString();
            usuario.Cliente.DireccionCliente = txtDireccion.Text;

            UsuarioManager manager = new UsuarioManager();

            if (manager.ActualizarDireccionCliente(usuario))
            {
                usuarioSession.Cliente = usuario.Cliente;
                Session["Usuario"] = usuarioSession;

                Response.Redirect("/PerfilUsuario.aspx", false);
            }
            else
            {
                lblErrorDireccion.Text = "Ocurrió un error al actualizar la dirección.";
                lblErrorDireccion.Visible = true;
            }
        }

        private void CargarDatosCliente()
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario?.Cliente != null && usuario.Cliente.DireccionCliente != "No ingresado")
            {
                // Dirección
                txtDireccion.Text = usuario.Cliente.DireccionCliente;

                // Provincia
                ddlProvincia.SelectedValue = usuario.Cliente.Provincia;

                // Activar departamento
                ddlDepartamento.Enabled = true;

                // Cargar departamentos y luego seleccionar
                RegisterAsyncTask(new PageAsyncTask(async () =>
                {
                    await CargarDepartamentos();

                    ddlDepartamento.SelectedValue = usuario.Cliente.Departamento;

                    ddlLocalidad.Enabled = true;

                    await CargarLocalidades();

                    ddlLocalidad.SelectedValue = usuario.Cliente.Localidad;
                }));
            }
        }
    }
}