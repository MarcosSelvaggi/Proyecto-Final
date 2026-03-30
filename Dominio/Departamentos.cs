using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Departamentos
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }

    public class ListaMunicipios
    {
        public List<Departamentos> Departamentos { get; set; }
    }
}
