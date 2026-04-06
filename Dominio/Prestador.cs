using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Prestador
    {
        public int IdPrestador {  get; set; }
        public string DescripcionPrestador {  set; get; }

        public List<int> IdServicios { get; set; }
        public List<ServiciosPrestador> Servicios { get; set; }
    }
}
