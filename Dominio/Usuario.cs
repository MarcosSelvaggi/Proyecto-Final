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
        public string FotoPerfil;
        public Cliente Cliente;
        public Prestador Prestador;

        public Usuario()
        {
            Cliente = new Cliente();
            Prestador = new Prestador();
        }
    }
}
