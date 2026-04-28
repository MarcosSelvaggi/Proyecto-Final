using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Fixnet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        [System.Web.Services.WebMethod]
        public static int ContarPrestadores(FiltroPrestadoresChat datos)
        {
            UsuarioManager bd = new UsuarioManager();

            int idServicio = bd.ObtenerIdServicio(datos.servicio);
            string localidad = datos.localidad;

            return bd.ContarPrestadores(idServicio, localidad);
        }
        [System.Web.Services.WebMethod]
        public static List<object> TraerServicios()
        {
            UsuarioManager mgr = new UsuarioManager();
            return mgr.TraerServicios()
                .Select(s => new { id = s.IdServicio, nombre = s.Nombre })
                .Cast<object>()
                .ToList();
        }
    }
}