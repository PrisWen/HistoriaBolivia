using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.CarpetaJson
{
    class ListSucesos
    {
        private string pertenece { get; set; }
        private List<string> Sucesos { get; set; }

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
