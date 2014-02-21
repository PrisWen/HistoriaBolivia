using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.CarpetaJson
{
    class ListaPreguntas
    {
        //public List<Pregunta> Preguntas { get; set; }

        public List<Pregunta>  preguntas;

        public List<Pregunta> Preguntas
        {
            get { return preguntas; }
            set { preguntas = value; }
        }
        
    }
}
