using OurHistory.UserControls;
using OurHistory.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Background;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OurHistory
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        private const string TASK_NAME = "TileUpdate3";
        private const string TASK_ENTRY = "BackGroundTask3.TileUpdate3";

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
                this.AppBar.Visibility = Visibility.Collapsed;

            }
            else
            {
                this.grid0.Visibility = Visibility.Visible;
                this.imgPausa.Visibility = Visibility.Collapsed;
                this.AppBar.Visibility = Visibility.Visible;
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
            facil.Click += facil_Click;
            medio.Click += medio_Click;
            dificil.Click += dificil_Click;
            Creditos.Click += Creditos_Click;
            lineTime.Click += lineTime_Click;
        }

        void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Clear();
            var jkCommand = new SettingsCommand("Acerca de", "Acerca de",
                                                (handler) =>
                                                {
                                                    var settingsHelper = new SettingsWindowHelper();
                                                    settingsHelper.ShowFlyout(new UserControlTest());

                                                });

            args.Request.ApplicationCommands.Add(jkCommand);
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                result == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == TASK_NAME)
                        task.Value.Unregister(true);
                }

                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                builder.Name = TASK_NAME;
                builder.TaskEntryPoint = TASK_ENTRY;
                builder.SetTrigger(new TimeTrigger(15, false));
                var registration = builder.Register();
            }

        }
    }

    public class SettingsWindowHelper
    {
        /// <summary>Default Window Size</summary>
        public const double DEFAULT_WIDTH = 346;
        /// <summary>Allow to show any control floating over the main Window</summary>
        private Popup _popup;
        /// <summary>Delegate to execute when Popup is closed</summary>
        Action CloseAction;


        public SettingsWindowHelper()
        {
            _popup = new Popup();
            _popup.IsLightDismissEnabled = true;
            _popup.Closed += OnPopupClosed;
        }
        void OnPopupClosed(object sender, object e)
        {
            if (CloseAction != null)
                CloseAction.Invoke();
        }
        public void ShowFlyout(UserControl control, Action closeAction = null,
                        double width = DEFAULT_WIDTH)
        {
            //Asignar acciÃ³n a ejecutar al cerrar el Popup
            CloseAction = closeAction;

            //Asignar ancho y alto del Popup
            _popup.Width = width;
            _popup.Height = Window.Current.Bounds.Height;

            /* Asignar el ancho y alto del control
                * Aunque este puede ya traerlos definidos
                * en este caso es conveniente adecuarlo a 
                * la estructura que hemos planteado*/
            control.Width = width;
            control.Height = Window.Current.Bounds.Height;

            //Asignar el control del parÃ¡metro al Popup
            _popup.Child = control;

            //Establecer en que parte de la ventana se comienza a dibujar el Popup
            _popup.VerticalOffset = 0;
            _popup.HorizontalOffset = Window.Current.Bounds.Width - width;

            //Mostrar el Popup, sus contenidos
            _popup.IsOpen = true;
        }
    }
}
