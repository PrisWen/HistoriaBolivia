using Newtonsoft.Json;
using OurHistory.CarpetaJson;
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
    public sealed partial class HistoriaYear : Page
    {
        string yearReceived;
        Esquema EsquemaJson;
        List<ListAnio> ListofYearAll = new List<ListAnio>();
        ListAnio contYear = new ListAnio();
        
        public HistoriaYear()
        {
            this.InitializeComponent();
            this.Loaded += HistoriaYear_Loaded;
        }

        void HistoriaYear_Loaded(object sender, RoutedEventArgs e)
        {
            BotonAtras.Click += BotonAtras_Click;
            lstAnio.SelectionChanged += lstAnio_SelectionChanged;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e.Size.Width <= 1000)
            {
                this.imgPausa.Visibility = Visibility.Visible;
                this.stack0.Visibility = Visibility.Collapsed;
                BotonAtras.Visibility = Visibility.Visible;


            }
            else
            {
                this.stack0.Visibility = Visibility.Visible;
                this.imgPausa.Visibility = Visibility.Collapsed;
                BotonAtras.Visibility = Visibility.Visible;
            }
        }

        void lstAnio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListSucesos listDeSucesosObtenido = lstAnio.SelectedItem as ListSucesos;
            lstSucesos.ItemsSource = listDeSucesosObtenido.Sucesos;
            
        }

        void BotonAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LineaDelTiempo));
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string mensaje = e.Parameter as string;
            if (!string.IsNullOrWhiteSpace(mensaje))
            {
                yearReceived = mensaje;
                year.Text = yearReceived;
            }
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("CarpetaJson");
            var file = await folder.GetFileAsync("esquema.json");
            var json = await Windows.Storage.FileIO.ReadTextAsync(file);
            var Esquem = JsonConvert.DeserializeObject<Esquema>(json);
            this.EsquemaJson = Esquem;
            //lstAnios.ItemsSource = Esquem.ListaCompleta;
            ListofYearAll = Esquem.listaCompleta;
            for (int i = 0; i < ListofYearAll.Count;i++)
            {
                if(ListofYearAll.ElementAt(i).anio.Equals(yearReceived))
                {
                    contYear = ListofYearAll.ElementAt(i);
                    break;
                }
            }
            lstAnio.ItemsSource = contYear.lstAnio;
            //lstSucesos.ItemsSource = Esquem.listaCompleta.ElementAt(0).lstAnio.ElementAt(0).Sucesos;
        }

        private void lstSucesos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
