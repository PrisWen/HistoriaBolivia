using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace OurHistory.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class LineaDelTiempo : Page
    {
        string anio = "1810";
        public LineaDelTiempo()
        {
            this.InitializeComponent();
            this.Loaded += LineaDelTiempo_Loaded;
        }

        void LineaDelTiempo_Loaded(object sender, RoutedEventArgs e)
        {
            //usuarioTime.Text = (sliderLineTime.Value).ToString();
            sliderLineTime.ValueChanged += sliderLineTime_ValueChanged;
            BotonAtras.Click += BotonAtras_Click;
            gohistory.Click += gohistory_Click;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e.Size.Width <= 1000)
            {
                this.imgPausa.Visibility = Visibility.Visible;
                this.stack0.Visibility = Visibility.Collapsed;
                BotonAtras.Visibility = Visibility.Collapsed;

            }
            else
            {
                this.stack0.Visibility = Visibility.Visible;
                this.imgPausa.Visibility = Visibility.Collapsed;
                BotonAtras.Visibility = Visibility.Visible;
            }
        }

        void gohistory_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HistoriaYear), anio);
        }

        void BotonAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        void sliderLineTime_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            usuarioTime.Text = (sliderLineTime.Value).ToString();
            lineImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Picture/jjj" + usuarioTime.Text.ToString()+".jpg", UriKind.RelativeOrAbsolute));
            anio = (int.Parse((sliderLineTime.Value).ToString())).ToString();
        }
        void cargandoDatosdeXAnio()
        {

        }
    }
}
