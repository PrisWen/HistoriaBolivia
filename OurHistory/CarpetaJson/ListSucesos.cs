using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.CarpetaJson
{
    class ListSucesos
    {
        public string pertenece { get; set; }
        public List<string> Sucesos { get; set; }

        public string Pertenece
        {
            get { return pertenece; }
            set { pertenece = value; }
        }
        public List<string> sucesos
        {
            get { return Sucesos; }
            set { Sucesos = value; }
        }
    }
}
