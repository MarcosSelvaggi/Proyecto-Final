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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Logearse.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                int idUsuario;
                if (!int.TryParse(Request.QueryString["id"], out idUsuario))
                {
                    Response.Redirect("~/BuscarPrestadores.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                CargarPerfil(idUsuario);
            }
        }

        private void CargarPerfil(int idUsuario)
        {
            UsuarioManager bd = new UsuarioManager();
            DataRow row = bd.TraerPerfilPrestador(idUsuario);

            if (row == null)
            {
                Response.Redirect("~/BuscarPrestadores.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string nombre = row["Nombre"].ToString();
            string apellido = row["Apellido"].ToString();
            int idPrestador = Convert.ToInt32(row["IdPrestador"]);
            string fotoPerfil = row["FotoPerfil"] == DBNull.Value ? null : row["FotoPerfil"].ToString();

            if (!string.IsNullOrEmpty(fotoPerfil))
            {
                ImgFotoPrestador.ImageUrl = fotoPerfil;
                ImgFotoPrestador.Visible = true;
                divInicialesPrestador.Visible = false;
            }
            else
            {
                LblIniciales.Text = ObtenerIniciales(nombre, apellido);
                ImgFotoPrestador.Visible = false;
                divInicialesPrestador.Visible = true;
            }

            LblNombre.Text = nombre + " " + apellido;
            LblEmail.Text = row["Email"].ToString();
            LblTelefono.Text = row["Telefono"].ToString();
            LblDescripcion.Text = row["Descripcion"].ToString();
            if (row["Calificacion"].ToString() != "0")
            {
                LblPuntuacionPrestador.Text = row["Calificacion"].ToString();
                LblPuntuacionPrestador.Visible = true;
                LblEstrellaPuntuacion.Visible = true;
                TituloPuntuacionPrestador.Visible = true;
            }

            Session.Add("ListaLocalidades", row["IdLocalidad"].ToString());
            RegisterAsyncTask(new PageAsyncTask(ListarLocalidades));

            RptHorarios.DataSource = ParsearHorarios(row["DisponibilidadPrestador"].ToString());
            RptHorarios.DataBind();

            ViewState["IdPrestador"] = idPrestador;
            CargarServicios(idPrestador);
        }

        private void CargarServicios(int idPrestador)
        {
            UsuarioManager bd = new UsuarioManager();
            RptServicios.DataSource = bd.TraerServiciosDePrestador(idPrestador);
            RptServicios.DataBind();
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
                Response.Redirect("~/Logearse.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
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
            string mensaje = txtMensaje.Text.Trim();

            UsuarioManager bd = new UsuarioManager();
            int idTurno = bd.CrearSolicitudTurno(idCliente, idPrestador, idServicio, mensaje);

            if (idTurno > 0)
            {
                // Crea la conversación y manda el mensaje inicial si escribió algo en el modal
                if (!string.IsNullOrWhiteSpace(mensaje))
                {
                    int idConv = bd.ObtenerOCrearConversacion(idTurno, idCliente, idPrestador);
                    bd.EnviarMensaje(idConv, usuario.IdUsuario, mensaje);
                    MensajesHub.NotificarMensajeExterno(
                        idConv,
                        usuario.IdUsuario,
                        mensaje,
                        usuario.NombreUsuario,
                        usuario.ApellidoUsuario,
                        bd
                    );
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                "modalYRedirect",
                @"
                var modal = document.getElementById('modalMensajeSistema');
                document.getElementById('modalHeader').className = 'modal-header';
                document.getElementById('modalIcon').innerText = '✔️';
                modal.querySelector('.modal-body').innerText = 'Solicitud enviada correctamente. Serás redirigido a Mis Turnos...';
                var m = new bootstrap.Modal(modal);
                m.show();
                setTimeout(function() { window.location.href = 'MisTurnos.aspx'; }, 3000);
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

            string icono = "ℹ️";

            switch (tipo)
            {
                case "success":
                    icono = "✔️";
                    break;

                case "error":
                    icono = "❌";
                    break;

                case "warning":
                    icono = "⚠️";
                    break;

                case "info":
                    icono = "ℹ️";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "modalSistema",
                $@"
                var modal = document.getElementById('modalMensajeSistema');
                document.getElementById('modalHeader').className = 'modal-header';
                document.getElementById('modalIcon').innerText = '{icono}';
                new bootstrap.Modal(modal).show();
                ",
                true);
        }

        protected bool MostrarCalificaciones(float Calificacion)
        {
            return Calificacion != 0;
        }

    }


}