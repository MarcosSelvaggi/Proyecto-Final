using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Fixnet
{
    public partial class PerfilPrestador : System.Web.UI.Page
    {
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

                CargarLocalidades(usuario);

                if (usuario.Prestador != null)
                {
                    txtDescripcion.Text = usuario.Prestador.DescripcionPrestador;
                    btnGuardarPrestador.Text = "Actualizar";
                }

                CargarServicios();
            }
        }

        private void CargarLocalidades(Usuario usuario)
        {
            ddlLocalidad.Items.Clear();

            ddlLocalidad.Items.Add(new ListItem("-- Seleccioná tu localidad --", ""));

            if (!string.IsNullOrEmpty(usuario.Cliente.Localidad))
            {
                ddlLocalidad.Items.Add(new ListItem(usuario.Cliente.Localidad, usuario.Cliente.Localidad));
                ddlLocalidad.SelectedValue = usuario.Cliente.Localidad;
            }

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
    }
}