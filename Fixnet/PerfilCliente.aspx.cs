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
        private string ProvinciaSeleccionada { get; set; }
        private string MunicipioSeleccionado { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(CargarProvincias));
                ddlDepartamento.Enabled = false;
                ddlLocalidad.Enabled = false;

                ddlDepartamento.Items.Insert(0, "-- Seleccionar Municipio --");
                ddlLocalidad.Items.Insert(0, "-- Seleccionar Localidad --");
            }
        }

        protected async Task CargarProvincias()
        {
            var url = "https://apis.datos.gob.ar/georef/api/v2.0/provincias";

            using (HttpClient httpClient = new HttpClient())
            {
                var Respuesta = await httpClient.GetAsync(url);

                if (Respuesta.IsSuccessStatusCode)
                {
                    var Data = await Respuesta.Content.ReadAsStringAsync();
                    var ListaProvincias = JsonSerializer.Deserialize<Listadeprovincias>(Data,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    var Provincias = ListaProvincias.Provincias.OrderBy(p => p.Nombre).ToList();


                    ddlProvincia.DataSource = Provincias;
                    ddlProvincia.DataValueField = "Nombre";
                    ddlProvincia.DataBind();

                    ddlProvincia.Items.Insert(0, "-- Seleccionar una provincia --");
                }
            }
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProvincia.SelectedIndex == 0)
            {
                ddlDepartamento.Enabled = false; 
                ddlLocalidad.Enabled = false;
            }
            else
            {
                LimpiarDdls();
                ddlDepartamento.Enabled = true; 
                ProvinciaSeleccionada = ddlProvincia.SelectedValue.ToString();
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
            var url = "https://apis.datos.gob.ar/georef/api/v2.0/departamentos?provincia=" + ProvinciaSeleccionada;
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

                    ddlDepartamento.Items.Insert(0, "-- Selecccionar una localidad");
                }
            }
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlDepartamento.SelectedIndex == 0)
            {
                ddlLocalidad.Enabled = false;
            }
            else
            {
                ddlLocalidad.Enabled = true;
                MunicipioSeleccionado = ddlDepartamento.SelectedValue.ToString();
                RegisterAsyncTask(new PageAsyncTask(CargarLocalidades));
            }
        }

        protected async Task CargarLocalidades()
        {
            var url = "https://apis.datos.gob.ar/georef/api/v2.0/localidades?provincia=" + ddlProvincia.SelectedItem.ToString() 
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
                    
                    //Con esto se eliminan duplicados, ya que hay departamentos con localidades "repetidas"
                    var LocalidadesSinDuplicadas = Localidades.GroupBy(x => x.Nombre).Select(x => x.First()).ToList();

                    ddlLocalidad.DataSource = LocalidadesSinDuplicadas;
                    ddlLocalidad.DataValueField = "Nombre";
                    ddlLocalidad.DataBind();

                    ddlLocalidad.Items.Insert(0, "-- Selecccionar una localidad");
                }

            }
        }

        //No tiene validaciones todavía
        protected void btnActualizarInformacion_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario();
            usuario.Cliente.Provincia = ddlProvincia.SelectedValue.ToString();
            usuario.Cliente.Departamento = ddlDepartamento.SelectedValue.ToString();
            usuario.Cliente.Localidad = ddlLocalidad.SelectedValue.ToString();
            usuario.Cliente.DireccionCliente = txtDireccion.Text; 

            UsuarioManager manager = new UsuarioManager();
            
            if (manager.ActualizarDireccionCliente(usuario))
            {
                Response.Redirect("/SeleccionarPerfil.aspx", false);
            }
            else
            {
                //Algo
            }

        }
    }
}