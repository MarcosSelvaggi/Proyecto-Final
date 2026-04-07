using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Provincias
    {
        public string Id {  get; set; }
        public string Nombre { get; set; } 
    }

    public class ListaDeProvincias
    {
        public List<Provincias> Provincias {  get; set; }
    }
}
