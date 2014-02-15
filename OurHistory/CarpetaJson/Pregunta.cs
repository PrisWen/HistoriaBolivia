using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.CarpetaJson
{
    public class Pregunta
    {

        private int codigo;
        private string descripcion;
        private List<string> respuestasO { get; set; }
        private string respuestaC;
        private string detalleRespuesta;
        private string nivel;
        private int puntaje;

        public Pregunta()
        {
            this.codigo = 0;
            this.descripcion = "";
            this.respuestasO = new List<string>(); ;
            this.respuestaC = "";
            this.detalleRespuesta = "";
            this.nivel = "";
            this.puntaje = 0;
        }

        public Pregunta(int codigo, string descripcion, List<string> respuestasO, string respuestaC, string nivel, int puntaje)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.respuestasO = respuestasO;
            this.respuestaC = respuestaC;
            this.nivel = nivel;
            this.puntaje = puntaje;
        }


        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }


        public string RespuestaC
        {
            get { return respuestaC; }
            set { respuestaC = value; }
        }

        public string Nivel
        {
            get { return nivel; }
            set { nivel = value; }
        }
        public string DetalleRespuesta
        {
            get { return detalleRespuesta; }
            set { detalleRespuesta = value; }
        }

        public int Puntaje
        {
            get { return puntaje; }
            set { puntaje = value; }
        }
        public List<string> RespuestasO
        {
            get { return respuestasO; }
            set { respuestasO = value; }
        }
    }
}
