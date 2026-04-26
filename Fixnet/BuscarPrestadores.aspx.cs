using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

namespace Fixnet
{
    public partial class BuscarPrestadores : System.Web.UI.Page
    {
        UsuarioManager UsuarioManager = new UsuarioManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Solo usuarios logueados
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Logearse.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarServicios();
            }
        }

        protected void CargarServicios()
        {
            var servicios = UsuarioManager.TraerServicios();
            Session["Servicios"] = servicios;

            DdlServicio.Items.Clear();
            DdlServicio.Items.Add(new ListItem("-- Seleccioná un servicio --", ""));

            foreach (var s in servicios)
                DdlServicio.Items.Add(new ListItem(s.Nombre, s.IdServicio.ToString()));
        }

        protected void DdlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DdlServicio.SelectedValue))
            {
                RptPrestadores.DataSource = null;
                RptPrestadores.DataBind();
                LblContador.Visible = false;
                contenedorSlider.Visible = false;
                return;
            }

            BuscarPrestadoresEnLaBD();
        }

        protected void BuscarPrestadoresEnLaBD()
        {
            var usuario = (Dominio.Usuario)Session["Usuario"];
            int idServicio = int.Parse(DdlServicio.SelectedValue);

            var prestadores = UsuarioManager.TraerPrestadores(usuario, idServicio);
            prestadores.RemoveAll(x => x.IdUsuario == usuario.IdUsuario);

            // Anotar el precio del servicio filtrado en cada prestador
            // para poder usarlo en el Repeater y en el slider del lado de clente
            var vista = prestadores.ConvertAll(p =>
            {
                decimal precio = 0;
                if (p.Prestador?.Servicios != null)
                {
                    var serv = p.Prestador.Servicios.Find(s => s.IdServicio == idServicio);
                    if (serv != null) precio = serv.Precio;
                }
                return new PrestadorVista
                {
                    IdUsuario = p.IdUsuario,
                    NombreUsuario = p.NombreUsuario,
                    ApellidoUsuario = p.ApellidoUsuario,
                    EmailUsuario = p.EmailUsuario,
                    TelefonoUsuario = p.TelefonoUsuario,
                    Prestador = p.Prestador,
                    FotoPerfil = p.FotoPerfil,
                    PrecioServicio = precio
                };
            });

            int cantidad = vista.Count;
            LblContador.Text = cantidad + (cantidad == 1 ? " resultado" : " resultados");
            LblContador.Visible = true;
            contenedorSlider.Visible = cantidad > 0;

            RptPrestadores.DataSource = vista;
            RptPrestadores.DataBind();
        }

        protected string ObtenerIniciales(string nombre, string apellido)
        {
            string ini = "";
            if (!string.IsNullOrEmpty(nombre)) ini += nombre[0];
            if (!string.IsNullOrEmpty(apellido)) ini += apellido[0];
            return ini.ToUpper();
        }


        protected string ObtenerAvatar(object fotoPerfil, string nombre, string apellido)
        {
            string foto = fotoPerfil == null ? null : fotoPerfil.ToString();
            if (!string.IsNullOrEmpty(foto))
                return "<img src='" + foto + "' class='avatar-foto' alt='Foto' />";
            return "<div class='avatar-iniciales'>" + ObtenerIniciales(nombre, apellido) + "</div>";
        }

    }



}
