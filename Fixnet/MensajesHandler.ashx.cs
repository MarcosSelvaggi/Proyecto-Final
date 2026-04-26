using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Dominio;
using Negocio;

namespace Fixnet
{
    public class MensajesHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Cache.SetNoStore();

            var usuario = context.Session["Usuario"] as Usuario;
            if (usuario == null) { context.Response.Write("null"); return; }

            var mgr = new UsuarioManager();
            string accion = context.Request.QueryString["accion"];

            if (accion == "noLeidos")
            {
                int n = mgr.ContarMensajesNoLeidos(usuario.IdUsuario);
                context.Response.Write(n.ToString());
                return;
            }

            else if (accion == "mensajes")
            {
                int idConv;
                if (!int.TryParse(context.Request.QueryString["conv"], out idConv))
                { context.Response.Write("[]"); return; }

                DataTable dt = mgr.TraerMensajesConversacion(idConv);
                var sb = new StringBuilder("[");
                bool primero = true;
                foreach (DataRow r in dt.Rows)
                {
                    if (!primero) sb.Append(",");
                    primero = false;
                    bool leido = r["Leido"] != DBNull.Value && Convert.ToBoolean(r["Leido"]);
                    string foto = r["FotoPerfil"] == DBNull.Value ? "" : Escapar(r["FotoPerfil"].ToString());
                    sb.Append("{");
                    sb.AppendFormat("\"idEmisor\":{0},", r["IdEmisor"]);
                    sb.AppendFormat("\"texto\":\"{0}\",", Escapar(r["Texto"].ToString()));
                    sb.AppendFormat("\"hora\":\"{0}\",", Convert.ToDateTime(r["FechaEnvio"]).ToString("HH:mm"));
                    sb.AppendFormat("\"leido\":{0},", leido ? "true" : "false");
                    sb.AppendFormat("\"foto\":\"{0}\",", foto);
                    sb.AppendFormat("\"nombre\":\"{0}\",", Escapar(r["Nombre"].ToString()));
                    sb.AppendFormat("\"apellido\":\"{0}\"", Escapar(r["Apellido"].ToString()));
                    sb.Append("}");
                }
                sb.Append("]");
                context.Response.Write(sb.ToString());
                return;
            }
            else if (accion == "eliminar")
            {
                int idConv;
                if (!int.TryParse(context.Request.QueryString["conv"], out idConv))
                { context.Response.Write("false"); return; }

                bool ok = mgr.EliminarConversacion(idConv, usuario.IdUsuario);
                context.Response.ContentType = "text/plain";
                context.Response.Write(ok ? "true" : "false");
                return;
            }
            else if (accion == "marcarLeidos")
            {
                int idConv;
                if (!int.TryParse(context.Request.QueryString["conv"], out idConv))
                { context.Response.Write("false"); return; }

                mgr.MarcarMensajesLeidos(idConv, usuario.IdUsuario);
                context.Response.Write("true");
                return;
            }
            else if (accion == "conversaciones")
            {
                DataTable dt = mgr.TraerConversacionesUsuario(usuario.IdUsuario);
                var sb = new StringBuilder("[");
                bool primero = true;
                foreach (DataRow r in dt.Rows)
                {
                    if (!primero) sb.Append(",");
                    primero = false;
                    sb.Append("{");
                    sb.AppendFormat("\"idConversacion\":{0},", r["IdConversacion"]);
                    sb.AppendFormat("\"otroNombre\":\"{0}\",", Escapar(r["OtroNombre"].ToString()));
                    sb.AppendFormat("\"otroApellido\":\"{0}\",", Escapar(r["OtroApellido"].ToString()));
                    sb.AppendFormat("\"servicio\":\"{0}\",", Escapar(r["Servicio"].ToString()));
                    sb.AppendFormat("\"otroFoto\":\"{0}\",", r["OtroFoto"] == DBNull.Value ? "" : Escapar(r["OtroFoto"].ToString()));
                    sb.AppendFormat("\"ultimoMensaje\":\"{0}\",", Escapar(r["UltimoMensaje"] == DBNull.Value ? "" : r["UltimoMensaje"].ToString()));
                    sb.AppendFormat("\"noLeidos\":{0},", r["NoLeidos"] == DBNull.Value ? 0 : Convert.ToInt32(r["NoLeidos"]));
                    // Fecha como string formateada
                    string fecha = "";
                    if (r["FechaUltimo"] != DBNull.Value)
                    {
                        DateTime d = Convert.ToDateTime(r["FechaUltimo"]);
                        fecha = d.Date == DateTime.Today ? d.ToString("HH:mm") : d.ToString("dd/MM");
                    }
                    sb.AppendFormat("\"fecha\":\"{0}\"", fecha);
                    sb.Append("}");
                }
                sb.Append("]");
                context.Response.Write(sb.ToString());
                return;
            }
            context.Response.Write("null");
        }

        private string Escapar(string s)
        {
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\r", "")
                    .Replace("\n", "\\n");
        }

        public bool IsReusable => false;
    }
}