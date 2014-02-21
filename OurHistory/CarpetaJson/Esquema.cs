using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.CarpetaJson
{
    class Esquema
    {
        public List<ListAnio> listaCompleta;

        public List<ListAnio> ListaCompleta
        {
            get { return listaCompleta; }
            set { listaCompleta = value; }
        }
    }
}
