using Dominio;
using Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fixnet
{
    public partial class PerfilPrestador : System.Web.UI.Page
    {
        List<Localidades> ListaLocalidades = new List<Localidades>();
        List<String> IdLocalidades = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Usuario usuario = (Usuario)Session["Usuario"];

                if (usuario == null)
                {
                    Response.Redirect("/Login.aspx");
                    return;
                }

                if (usuario.Prestador != null)
                {
                    txtDescripcion.Text = usuario.Prestador.DescripcionPrestador;
                    btnGuardarPrestador.Text = "Actualizar";

                    Session.Remove("Localidades_Seleccionadas_Por_Prestador");

                    if (usuario.Prestador.ZonasPrestador != null)
                    {
                        var Lista = usuario.Prestador.ZonasPrestador.Split(',');
                        Session.Add("Localidades_Ya_Ingresadas", Lista);
                        RegisterAsyncTask(new PageAsyncTask(RecuperarLocalidades));
                    }
                }

                RegisterAsyncTask(new PageAsyncTask(CargarProvincias));

                CargarServicios();
            }
        }

        //Famoso Ctrl + C / Ctrl + V desde PerfilCliente 
        protected async Task CargarProvincias()
        {
            var url = "https://apis.datos.gob.ar/georef/api/v2.0/provincias";

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
            var url = "https://apis.datos.gob.ar/georef/api/v2.0/departamentos?provincia=" + ddlProvincia.SelectedValue + "&max=5000";
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
            if (ddlDepartamento.SelectedIndex == 0)
            {
                ddlLocalidad.Enabled = false;
                ddlLocalidad.Items.Clear();
                ddlLocalidad.Items.Insert(0, "-- Seleccionar una Localidad --");
            }
            else
            {
                ddlLocalidad.Enabled = true;
                RegisterAsyncTask(new PageAsyncTask(CargarLocalidades));
            }
        }

        protected async Task CargarLocalidades()
        {
            if(ddlProvincia.SelectedValue != null && ddlDepartamento.SelectedItem != null)
            {
                var url = "https://apis.datos.gob.ar/georef/api/v2.0/localidades?provincia=" + ddlProvincia.SelectedValue
                + "&departamento=" + ddlDepartamento.SelectedItem.ToString()
                + "&max=200&campos=basico";
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
                        var ListaLocalidadesSinNombresDuplicados = Localidades.GroupBy(x => x.Nombre).Select(x => x.First()).ToList();


                        var ListaLocalidadesNoSeleccionadas = new List<Localidades>();

                        if (Session["Localidades_Seleccionadas_Por_Prestador"] != null)
                        {
                            var LocalidadesSeleccionadasPorPrestador = (List<Localidades>)Session["Localidades_Seleccionadas_Por_Prestador"];
                            foreach (var item in ListaLocalidadesSinNombresDuplicados)
                            {
                                if (!LocalidadesSeleccionadasPorPrestador.Contains(item))
                                {
                                    ListaLocalidadesNoSeleccionadas.Add(item);
                                }
                            }
                        }
                        else
                        {
                            ListaLocalidadesNoSeleccionadas = ListaLocalidadesSinNombresDuplicados;
                        }

                        ddlLocalidad.DataSource = ListaLocalidadesNoSeleccionadas;
                        ddlLocalidad.DataValueField = "Nombre";
                        ddlLocalidad.DataBind();

                        ddlLocalidad.Items.Insert(0, "-- Selecccionar una Localidad --");

                        AgregarListaEnSession(ListaLocalidadesNoSeleccionadas);
                    }
                }
            }
        }

        //La lista parece que hace clear en el postback
        protected void AgregarListaEnSession(List<Localidades> lista)
        {
            IdLocalidades.Clear();
            foreach (var item in lista)
            {
                IdLocalidades.Add(item.Id);
            }

            Session["Localidades_API_Georef"] = IdLocalidades;
        }

        protected void ddlLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocalidad.SelectedIndex != 0)
            {
                IdLocalidades = (List<string>)Session["Localidades_API_Georef"];

                Localidades Localidad = new Localidades
                {
                    Id = IdLocalidades[ddlLocalidad.SelectedIndex - 1],
                    Nombre = ddlLocalidad.SelectedValue
                };

                //Trae las localidades que el prestador eligió, agrega la nueva localidad seleccionada y remplaza la vieja lista
                if (Session["Localidades_Seleccionadas_Por_Prestador"] != null)
                {
                    ListaLocalidades = (List<Localidades>)Session["Localidades_Seleccionadas_Por_Prestador"];
                  
                }
                ListaLocalidades.Add(Localidad);
                Session["Localidades_Seleccionadas_Por_Prestador"] = ListaLocalidades;

                AgregarLocalidad();
            }
        }

        //Trae la lista de session y se la pasa al repeater
        protected void AgregarLocalidad()
        {
            rptLocalidades.DataSource = (List<Localidades>)Session["Localidades_Seleccionadas_Por_Prestador"];
            rptLocalidades.DataBind();

            RegisterAsyncTask(new PageAsyncTask(CargarLocalidades));
        }

        protected void EliminarLocalidades(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "EliminarLocalidad")
            {
                string IdLocalidadEliminada = e.CommandArgument.ToString();
                ListaLocalidades = (List<Localidades>)Session["Localidades_Seleccionadas_Por_Prestador"];
                ListaLocalidades.RemoveAll(x => x.Id.Equals(IdLocalidadEliminada));
                Session["Localidades_Seleccionadas_Por_Prestador"] = ListaLocalidades;
                AgregarLocalidad();
            }
        }

        protected async Task RecuperarLocalidades()
        {
            var Lista = (string[])Session["Localidades_Ya_Ingresadas"];

            foreach (var item in Lista)
            {
                var url = "https://apis.datos.gob.ar/georef/api/v2.0/localidades?id=" + item + "&campos=basico";

                using (HttpClient httpClient = new HttpClient())
                {
                    var Respuesta = await httpClient.GetAsync(url);

                    if (Respuesta.IsSuccessStatusCode)
                    {
                        var Data = await Respuesta.Content.ReadAsStringAsync();
                        var Localidad = JsonSerializer.Deserialize<ListaDeLocalidades>(Data,
                            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                        if (Session["Localidades_Seleccionadas_Por_Prestador"] != null)
                        {
                            ListaLocalidades = (List<Localidades>)Session["Localidades_Seleccionadas_Por_Prestador"];
                        }
                        Localidades LocalidadAux = new Localidades()
                        {
                            Id = Localidad.Localidades[0].Id,
                            Nombre = Localidad.Localidades[0].Nombre
                        };

                        ListaLocalidades.Add(LocalidadAux);
                        Session["Localidades_Seleccionadas_Por_Prestador"] = ListaLocalidades;

                    }
                }
            }
            rptLocalidades.DataSource = (List<Localidades>)Session["Localidades_Seleccionadas_Por_Prestador"];
            rptLocalidades.DataBind();
        }



        private void CargarServicios()
        {
            Usuario usuario = (Usuario)Session["Usuario"];
            UsuarioManager manager = new UsuarioManager();

            // Lista general de servicios
            var servicios = manager.TraerServicios();

            // Lista de servicios del prestador desde BD
            List<ServiciosPrestador> serviciosPrestador = new List<ServiciosPrestador>();
            if (usuario.Prestador != null)
            {
                if (usuario.Prestador.IdPrestador > 0)
                    serviciosPrestador = manager.TraerServiciosPrestador(usuario.Prestador.IdPrestador);
                else
                    serviciosPrestador = usuario.Prestador.Servicios ?? new List<ServiciosPrestador>();
            }


            rptServicios.DataSource = servicios;
            rptServicios.DataBind();


            foreach (RepeaterItem item in rptServicios.Items)
            {
                HiddenField hf = (HiddenField)item.FindControl("hfIdServicio");
                TextBox txt = (TextBox)item.FindControl("txtPrecio");
                CheckBox chk = (CheckBox)item.FindControl("chkServicio");

                int idServicio = int.Parse(hf.Value);

                // Busco si el prestador tiene este servicio
                var servicio = serviciosPrestador.Find(s => s.IdServicio == idServicio);

                if (servicio != null)
                {
                    chk.Checked = true;
                    txt.Text = servicio.Precio.ToString("0.##");
                }
            }
        }

        protected void btnGuardarPrestador_Click(object sender, EventArgs e)
        {
            Usuario usuarioSession = (Usuario)Session["Usuario"];
            if (usuarioSession == null)
                Response.Redirect("/Login.aspx");

            List<ServiciosPrestador> servicios = new List<ServiciosPrestador>();
            foreach (RepeaterItem item in rptServicios.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkServicio");
                HiddenField hf = (HiddenField)item.FindControl("hfIdServicio");
                TextBox txt = (TextBox)item.FindControl("txtPrecio");

                if (chk.Checked)
                {
                    if (decimal.TryParse(txt.Text, out decimal precio) && precio > 0)
                    {
                        servicios.Add(new ServiciosPrestador
                        {
                            IdServicio = int.Parse(hf.Value),
                            Precio = precio
                        });
                    }
                    else
                    {
                        lblErrorPrestador.Text = "Ingresá un precio válido para los servicios seleccionados.";
                        lblErrorPrestador.Visible = true;
                        return;
                    }
                }
            }
            
            //Limpió el Zonas Prestador sino se sigue agregando lo viejo
            usuarioSession.Prestador.ZonasPrestador = "";

            if (Session["Localidades_Seleccionadas_Por_Prestador"] != null){
                var ListaLocalidadesAux = (List<Localidades>)Session["Localidades_Seleccionadas_Por_Prestador"];
                foreach (Localidades localidad in ListaLocalidadesAux)
                {
                    usuarioSession.Prestador.ZonasPrestador += localidad.Id + ',';
                }
            }
            usuarioSession.Prestador.ZonasPrestador = usuarioSession.Prestador.ZonasPrestador.Remove(usuarioSession.Prestador.ZonasPrestador.Length - 1, 1);
             

            usuarioSession.Prestador.Servicios = servicios;
            usuarioSession.Prestador.DescripcionPrestador = txtDescripcion.Text;

            UsuarioManager manager = new UsuarioManager();
            int idPrestador = manager.ActualizarDatosPrestador(usuarioSession);

            if (idPrestador > 0)
            {
                // Actualizo IdPrestador en sesión
                usuarioSession.Prestador.IdPrestador = idPrestador;
                Session["Usuario"] = usuarioSession;
                Response.Redirect("/SeleccionarPerfil.aspx", false);
            }
            else
            {
                lblErrorPrestador.Text = "Error al guardar.";
                lblErrorPrestador.Visible = true;
            }
        }


        //private void CargarLocalidades(Usuario usuario)
        //{
        //    ddlLocalidad.Items.Clear();

        //    ddlLocalidad.Items.Add(new ListItem("-- Seleccioná tu localidad --", ""));

        //    if (!string.IsNullOrEmpty(usuario.Cliente.Localidad))
        //    {
        //        ddlLocalidad.Items.Add(new ListItem(usuario.Cliente.Localidad, usuario.Cliente.Localidad));
        //        ddlLocalidad.SelectedValue = usuario.Cliente.Localidad;
        //    }
        //}
    }
}