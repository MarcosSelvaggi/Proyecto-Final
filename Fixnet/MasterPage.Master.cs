using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fixnet
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] != null)
            {   
                //Esto refresca el tiempo de la sesión cada vez que el usuario interactua con alguna página, para evitar que lo eche cuando está usando la app
                Session.Timeout = 20; 
            }
        }

        protected void btn_CerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("/Default.aspx", false);
            return;
        }
    }
}