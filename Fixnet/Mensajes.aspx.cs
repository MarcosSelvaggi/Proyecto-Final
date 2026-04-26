using System;
using System.Data;
using System.Web.UI;
using Dominio;
using Negocio;

namespace Fixnet
{
    public partial class Mensajes : System.Web.UI.Page
    {
        UsuarioManager UsuarioManager = new UsuarioManager();
        protected Usuario UsuarioActual
        {
            get { return Session["Usuario"] as Usuario; }
        }

        protected int ConvSeleccionada
        {
            get
            {
                int id = 0;
                if (Request.QueryString["conv"] != null)
                    int.TryParse(Request.QueryString["conv"], out id);
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioActual == null)
            {
                Response.Redirect("~/Logearse.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarConversaciones();

                if (ConvSeleccionada > 0)
                    CargarChat(ConvSeleccionada);
            }
        }

        protected void CargarConversaciones()
        {
            DataTable dt = UsuarioManager.TraerConversacionesUsuario(UsuarioActual.IdUsuario);

            if (dt.Rows.Count == 0)
                LblSinConversaciones.Visible = true;
            else
            {
                RptConversaciones.DataSource = dt;
                RptConversaciones.DataBind();
            }
        }

        protected void CargarChat(int idConversacion)
        {
            UsuarioManager.MarcarMensajesLeidos(idConversacion, UsuarioActual.IdUsuario);

            // Notificar al otro usuario que sus mensajes fueron leídos
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MensajesHub>();
            context.Clients.Group("conv-" + idConversacion).mensajesLeidos();

            DataTable mensajes = UsuarioManager.TraerMensajesConversacion(idConversacion);
            DataTable convs = UsuarioManager.TraerConversacionesUsuario(UsuarioActual.IdUsuario);

            DataRow conv = null;
            foreach (DataRow row in convs.Rows)
                if (Convert.ToInt32(row["IdConversacion"]) == idConversacion)
                { conv = row; break; }

            if (conv != null)
            {
                LblChatNombre.Text = conv["OtroNombre"] + " " + conv["OtroApellido"];
                LblChatServicio.Text = conv["Servicio"].ToString();
            }

            HfConvId.Value = idConversacion.ToString();

            RptMensajes.DataSource = mensajes;
            RptMensajes.DataBind();

            PnlVacio.Visible = false;
            PnlChat.Visible = true;
        }
        
        //Helpers para el chat

        protected string ObtenerVisto(object leido, object idEmisor)
        {
            if (!EsMio(idEmisor == DBNull.Value ? 0 : Convert.ToInt32(idEmisor))) return "";

            bool fueLeido = leido != null && leido != DBNull.Value && Convert.ToBoolean(leido);

            if (fueLeido)
                return "<span class='msg-visto leido' title='Visto'>✓✓</span>";
            else
                return "<span class='msg-visto enviado' title='Enviado'>✓✓</span>";
        }
        protected bool EsSeleccionada(int idConv)
        {
            return idConv == ConvSeleccionada;
        }

        protected bool EsMio(int idEmisor)
        {
            return UsuarioActual != null && idEmisor == UsuarioActual.IdUsuario;
        }

        protected string ObtenerIniciales(string nombre, string apellido)
        {
            string ini = "";
            if (!string.IsNullOrEmpty(nombre)) ini += nombre[0];
            if (!string.IsNullOrEmpty(apellido)) ini += apellido[0];
            return ini.ToUpper();
        }

        protected string ObtenerAvatarConv(object foto, string nombre, string apellido, int noLeidos)
        {
            string clase = noLeidos > 0 ? "avatar-conv has-badge" : "avatar-conv";
            string f = foto == DBNull.Value ? null : foto?.ToString();
            string img = !string.IsNullOrEmpty(f)
                ? $"<img src='{f}' class='avatar-foto' alt='Foto' />"
                : $"<div class='avatar-iniciales'>{ObtenerIniciales(nombre, apellido)}</div>";
            return $"<div class='{clase}'>{img}</div>";
        }

        protected string ObtenerAvatarMsg(object foto, string nombre, string apellido)
        {
            string f = foto == DBNull.Value ? null : foto?.ToString();
            return !string.IsNullOrEmpty(f)
                ? $"<img src='{f}' class='avatar-msg' alt='Foto' />"
                : $"<div class='avatar-msg avatar-iniciales-sm'>{ObtenerIniciales(nombre, apellido)}</div>";
        }

        protected string TruncarTexto(object texto)
        {
            if (texto == null || texto == DBNull.Value) return "Sin mensajes";
            string t = texto.ToString();
            return t.Length > 45 ? t.Substring(0, 45) + "…" : t;
        }

        protected string FormatearFecha(object fecha)
        {
            if (fecha == null || fecha == DBNull.Value) return "";
            DateTime d = Convert.ToDateTime(fecha);
            return d.Date == DateTime.Today ? d.ToString("HH:mm") : d.ToString("dd/MM");
        }

        protected string FormatearHora(object fecha)
        {
            if (fecha == null || fecha == DBNull.Value) return "";
            return Convert.ToDateTime(fecha).ToString("HH:mm");
        }
    }
}