using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fixnet
{
    public partial class SeleccionarPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario == null)
            {
                Response.Redirect("/Logearse.aspx");
                return;
            }

            if (TieneAlgunPerfil(usuario))
            {
                Response.Redirect("/PerfilUsuario.aspx");
            }
        }

        private bool TieneAlgunPerfil(Usuario usuario)
        {
            return EsValido(usuario.Cliente?.DireccionCliente) ||
                   EsValido(usuario.Prestador?.DescripcionPrestador);
        }

        private bool EsValido(string valor)
        {
            return !string.IsNullOrEmpty(valor) && valor != "No ingresado";
        }
    }
}