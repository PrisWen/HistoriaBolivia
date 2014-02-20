using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.CarpetaJson
{
    class ListAnio
    {
        private string anio { get; set; }
        private List<ListSucesos> lstAnio { get; set; }
        public string Anio
        {
            get { return anio; }
            set { anio = value; }
        }
        public List<ListSucesos> LstAnio
        {
            get { return lstAnio; }
            set { lstAnio = value; }
        }
    }
}
