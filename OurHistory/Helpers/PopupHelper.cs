using OurHistory.UserControls;
using OurHistory.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace OurHistory.Helpers
{
    public class PopupHelper
    {
        //Popup _popup;
        public event EventHandler<MyEventArgs> Incorrecto;
        public event EventHandler<MyEventArgs> Correcto;
        private PreguntaControl uc;
        public PopupHelper()
        {
            
        }
        public void Show(PreguntaControl control, Point location)
        {
            
            control.Correcto += control_Correcto;
            control.Incorrecto += control_Incorrecto;
            //control.Incorrecto += (s, a) =>
            //{
            //    Incorrecto(this, new MyEventArgs(a.Result));
            //};
            //control.Correcto += (s, a) =>
            //{
            //    Correcto(this, new MyEventArgs(a.Result));
            //};
            Popup _popup = new Popup();
            _popup.IsLightDismissEnabled = true;
            _popup.Child = control;
            _popup.HorizontalOffset = location.X;
            _popup.VerticalOffset = location.Y;
            _popup.Width = control.Width;
            _popup.Height = control.Height;
            _popup.IsOpen = true;
            this.uc = control;
        }

        void control_Incorrecto(object sender, MyEventArgs e)
        {
            if (Incorrecto != null)
            {
                Incorrecto(this, new MyEventArgs(e.Result));
                this.uc.Incorrecto -= control_Incorrecto;
            }
        }

        void control_Correcto(object sender, MyEventArgs e)
        {
            if (Correcto != null)
            {
                Correcto(this, new MyEventArgs(e.Result));
                this.uc.Correcto -= control_Correcto;
            }
        }
        
    }


}
