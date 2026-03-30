using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Localidades
    {
        public string Id {  get; set; }
        public string Nombre { get; set; }
    }

    public class ListaDeLocalidades
    {
        public List<Localidades> Localidades { get; set; }
    }
}
