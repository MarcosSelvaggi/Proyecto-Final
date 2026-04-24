using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PrestadorVista
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string TelefonoUsuario { get; set; }
        
        public string FotoPerfil { get; set; }
        public Dominio.Prestador Prestador { get; set; }
        public decimal PrecioServicio { get; set; }
    }
}
