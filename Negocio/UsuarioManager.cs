using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Servicios; 

namespace Negocio
{
    public class UsuarioManager
    {
        Usuario Usuario; 

        public int RegistrarUsuario(Usuario Usuario)
        {
            BD conexion = new BD();
            return conexion.RegistrarUsuarioBD(Usuario); 
        }


        /*
        public Usuario LogearUsuario(string Email, string Password)
        {
            

            return Usuario;
        }*/

    }
}
