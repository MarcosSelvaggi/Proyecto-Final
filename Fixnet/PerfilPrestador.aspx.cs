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
            Usuario usuario = (Usuario)Session["Usuario"];
            if (usuario == null)
            {
                Response.Redirect("/Logearse.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CargarServicios();


                if (usuario.Prestador != null)
                {
                    //Para que la descripción no diga "No ingresado"
                    if (usuario.Prestador.DescripcionPrestador != "No ingresado")
                    {
                        txtDescripcion.Text = usuario.Prestador.DescripcionPrestador;
                    }
                    btnGuardarPrestador.Text = "Actualizar";

                    Session.Remove("Localidades_Seleccionadas_Por_Prestador");

                    if (usuario.Prestador.ZonasPrestador != null)
                    {
                        var Lista = usuario.Prestador.ZonasPrestador.Split(',');
                        Session.Add("Localidades_Ya_Ingresadas", Lista);
                        RegisterAsyncTask(new PageAsyncTask(RecuperarLocalidades));
                    }

                    //Si no tiene ZonasPrestador trae la default que es todo en 0, es para los usuarios que entran al Perfil recién registrados
                    if (usuario.Prestador.ZonasPrestador == null)
                    {
                        usuario.Prestador.ZonasPrestador = ArreglarHorarioDePrestadorNuevo();
                    }
                    CargarHorariosPrestador(usuario.Prestador.HorariosPrestador); 
                }

                RegisterAsyncTask(new PageAsyncTask(CargarProvincias));

            }
        }

        //Famoso Ctrl + C / Ctrl + V desde PerfilCliente 
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
                var url = "https://apis.datos.gob.ar/georef/api/localidades?provincia=" + ddlProvincia.SelectedValue
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
                var url = "https://apis.datos.gob.ar/georef/api/localidades?id=" + item + "&campos=basico";

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

            var servicios = manager.TraerServicios();

            rptServicios.DataSource = servicios;
            rptServicios.DataBind();

            List<ServiciosPrestador> serviciosPrestador = new List<ServiciosPrestador>();
            if (usuario.Prestador != null)
            {
                if (usuario.Prestador.IdPrestador > 0)
                    serviciosPrestador = manager.TraerServiciosPrestador(usuario.Prestador.IdPrestador);
                else
                    serviciosPrestador = usuario.Prestador.Servicios ?? new List<ServiciosPrestador>();
            }

            foreach (RepeaterItem item in rptServicios.Items)
            {
                HiddenField hf = (HiddenField)item.FindControl("hfIdServicio");
                TextBox txt = (TextBox)item.FindControl("txtPrecio");
                CheckBox chk = (CheckBox)item.FindControl("chkServicio");

                int idServicio = int.Parse(hf.Value);
                var servicio = serviciosPrestador.Find(s => s.IdServicio == idServicio);

                if (servicio != null)
                {
                    chk.Checked = true;
                    txt.Text = servicio.Precio.ToString("0.##");
                }
            }
        }
        protected void CbxChecked(object sender, EventArgs e)
        {
            ActivarHorarios();
        }

        //Habilita los Text donde van los horarios, si el check se saca, pone en 0 los valores de las horas
        protected void ActivarHorarios()
        {
            if (CbxDomingo.Checked)
            {
                HorariosDomingos.Visible = true;
            }
            else
            {
                HorariosDomingos.Visible = false;
                HorarioInicioDomingo.Text = "0";
                HorarioFinDomingo.Text = "0";
            }

            if (CbxLunes.Checked)
            {
                HorariosLunes.Visible = true;
            }
            else
            {
                HorariosLunes.Visible = false;
                HorarioInicioLunes.Text = "0";
                HorarioFinLunes.Text = "0";
            }

            if (CbxMartes.Checked)
            {
                HorariosMartes.Visible = true;
            }
            else
            {
                HorariosMartes.Visible = false;
                HorarioInicioMartes.Text = "0";
                HorarioFinMartes.Text = "0";
            }

            if (CbxMiercoles.Checked)
            {
                HorariosMiercoles.Visible = true;
            }
            else
            {
                HorariosMiercoles.Visible = false;
                HorarioInicioMiercoles.Text = "0";
                HorarioFinMiercoles.Text = "0";
            }

            if (CbxJueves.Checked)
            {
                HorariosJueves.Visible = true;
            }
            else
            {
                HorariosJueves.Visible = false;
                HorarioInicioJueves.Text = "0";
                HorarioFinJueves.Text = "0";
            }

            if (CbxViernes.Checked)
            {
                HorariosViernes.Visible = true;
            }
            else
            {
                HorariosViernes.Visible = false;
                HorarioInicioViernes.Text = "0";
                HorarioFinViernes.Text = "0";
            }

            if (CbxSabados.Checked)
            {
                HorariosSabados.Visible = true;
            }
            else
            {
                HorariosSabados.Visible = false;
                HorarioInicioSabados.Text = "0";
                HorarioFinSabados.Text = "0";
            }
        }

     
        protected void CargarHorariosPrestador(string Horarios)
        {
            if (Horarios != null)
            {
                string[] HorariosPrestador = Horarios.Split(',');

                for (int i = 0; HorariosPrestador.Length > i; i = i + 4)
                {
                    if (HorariosPrestador[i + 1].Trim() != "0")
                    {
                        if (HorariosPrestador[i].Trim() == "Domingos")
                        {
                            CbxDomingo.Checked = true;
                            HorarioInicioDomingo.Text = HorariosPrestador[i + 2].Trim();
                            HorarioFinDomingo.Text = HorariosPrestador[i + 3].Trim();
                        }
                        else if (HorariosPrestador[i].Trim() == "Lunes")
                        {
                            CbxLunes.Checked = true;
                            HorarioInicioLunes.Text = HorariosPrestador[i + 2].Trim();
                            HorarioFinLunes.Text = HorariosPrestador[i + 3].Trim();
                        }
                        else if (HorariosPrestador[i].Trim() == "Martes")
                        {
                            CbxMartes.Checked = true;
                            HorarioInicioMartes.Text = HorariosPrestador[i + 2].Trim();
                            HorarioFinMartes.Text = HorariosPrestador[i + 3].Trim();
                        }
                        else if (HorariosPrestador[i].Trim() == "Miércoles")
                        {
                            CbxMiercoles.Checked = true;
                            HorarioInicioMiercoles.Text = HorariosPrestador[i + 2].Trim();
                            HorarioFinMiercoles.Text = HorariosPrestador[i + 3].Trim();
                        }
                        else if (HorariosPrestador[i].Trim() == "Jueves")
                        {
                            CbxJueves.Checked = true;
                            HorarioInicioJueves.Text = HorariosPrestador[i + 2].Trim();
                            HorarioFinJueves.Text = HorariosPrestador[i + 3].Trim();
                        }
                        else if (HorariosPrestador[i].Trim() == "Viernes")
                        {
                            CbxViernes.Checked = true;
                            HorarioInicioViernes.Text = HorariosPrestador[i + 2].Trim();
                            HorarioFinViernes.Text = HorariosPrestador[i + 3].Trim();
                        }
                        else if (HorariosPrestador[i].Trim() == "Sábados")
                        {
                            CbxSabados.Checked = true;
                            HorarioInicioSabados.Text = HorariosPrestador[i + 2].Trim();
                            HorarioFinSabados.Text = HorariosPrestador[i + 3].Trim();
                        }
                    }
                }
            }
            ActivarHorarios();
        }

        protected bool NingunDiaSeleccionado()
        {
            return CbxDomingo.Checked == false && CbxLunes.Checked == false && CbxMartes.Checked == false &&
                 CbxMiercoles.Checked == false && CbxJueves.Checked == false && CbxViernes.Checked == false && CbxSabados.Checked == false;
        }

        protected bool VerificarHorariosCargados()
        {
            int Verificador = 0;
            if (VerificarTxt(HorarioInicioDomingo, HorarioFinDomingo) == false)
            {
                Verificador++;
            }
            if (VerificarTxt(HorarioInicioLunes, HorarioFinLunes) == false) 
            { 
                Verificador++; 
            }

            if (VerificarTxt(HorarioInicioMartes, HorarioFinMartes) == false) 
            { 
                Verificador++;
            }

            if (VerificarTxt(HorarioInicioMiercoles, HorarioFinMiercoles) == false) 
            { 
                Verificador++; 
            }

            if (VerificarTxt(HorarioInicioJueves, HorarioFinJueves) == false) 
            { 
                Verificador++; 
            }

            if (VerificarTxt(HorarioInicioViernes, HorarioFinViernes) == false) 
            { 
                Verificador++; 
            }

            if (VerificarTxt(HorarioInicioSabados, HorarioFinSabados) == false) 
            { 
                Verificador++; 
            }

            return Verificador > 0;

        }

        protected bool VerificarTxt(TextBox HorarioInicio, TextBox HorarioFin)
        {
            if (!String.IsNullOrEmpty(HorarioInicio.Text) || !String.IsNullOrEmpty(HorarioFin.Text))
            {
                try
                {
                    int HoraInicio = Int32.Parse(HorarioInicio.Text);
                    int HoraFin = Int32.Parse(HorarioFin.Text);

                    if (HoraInicio == 0 && HoraFin == 0)
                    {
                        return true;
                    }

                    return HoraInicio < HoraFin;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        protected string EscribirHorarios()
        {
            string horarios = String.Format("Domingos, {0},{1},{2}, Lunes, {3}, {4}, {5}, Martes, {6}, {7}, {8}, " +
                                            "Miércoles, {9}, {10}, {11}, Jueves, {12}, {13}, {14}, Viernes, {15}, {16}, {17}, " +
                                            "Sábados, {18}, {19}, {20}", 
                                            Convert.ToInt32(CbxDomingo.Checked), HorarioInicioDomingo.Text, HorarioFinDomingo.Text,
                                            Convert.ToInt32(CbxLunes.Checked), HorarioInicioLunes.Text, HorarioFinLunes.Text,
                                            Convert.ToInt32(CbxMartes.Checked), HorarioInicioMartes.Text, HorarioFinMartes.Text, 
                                            Convert.ToInt32(CbxMiercoles.Checked), HorarioInicioMiercoles.Text, HorarioFinMiercoles.Text,
                                            Convert.ToInt32(CbxJueves.Checked), HorarioInicioJueves.Text, HorarioFinJueves.Text,
                                            Convert.ToInt32(CbxViernes.Checked), HorarioInicioViernes.Text, HorarioFinViernes.Text,
                                            Convert.ToInt32(CbxSabados.Checked), HorarioInicioSabados.Text, HorarioFinSabados.Text);
            return horarios; 
        }


        //Método auxiliar para cuando recién se registran los prestadores
        protected string ArreglarHorarioDePrestadorNuevo()
        {
            return "Domingos,0,0,0,Lunes,0,0,0,Martes,0,0,0,Miércoles,0,0,0,Jueves,0,0,0,Viernes,0,0,0,Sábados,0,0,0";
        }


        protected void btnGuardarPrestador_Click(object sender, EventArgs e)
        {
            if (!chkAcepto.Checked)
            {
                lblErrorPrestador.Text = "Debes aceptar los términos y condiciones.";
                lblErrorPrestador.Visible = true;

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "abrirModal",
                    "Sys.Application.add_load(function() { var myModal = new bootstrap.Modal(document.getElementById('modalTerminos')); myModal.show(); });",
                    true
                );


                return;
            }

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

            var ListaLocalidadesAux = Session["Localidades_Seleccionadas_Por_Prestador"] as List<Localidades>;

            if (ListaLocalidadesAux == null || ListaLocalidadesAux.Count == 0)
            {
                LblErrorLocalidades.Visible = true;
                LblErrorLocalidades.InnerText = "Tenés que seleccionar al menos una localidad";
                return;
            }

            foreach (Localidades localidad in ListaLocalidadesAux)
            {
                usuarioSession.Prestador.ZonasPrestador += localidad.Id + ',';
            }

            usuarioSession.Prestador.ZonasPrestador = usuarioSession.Prestador.ZonasPrestador.TrimEnd(',');

            if (NingunDiaSeleccionado())
            {
                LblErrorHorariosPrestador.Visible = true;
                LblErrorHorariosPrestador.InnerText = "Debes seleccionar al menos un día";
                return;
            }
            else if (VerificarHorariosCargados())
            {
                LblErrorHorariosPrestador.Visible = true;
                LblErrorHorariosPrestador.InnerText = "Error al cargar los horarios";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                lblErrorPrestador.Text = "No puedes dejar en blanco la descripcion.";
                lblErrorPrestador.Visible = true;
                return;
            }

            else
            {
                usuarioSession.Prestador.HorariosPrestador = EscribirHorarios();
            }

            usuarioSession.Prestador.Servicios = servicios;
            usuarioSession.Prestador.DescripcionPrestador = txtDescripcion.Text;

            UsuarioManager manager = new UsuarioManager();
            int idPrestador = manager.ActualizarDatosPrestador(usuarioSession);

            

            if (idPrestador > 0)
            {
                // Actualizo IdPrestador en sesión
                usuarioSession.Prestador.IdPrestador = idPrestador;
                Session["Usuario"] = usuarioSession;
                Response.Redirect("/PerfilUsuario.aspx", false);
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