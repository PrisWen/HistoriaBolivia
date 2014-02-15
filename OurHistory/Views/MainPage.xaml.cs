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
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            facil.Click += facil_Click;
            medio.Click += medio_Click;
            dificil.Click += dificil_Click;
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
