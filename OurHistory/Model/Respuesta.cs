using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.Model
{
    public class Respuesta
    {
        private string opcion;

        public string Opcion
        {
            get { return opcion; }
            set { opcion = value; }
        }
        public Respuesta()
        {
            opcion = "";
        }
        public Respuesta(string resp)
        {
            opcion = resp;
        }

    }
}
