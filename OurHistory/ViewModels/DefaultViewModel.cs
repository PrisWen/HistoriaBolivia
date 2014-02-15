using OurHistory.Common;
using OurHistory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.ViewModels
{
    public class DefaultViewModel : BindableBase
    {
        private Historia history;

        public Historia History
        {
            get { return history; }
            set { SetProperty(ref history, value); }
        }

        public DefaultViewModel()
        {

            //ColeccionFoto listaFotos = new ColeccionFoto();

            List<Uri> fotitos = new List<Uri>();

            //fotitos.Add(new Uri("ms-appx:///Picture/Fondo.jpg"));
            //listaFotos.MisFotos = fotitos;
            fotitos.Add(new Uri("ms-appx:///Assets/Picture/cultura.jpg"));
            fotitos.Add(new Uri("ms-appx:///Assets/Picture/Fondo.jpg"));


            History = new Historia()
            {
                Titulo = "titulo",
                Descripcion = "Cupcake ipsum dolor sit amet I love topping marshmallow. I love marshmallow lemon drops danish sweet roll powder applicake lemon drops. Liquorice croissant chupa chups cookie toffee icing pastry sweet roll chocolate. Icing icing tiramisu donut unerdwear.com jelly dragée chocolate tootsie roll. Icing toffee gummi bears ice cream tart pudding jujubes jelly-o cheesecake. Unerdwear.com croissant I love lemon drops I love carrot cake. I love pudding tiramisu jelly beans danish. Gummi bears powder muffin gummies halvah cake. Applicake topping pastry jujubes cupcake danish jelly-o sugar plum biscuit. Brownie donut croissant sugar plum pastry I love soufflé powder. Sesame snaps gummies biscuit I love jelly sweet. Croissant sesame snaps jujubes marshmallow I love lemon drops tart. Marshmallow gummi bears topping tootsie roll tart cheesecake marshmallow cupcake. Fruitcake caramels muffin gingerbread halvah gummi bears wafer ice cream chocolate bar. Tiramisu cheesecake caramels jujubes croissant. Chupa chups jelly beans tootsie roll. Chocolate ice cream topping cake powder sweet cheesecake. Danish gingerbread gummi bears donut topping candy canes chupa chups oat cake. Donut brownie donut dessert. Dragée I love soufflé powder sugar plum dragée fruitcake. Chocolate chupa chups bear claw. Cupcake lemon drops halvah applicake cheesecake apple pie. Marzipan carrot cake unerdwear.com muffin pie I love fruitcake apple pie. Sweet I love bonbon. I love jelly-o tart. Tiramisu ice cream chocolate bar halvah dragée wafer tiramisu lollipop wafer. Soufflé candy canes pie unerdwear.com chocolate cake ice cream. Gummi bears cookie bonbon pie jujubes. ",
                Fotos = fotitos
            };
        }

    }
}
