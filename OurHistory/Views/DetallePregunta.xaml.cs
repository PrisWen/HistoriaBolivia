
using Newtonsoft.Json;
using OurHistory.CarpetaJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace OurHistory.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class DetallePregunta : Page
    {
        ListaPreguntas ListaPreg = new ListaPreguntas();
        public DetallePregunta()
        {
            this.InitializeComponent();
            Loaded += DetallePregunta_Loaded;
        }

        void DetallePregunta_Loaded(object sender, RoutedEventArgs e)
        {
            ReadJson();
        }
        async void ReadJson()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("CarpetaJson");
            var file = await folder.GetFileAsync("ListaPreguntas.json");
            var json = await Windows.Storage.FileIO.ReadTextAsync(file);
            var LstPregunta = JsonConvert.DeserializeObject<ListaPreguntas>(json);
            this.DataContext = LstPregunta;
            ListaPreg = LstPregunta;
            lstPreguntas.ItemsSource = ListaXNivel(ListaPreg.Preguntas, "3");
            //lstPreguntas.ItemsSource = LstPregunta.Preguntas;
        }
        public List<Pregunta> ListaXNivel(List<Pregunta> ListAll, string nivelX)
        {
            List<Pregunta> listPreg = new List<Pregunta>();
            int i=0;
            for(i=0; i<ListAll.Count ; i++)
            {
                if(ListAll.ElementAt(i).Nivel.Equals(nivelX))
                {
                    listPreg.Add(ListAll.ElementAt(i));
                }
            }
            return listPreg;
        }
        //protected async override void OnNavigatedTo(NavigationEventArgs e)
        //{} 
    }
}
