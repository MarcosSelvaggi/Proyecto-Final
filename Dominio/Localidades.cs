using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Localidades : IEquatable<Localidades>
    {
        public string Id {  get; set; }
        public string Nombre { get; set; }

        public bool Equals(Localidades other)
        {
            return (this.Nombre == other.Nombre);
        }
    }

    public class ListaDeLocalidades
    {
        public List<Localidades> Localidades { get; set; }

    }
}
