using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Fixnet
{
    public partial class SolicitarTurno : Page
    {
        //readonly string connectionString = "data source=localhost\\SQLSERVER;initial catalog=Proyecto_Final_Integrador;trusted_connection=true";
        readonly string connectionString = "data source=localhost\\SQLEXPRESS;initial catalog=Proyecto_Final_Integrador;trusted_connection=true";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int idUsuario;
                if (!int.TryParse(Request.QueryString["id"], out idUsuario))
                {
                    Response.Redirect("~/BuscarPrestadores.aspx");
                    return;
                }

                CargarPerfil(idUsuario);
            }
        }

        private void CargarPerfil(int idUsuario)
        {
            string query = @"
                SELECT
                    U.Nombre, U.Apellido, U.Email, U.Telefono,
                    P.IdPrestador, P.Descripcion,
                    ZP.IdLocalidad,
                    D.DisponibilidadPrestador
                FROM Usuario U
                INNER JOIN Prestador P ON U.IdUsuario = P.IdUsuario
                LEFT JOIN ZonasPrestador ZP ON ZP.IdPrestador = P.IdPrestador
                LEFT JOIN Disponibilidad D ON D.IdPrestador = P.IdPrestador
                WHERE U.IdUsuario = @IdUsuario";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    Response.Redirect("~/BuscarPrestadores.aspx");
                    return;
                }

                string nombre = reader["Nombre"].ToString();
                string apellido = reader["Apellido"].ToString();
                int idPrestador = Convert.ToInt32(reader["IdPrestador"]);

               
                LblIniciales.Text = ObtenerIniciales(nombre, apellido);
                LblNombre.Text = nombre + " " + apellido;
                LblEmail.Text = reader["Email"].ToString();
                LblTelefono.Text = reader["Telefono"].ToString();
                LblDescripcion.Text = reader["Descripcion"].ToString();

                
                //string zonaRaw = reader["IdLocalidad"].ToString();
                //var zonas = zonaRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //RptZonas.DataSource = zonas;
                //RptZonas.DataBind();

                Session.Add("ListaLocalidades", reader["IdLocalidad"].ToString());
                RegisterAsyncTask(new PageAsyncTask(ListarLocalidades));

                // Horarios — PARSE DE LOS HORARIOS CON COMA DE MARCOS
                string horarioRaw = reader["DisponibilidadPrestador"].ToString();
                RptHorarios.DataSource = ParsearHorarios(horarioRaw);
                RptHorarios.DataBind();

                reader.Close();


                ViewState["IdPrestador"] = idPrestador;


                // S query separada con nombre del servicio
                CargarServicios(idPrestador);
            }
        }

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

                    string zonaRaw = "";

                    foreach (var item in Lista.Localidades)
                    {
                        zonaRaw += item.Nombre + ",";
                    }

                    var zonas = zonaRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    RptZonas.DataSource = zonas;
                    RptZonas.DataBind();
                }
            }
        }

        private void CargarServicios(int idPrestador)
        {
            string query = @"
                SELECT S.IdServicio, S.Nombre AS NombreServicio, PS.PrecioHora AS Precio
                FROM PrestadorServicio PS
                INNER JOIN Servicios S ON S.IdServicio = PS.IdServicio
                WHERE PS.IdPrestador = @IdPrestador";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdPrestador", idPrestador);
                conn.Open();

                var tabla = new DataTable();
                tabla.Load(cmd.ExecuteReader());
                RptServicios.DataSource = tabla;
                RptServicios.DataBind();
            }
        }

        
        private List<HorarioVista> ParsearHorarios(string raw)
        {
            var result = new List<HorarioVista>();
            if (string.IsNullOrWhiteSpace(raw)) return result;

            var partes = raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Cada día ocupa 4 tokens: nombre, trabaja, horaInicio, horaFin
            for (int i = 0; i + 3 < partes.Length; i += 4)
            {
                result.Add(new HorarioVista
                {
                    Dia = partes[i].Trim(),
                    Trabaja = partes[i + 1].Trim() == "1",
                    HoraInicio = partes[i + 2].Trim(),
                    HoraFin = partes[i + 3].Trim()
                });
            }

            return result;
        }

        private string ObtenerIniciales(string nombre, string apellido)
        {
            string ini = "";
            if (!string.IsNullOrEmpty(nombre)) ini += nombre[0];
            if (!string.IsNullOrEmpty(apellido)) ini += apellido[0];
            return ini.ToUpper();
        }

        protected void BtnSolicitar_Click(object sender, EventArgs e)
        {
            int? idServicioSeleccionado = null;

            foreach (RepeaterItem item in RptServicios.Items)
            {
                RadioButton rb = (RadioButton)item.FindControl("rbServicio");
                HiddenField hf = (HiddenField)item.FindControl("hfIdServicio");

                if (rb.Checked)
                {
                    idServicioSeleccionado = int.Parse(hf.Value);
                    break;
                }
            }

            if (idServicioSeleccionado == null)
            {
                MostrarModal("Seleccioná un servicio", "warning");
                return;
            }

            ViewState["IdServicio"] = idServicioSeleccionado;

            ScriptManager.RegisterStartupScript(this, this.GetType(),
            "abrirModal",
            "var myModal = new bootstrap.Modal(document.getElementById('modalMensaje')); myModal.show();",
            true);
        }

        protected void BtnConfirmarSolicitud_Click(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario == null || usuario.Cliente == null || usuario.Cliente.IdCliente <= 0)
            {
                Response.Redirect("~/Logearse.aspx");
                return;
            }

            if (ViewState["IdPrestador"] == null || ViewState["IdServicio"] == null)
            {
                MostrarModal("Error en la solicitud", "error");
                return;
            }

            int idCliente = usuario.Cliente.IdCliente;
            int idPrestador = (int)ViewState["IdPrestador"];
            int idServicio = (int)ViewState["IdServicio"];

            string mensaje = txtMensaje.Text;

            UsuarioManager bd = new UsuarioManager();
            bool ok = bd.CrearSolicitudTurno(idCliente, idPrestador, idServicio, mensaje);

            if (ok)
            {
                //MostrarModal("Solicitud enviada correctamente", "success");
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                "modalYRedirect",
                @"
                var modal = document.getElementById('modalMensajeSistema');
                document.getElementById('modalHeader').className = 'modal-header text-white bg-success';
                document.getElementById('modalIcon').innerText = '✔️';
                modal.querySelector('.modal-body').innerText = 'Solicitud enviada correctamente. Serás redirigido a Mis Turnos...';
                var m = new bootstrap.Modal(modal);
                m.show();

                setTimeout(function() {
                    window.location.href = 'MisTurnos.aspx';
                }, 3000);
                ",
                true);
            }
            else
            {
                MostrarModal("Error al enviar solicitud", "warning");
            }
        }

        private void MostrarModal(string mensaje, string tipo)
        {
            LblMensajeSistema.Text = mensaje;

            string color = "bg-primary";
            string icono = "ℹ️";

            switch (tipo)
            {
                case "success":
                    color = "bg-success";
                    icono = "✔️";
                    break;

                case "error":
                    color = "bg-danger";
                    icono = "❌";
                    break;

                case "warning":
                    color = "bg-warning text-dark";
                    icono = "⚠️";
                    break;

                case "info":
                    color = "bg-primary";
                    icono = "ℹ️";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "modalSistema",
                $@"
                var modal = document.getElementById('modalMensajeSistema');
                document.getElementById('modalHeader').className = 'modal-header text-white {color}';
                document.getElementById('modalIcon').innerText = '{icono}';
                new bootstrap.Modal(modal).show();
                ",
                true);
        }

    }

    
}