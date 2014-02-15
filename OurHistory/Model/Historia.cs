using OurHistory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.Model
{
    public class Historia : BindableBase
    {
        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set { SetProperty(ref _titulo, value); }
        }

        private string _descripcion;
        public string Descripcion
        {
            get { return _descripcion; }
            set { SetProperty(ref _descripcion, value); }
        }

        private List<Uri> _fotos;
        public List<Uri> Fotos
        {
            get { return _fotos; }
            set { SetProperty(ref _fotos, value); }
        }

        //private Uri _fotos;
        //public Uri Fotos
        //{
        //    get { return _fotos; }
        //    set { SetProperty(ref _fotos, value); }
        //}



    }
}