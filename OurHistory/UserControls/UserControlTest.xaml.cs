using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
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
    public sealed partial class UserControlTest : UserControl
    {
        public UserControlTest()
        {
            this.InitializeComponent();
        }

        private void ButtonAtras(object sender, RoutedEventArgs e)
        {
            //Referenciar el Popup que es el control padre de este user control
            var pop = this.Parent as Popup;

            //Si elpadre es en efecto un Popup cerrarlo
            if (pop != null)
                pop.IsOpen = false;

            //Mostrar el SettingsPane
            SettingsPane.Show();
        }
    }
}
