using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario
    {
        public int IdUsuario;
        public string EmailUsuario;
        public string PasswordUsuario;
        public bool UsuarioActivo;
        public string NombreUsuario;
        public string ApellidoUsuario;
        public string TelefonoUsuario;
        public Cliente Cliente;
        public Prestador Prestador;

        public Usuario()
        {
            Cliente = new Cliente();
            Prestador = new Prestador();
        }


        //Propiedades necesarias para el Eval en BuscarPrestadores.aspx, leer lo siguiente -> https://stackoverflow.com/questions/14901542/why-databinding-cant-find-a-property-which-exist
        public string Nombre
        {
            get {  return NombreUsuario; }
            set { NombreUsuario = value; }
        }

        public string Apellido
        {
            get { return ApellidoUsuario; }
            set { ApellidoUsuario = value;}
        }
    }
}
