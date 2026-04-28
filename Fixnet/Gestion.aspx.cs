using Dominio;
using Negocio;
using System;
using System.Data;
using System.Text;
using System.Web.UI;

namespace Fixnet
{
    public partial class Gestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario == null)
            {
                Response.Redirect("~/Logearse.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            bool esPrestador = !string.IsNullOrEmpty(usuario.Prestador?.DescripcionPrestador)
                               && usuario.Prestador.DescripcionPrestador != "No ingresado";

            if (!esPrestador)
            {
                Response.Redirect("~/PerfilUsuario.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            int mes = DateTime.Now.Month;
            int anio = DateTime.Now.Year;

            // Si viene de cambiar mes via postback
            if (IsPostBack && !string.IsNullOrEmpty(hfMesAnio.Value) && hfMesAnio.Value.Contains("|"))
            {
                var partes = hfMesAnio.Value.Split('|');
                anio = int.Parse(partes[0]);
                mes = int.Parse(partes[1]);
            }

            CargarCalendario(usuario.Prestador.IdPrestador, anio, mes);
        }

        private void CargarCalendario(int idPrestador, int anio, int mes)
        {
            UsuarioManager bd = new UsuarioManager();

            // Disponibilidad
            hfDisponibilidad.Value = bd.TraerDisponibilidadPrestador(idPrestador);

            // Turnos del mes → serializar a JSON simple
            DataTable dt = bd.TraerTurnosPrestadorPorMes(idPrestador, anio, mes);
            hfTurnosJson.Value = DataTableToJson(dt);
        }

        private string DataTableToJson(DataTable dt)
        {
            var sb = new StringBuilder("[");
            foreach (DataRow row in dt.Rows)
            {
                string fecha = Convert.ToDateTime(row["FechaProgramada"]).ToString("yyyy-MM-dd");
                string cliente = EscapeJson(row["NombreCliente"] + " " + row["ApellidoCliente"]);
                string servicio = EscapeJson(row["Servicio"].ToString());

                sb.Append("{");
                sb.Append($"\"fecha\":\"{fecha}\",");
                sb.Append($"\"cliente\":\"{cliente}\",");
                sb.Append($"\"servicio\":\"{servicio}\"");
                sb.Append("},");
            }
            if (sb.Length > 1) sb.Length--;
            sb.Append("]");
            return sb.ToString();
        }

        private string EscapeJson(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
