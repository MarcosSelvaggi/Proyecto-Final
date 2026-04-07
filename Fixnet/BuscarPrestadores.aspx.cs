using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace Fixnet
{
    public partial class BuscarPrestadores : System.Web.UI.Page
    {
        UsuarioManager UsuarioManager = new UsuarioManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarServicios();
            }
        }
        protected void CargarServicios()
        {
            
            var Servicios = UsuarioManager.TraerServicios();
            Session.Add("Servicios", Servicios);

            foreach (var servicio in Servicios)
            {
                DdlServicio.Items.Add(servicio.Nombre);
            }
        }


        protected void DdlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuscarPrestadoresEnLaBD();
        }

        protected void BuscarPrestadoresEnLaBD()
        {
            var Servicios = (List<Dominio.Servicio>)Session["Servicios"]; 
            var Prestadores = UsuarioManager.TraerPrestadores((Dominio.Usuario)Session["Usuario"], Servicios[DdlServicio.SelectedIndex].IdServicio);

            var Usuario = (Dominio.Usuario)Session["Usuario"];

            Prestadores.RemoveAll(x => x.IdUsuario == Usuario.IdUsuario);

            RptPrestadores.DataSource = Prestadores; 
            RptPrestadores.DataBind();
        }
    }
}