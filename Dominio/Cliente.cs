using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Provincia {  get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string IdLocalidad {  get; set; }
        public string DireccionCliente { get; set; }
    }
}
