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
        public HistoriaYear()
        {
            this.InitializeComponent();
            this.Loaded += HistoriaYear_Loaded;
        }

        void HistoriaYear_Loaded(object sender, RoutedEventArgs e)
        {
            BotonAtras.Click += BotonAtras_Click;
            CargarJsonInApp();
        }

        private void CargarJsonInApp()
        {
            lstAnios.ItemsSource = EsquemaJson.ListaCompleta;
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


            //var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //folder = await folder.GetFolderAsync("CarpetaJson");
            //var file = await folder.GetFileAsync("esquema.json");
            //var json = await Windows.Storage.FileIO.ReadTextAsync(file);

            //var Esquem = JsonConvert.DeserializeObject<Esquema>(json);
            //this.EsquemaJson = Esquem;
        }
    }
}
