using OurHistory.Model;
using OurHistory.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OurHistory.UserControls
{
    public sealed partial class PreguntaControl : UserControl
    {
        public event EventHandler<MyEventArgs> Incorrecto;
        public event EventHandler<MyEventArgs> Correcto;

        private string myPregunta;

        public string MyPregunta
        {
            get
            {
                return myPregunta;
            }
            set
            {
                myPregunta = value;
                UserPregunta.Text = MyPregunta;
            }
        }
        private List<Respuesta> myListaRespuestas;

        public List<Respuesta> MyListaRespuestas
        {
            get
            {
                return myListaRespuestas;
            }
            set
            {
                myListaRespuestas = value;
                lstOpciones.ItemsSource = MyListaRespuestas;
            }
        }

        private string detalleR;

        public string DetalleR
        {
            get
            {
                return detalleR;
            }
            set
            {
                detalleR = value;
                detalleRespC.Text = DetalleR;
            }
        }
        private string respuestaCorrecta;

        public string RespuestaCorrecta
        {
            get
            {
                return respuestaCorrecta;
            }
            set
            {
                respuestaCorrecta = value;
                txtRespuestaCorrecta.Text = RespuestaCorrecta;
            }
        }

        private int suma;

        public int Suma
        {
            get { return suma; }
            set { 
                suma = value; 
            }
        }

        private string respuestaIncorrecta;

        public string RespuestaIncorrecta
        {
            get { return respuestaIncorrecta; }
            set { 
                respuestaIncorrecta = value;
                txtRespuestaInCorrecta.Text = RespuestaIncorrecta;
               
            }
        }

        public PreguntaControl()
        {
            this.InitializeComponent();
        }

        private void lstOpciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //implementar suma de puntaje supongo
            //pero por el momento solo hare q al dar touch a alguna opcion se cierre
            lstOpciones.Visibility = Visibility.Collapsed;
            int posicion = lstOpciones.SelectedIndex;
            /*
            var send = (ListBox)sender;
            
            ItemCollection itemC=send.Items;
            Respuesta resp=(Respuesta)itemC[posicion];
            string loQRespondi = resp.Opcion;
            */
            string loQRespondi = MyListaRespuestas[posicion].Opcion;
            if (loQRespondi.Equals(RespuestaCorrecta))
            {
                //codigo para comparar si es igual a la respuesta
                Suma = Suma + 1;
                if (Correcto != null)
                {
                    Correcto(this, new MyEventArgs("correcto"));
                    SoundsRespuesta.Source = new Uri("ms-appx:///Sounds/aplausos.mp3", UriKind.RelativeOrAbsolute);
                    SoundsRespuesta.Play();
                }
                detalleRespC.Visibility = Visibility.Visible;
                btnSiguiente.Visibility = Visibility.Visible;
            
                // mensaje("correcto");
            }
            else
            {
                if (Incorrecto != null)
                {
                    Incorrecto(this, new MyEventArgs("incorrecto"));
                    SoundsRespuesta.Source = new Uri("ms-appx:///Sounds/buu.mp3", UriKind.RelativeOrAbsolute);
                    SoundsRespuesta.Play();
                }
                this.Visibility = Visibility.Collapsed;
                //mensaje("incorrecto");
            }

            //this.Visibility = Visibility.Collapsed;
        }
        private void btnSiguiente_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        private async void mensaje(string men)
        {
            var msg = new MessageDialog("EL CONTENIDO" + men, "PREGUNTA DESGLOZADA");
            await msg.ShowAsync();

        }
    }
}
