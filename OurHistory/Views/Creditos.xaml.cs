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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace OurHistory.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Creditos : Page
    {
        public Creditos()
        {
            this.InitializeComponent();
            this.Loaded += Creditos_Loaded;
        }
        void Creditos_Loaded(object sender, RoutedEventArgs e)
        {
            CargandoCreditos();
            Storyboard1.Begin();
            BotonAtras.Click += BotonAtras_Click;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e.Size.Width <= 1000)
            {
                
                this.grid0.Visibility = Visibility.Collapsed;
                this.imgPausa.Visibility = Visibility.Visible;

            }
            else
            {
                this.grid0.Visibility = Visibility.Visible;
                this.imgPausa.Visibility = Visibility.Collapsed;
            }
        }
        void CargandoCreditos()
        {
            txtCredits2.Text = "DISEÑADORAS\n\nEspinoza Tito Priscila Wendy\nGuerrero Quiñajo Milenka Anahi\nParisaca Quispe Alison\n\n\nDESARROLLADORAS\n\nEspinoza Tito Priscila Wendy\nGuerrero Quiñajo Milenka Anahi\nParisaca Quispe Alison";
            txtFuentes.Text = "FUENTE\n\nHistoria de Bolivia\nOctava Edición\nCarlos D. Mesa Gisbert\nJose de Mesa\nTeresa Gisbert\nEditorial Gisbert\nLibro V - La Independencia\n1800 - 1820";
        }
        void BotonAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
            //this.Frame.Navigate(typeof(MainPage));
        }
    }
}
