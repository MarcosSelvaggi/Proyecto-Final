using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fixnet
{
    public partial class PerfilPrestador : System.Web.UI.Page
    {   
        private List<String> ListaServicios = new List<String>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListaServicios.Add(ddlServicios.SelectedValue);
            Session.Add("ServiciosOfrecidos", ListaServicios);
        }
    }
}