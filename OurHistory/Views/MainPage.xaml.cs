using OurHistory.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OurHistory
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string nivel;
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            Window.Current.SizeChanged += Current_SizeChanged;
        }
        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e.Size.Width <= 1000)
            {
                //this.GridletraCancion.Margin = new Thickness(-500, 0, 0, 0);
                //this.TextBlockSongLyrics.Margin = new Thickness(10, 100, 0, 30);
                //this.StackPanelDatos.Visibility = Visibility.Collapsed;
                this.imgPausa.Visibility = Visibility.Visible;
                this.grid0.Visibility = Visibility.Collapsed;

            }
            else
            {
                this.grid0.Visibility = Visibility.Visible;
                this.imgPausa.Visibility = Visibility.Collapsed;
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            facil.Click += facil_Click;
            medio.Click += medio_Click;
            dificil.Click += dificil_Click;
            Creditos.Click += Creditos_Click;
            lineTime.Click += lineTime_Click;
        }

        void lineTime_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LineaDelTiempo));
        }

        void Creditos_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Creditos));
        }

        

        void dificil_Click(object sender, RoutedEventArgs e)
        {
            nivel = "3";
            this.Frame.Navigate(typeof(Start),nivel);
        }

        void medio_Click(object sender, RoutedEventArgs e)
        {
            nivel = "2";
            this.Frame.Navigate(typeof(Start),nivel);
        }

        void facil_Click(object sender, RoutedEventArgs e)
        {
            nivel = "1";
            this.Frame.Navigate(typeof(Start),nivel);
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
