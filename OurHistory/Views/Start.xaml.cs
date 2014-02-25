using Newtonsoft.Json;
using OurHistory.CarpetaJson;
using OurHistory.Helpers;
using OurHistory.Model;
using OurHistory.UserControls;
using OurHistory.Views.Puzzle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace OurHistory.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Start : Page
    {
        string nivelX;
        List<Pregunta> codigos = new List<Pregunta>(); //todas las preguntas
        List<Pregunta> codigosTemporales = new List<Pregunta>();//preguntas q se veran en el juego

        List<Color> listaColores = new List<Color>();//elipses color general!
        List<Thickness> listaPosiciones = new List<Thickness>();//elipses posiciones general!
        List<int> indicePosiciones = new List<int>();//indices de posiciones en las q no se pueden repetir 
        int numPregPorNivel = 1;

        Random ran = new Random();

        int numeroPreguntaDelCamino = 0;
        
        //listaRespuestas por pregunta
        List<Respuesta> listaRespuestas = new List<Respuesta>();
            
        ListaPreguntas ListaPreg = new ListaPreguntas();

        //control pregunta
        //PreguntaControl pregCtrl = new PreguntaControl();

        private string varIncorrecto;
        public string VarIncorrecto
        {
            get { return varIncorrecto; }
            set {
                varIncorrecto = value;
                txtIncorrecto.Text = VarIncorrecto;
            }
        }
        public Start()
        {
            this.InitializeComponent();
            Loaded += Start_Loaded;
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

        void Start_Loaded(object sender, RoutedEventArgs e)
        {
            //Reading file Json
            //ReadingJson();
            //codigos = ListaXNivel(ListaPreg.Preguntas, nivelX);
            cargarPreguntasDeNivel();

            LaPaz.Tapped += LaPaz_Tapped;
            BotonAtras.Click += BotonAtras_Click;
            //botob.Click += botob_Click;

            //seleccion de prguntas d json

            cargaRandomPreguntaPorNivel(numPregPorNivel);
            ordenar();

            //cargaRandomPreguntaPorNivel(2);//digamos q tendra 2 preguntas
            cargaDatosElipses();

            //dibujar primera pregunta
            var el = new Ellipse() { Fill = new SolidColorBrush(Colors.Red) };
            el.SetValue(Grid.HeightProperty, 23);
            el.SetValue(Grid.WidthProperty, 26);
            el.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            el.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);
            el.SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.Black));

            //aca probando q se grafique pero luego se adicionara 1 por 1 ya q los datos estan guardados aqui
            el.SetValue(Grid.MarginProperty, listaPosiciones[indicePosiciones[numeroPreguntaDelCamino]]);
            
            el.Tapped += el_Tapped;
            grid0.Children.Add(el);
            //
            //Pd0.Visibility = Visibility.Collapsed;
            Creditos.Click += Creditos_Click;
        }

        void Creditos_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Creditos));
        }


        private void cargarPreguntasDeNivel()
        {
            if (nivelX.Equals("1"))
            {
                Pregunta preg = new Pregunta();
                preg.Codigo = 101;
                preg.Descripcion = "La ciudad de Pando fue creada el:";
                preg.DetalleRespuesta = "PANDO Creado el 24 de Septiembre de 1938 en el gobierno de Tcnl. German Buch. Fecha cívica: 24 de Septiembre (1938) Héroe departamental: José Manuel Pando";
                preg.RespuestaC = "24 de septiembre de 1938";
                preg.RespuestasO = new List<string> { "24 de septiembre de 1950", "24 de septiembre de 1938", "9 de febrero de 1906" };
                preg.Puntaje = 10;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 102;
                preg.Descripcion = "Cobija fue fundada el 9 de febrero de 1906 por el Coronel Enrique Cornejo:";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "La capital del departamento de Pando es Cobija fundada el 9 de febrero de 1906 por el Coronel Enrique Cornejo. Fue bautizada con el nombre de Cobija en recuerdo al Puerto de Cobija de nuestro perdido Litoral.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 201;
                preg.Descripcion = "La ciudad de La Paz fue fundada el:";
                preg.RespuestasO = new List<string> {  "16 de julio de 1809 ",
				                                        "20 de octubre de 1548",
				                                        "9 de marzo de 1809"};
                preg.RespuestaC = "20 de octubre de 1548";
                preg.DetalleRespuesta = "LA PAZ Creado el 23 de Enero de 1826 en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial) Fecha cívica: 16 de Julio de (1809) Héroe departamental: Pedro Domingo Murillo La Capital del departamento  es La Paz fundada el 20 de octubre de 1548 por el capitán Alonzo de Mendoza originalmente en lo que hoy es el pueblo de Laja, para trasladarse a los tres días al valle de Chuquiago. Nombre español: Nuestra Señora de La Paz Nombre indígena: Chuquiago";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 301;
                preg.Descripcion = "La capital del departamento es Oruro fundada el 1º de noviembre de 1606 por:";
                preg.RespuestasO = new List<string> { "Sebastián Pagador",
				                                        "Mcal. Antonio José de Sucre",
				                                        "Lic. Manuel Castro de Padilla" };
                preg.RespuestaC = "Lic. Manuel Castro de Padilla";
                preg.DetalleRespuesta = "ORURO Creado el 5 de Septiembre de 1826 en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial) Fecha cívica: 10 de Febrero (1781) Héroe departamental: Sebastián Pagador La capital del departamento es Oruro fundada el 1º de noviembre de 1606 por el Lic. Manuel Castro de Padilla. Nombre español: Villa de san Felipe de Austria Nombre indígena: Uru Uru";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 401;
                preg.Descripcion = "Fecha de creación de Potosí:";
                preg.RespuestasO = new List<string> { "23 de enero de 1826 ",
		                                                "23 de febrero de 1825 ",
		                                                "10 noviembre de 1810 " };
                preg.RespuestaC = "23 de enero de 1826 ";
                preg.DetalleRespuesta = "POTOSI Creado el  23 de Enero de 1826  en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial) Fecha cívica: 10 de Noviembre (1810) Héroe departamental: Alonzo de Ibáñez La capital del departamento es Potosí fundada en 1545 por Juan de Villarroel, aunque en realidad este recibió el título de fundado por Célula Real, ya que la ciudad nació espontáneamente por la riqueza del cerro rico. El Virrey Francisco de Toledo realizo la verdadera fundación y planeamiento en 1572. Nombre español: Villa Imperial de Potosí Nombre indígena: Potosí";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 501;
                preg.Descripcion = "Heroe departamental  de Tarija:";
                preg.RespuestasO = new List<string> { "Moto Méndez",
		                                                "Luis de Fuentes",
		                                                "Jaime Zudañez"};
                preg.RespuestaC = "Moto Méndez";
                preg.DetalleRespuesta = "TARIJA Creado el 24 de Septiembre de 1831 en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial) Fecha cívica: 15 de Abril (1817) Héroe departamental: José Eustaquio (Moto Méndez) La capital del departamento es Tarija fundada el 4 de Julio por Luis de Fuentes. Nombre español: Villa y frontera de Tarija";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 601;
                preg.Descripcion = "Chuquisaca fue creado el 23 de Enero de 1826   en el gobierno del:";
                preg.RespuestasO = new List<string> { "Gral. José Ballivián y Segurola",
				                                        "Mariscal Antonio José de Sucre ",
				                                        "Jaime Zudañez " };
                preg.RespuestaC = "Gral. José Ballivián y Segurola";
                preg.DetalleRespuesta = "CHUQUISACA Creado el 23 de Enero de 1826   en el gobierno del Gral. José Ballivián y Segurola Fecha cívica: 25 de Mayo (1809) Héroe departamental: Jaime Zudañez La capital del departamento  es Sucre hay tres versiones de la fundación 1.	El 29 de Septiembre de 1538 por Gonzalo Pizarro 2.	El 16 de Abril de 1540 por Pedro Anzurez 3.	No hubo acto oficial de fundación Nombre español: La Plata Nombre indígena: Choquechaca La ciudad además ha tenido además la denominación de Charcas, ciudad Blanca y tras la fundación de la Republica, Sucre en honor al Mariscal Antonio José de Sucre";
                preg.Puntaje = 100;
                codigos.Add(preg);


                preg = new Pregunta();
                preg.Codigo = 701;
                preg.Descripcion = "Heroe departamental  de Santa Cruz:";
                preg.RespuestasO = new List<string> { "Moto Méndez ",
				                                        "Ignacio Warnes ",
				                                        "Jaime Zudañez "};
                preg.RespuestaC = "Ignacio Warnes ";
                preg.DetalleRespuesta = "SANTA CRUZ Creado el 23 de Enero de 1826 en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial). Fecha cívica: 24 de Septiembre (1810) Héroe departamental: Ignacio Warnes La capital del departamento es Santa Cruz de la Sierra fundada el 26 de Febrero de 1560 por Ñuflo de Chávez. En 1604 Francisco de Alfaro traslado la ciudad de su antiguo lugar a otro llamado San Lorenzo de la Barraca, donde quedo definitivamente. Nombre español: Santa Cruz de la Sierra";
                preg.Puntaje = 100;
                codigos.Add(preg);


                preg = new Pregunta();
                preg.Codigo = 801;
                preg.Descripcion = "Capital de Beni: ";
                preg.RespuestasO = new List<string> { "Ciudad Blanca ",
				                                        "Cobija ",
				                                        "Trinidad" };
                preg.RespuestaC = "Trinidad";
                preg.DetalleRespuesta = "BENI Creado el 18 de Noviembre de 1842  en el gobierno de Gral. José Ballivián y Segurola Fecha cívica: 18 de Noviembre (1842) Héroe departamental: José Ballivián y Segurola La capital del departamento es Trinidad fundada 1686 por el Padre Jesuita Cipriano Barace. Nombre español: Misión de la Santísima Trinidad";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 901;
                preg.Descripcion = "Capital del departamento Cochabamba fundada el 4 de Agosto de 1571 :";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Falso";
                preg.DetalleRespuesta = "COCHABAMBA Creado el 23 de Enero de 1826 en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial) Fecha cívica: 14 de Septiembre (1810) Héroe departamental: Esteban Arze La capital del departamento es Cochabamba fundada el 2 de Agosto de 1571 por el capitán español Jerónimo de Osorio Nombre español: Villa de Oropesa Nombre indígena: Khocha Pampa";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 602;
                preg.Descripcion = "Cual fue la primera revolución realizada en América";
                preg.RespuestasO = new List<string> { "La revolución de La Paz",
				                                        "La revolución de Chuquisaca",
				                                        "La revolución de Quito" };
                preg.RespuestaC = "La revolución de Chuquisaca";
                preg.DetalleRespuesta = "Entre 1809 y 1811 se producen en América Hispana una serie de levantamientos que desembocan en la independencia. Los dos primeros en toda América española fueron la revolución de Chuquisaca, y el levantamiento de La Paz, en julio del mismo año.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 605;
                preg.Descripcion = "José Manuel de Goyeneche llega a la Charcas para conseguir el apoyo de  los virreinatos de Buenos Aires y Lima:";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                //gral
                preg = new Pregunta();
                preg.Codigo = 1605;
                preg.Descripcion = "Pregunta Gral";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1305;
                preg.Descripcion = "Pregunta Gral";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1785;
                preg.Descripcion = "Pregunta Gral";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                preg.Puntaje = 100;
                codigos.Add(preg);
            }
            else
            {
                if (nivelX.Equals("2"))
                {
                    Pregunta preg = new Pregunta();
                    preg.Codigo = 603;
                    preg.Descripcion = "Cuál fue el motivo para que España cree la junta de Sevilla";
                    preg.RespuestasO = new List<string> { "La revolución de Chuquisaca",
				                                        "La renuncia de Carlos IV",
				                                        "La invasión de Napoleón" };
                    preg.RespuestaC = "La invasión de Napoleón";
                    preg.DetalleRespuesta = "En España se había producido la invasión de Napoleón, la renuncia de Carlos IV en favor a su hijo Fernando VII y, más tarde, en Bayona (Francia), en la entrevista que el monarca español sostuvo con Napoleón y la renuncia al trono en favor del francés. Es entonces que los españoles iniciaron la resistencia, formando una junta de gobierno en Sevilla";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 167;
                    preg.Descripcion = "Despues de la firma el Acta de Independencia, el congreso aprobo una ley de reconocimiento para:";
                    preg.RespuestasO = new List<string> { "Jose  Mendizabal",
				                                            "Pedro Blanco y Sucre",
				                                            "Bolivar y Sucre" };
                    preg.RespuestaC = "Bolivar y Sucre";
                    preg.DetalleRespuesta = "El dia 10 de agosto de 1825, despues de firmada el Acta de Independencia, el congreso aprobo una ley de reconocimiento a los generales Bolivar y Sucre.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 256;
                    preg.Descripcion = "En homenaje al libertador el nuevo estado llevaria el nombre de :";
                    preg.RespuestasO = new List<string> { "El Alto Peru",
                                                            "Republica de Bolivia",
				                                            "Republica de Bolivar" };
                    preg.RespuestaC = "Republica de Bolivar";
                    preg.DetalleRespuesta = "En homenaje al libertador el nuevo estado llevaria el nombre de Republica de Bolivar y su capital recibiria de nombre de Sucre";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 378;
                    preg.Descripcion = "¿Cuál de estos enunciados es una afirmación verdadera?";
                    preg.RespuestasO = new List<string> { "El poder supremo corresponderia a Sucre, en tanto permaneciera en el pais, con el nombre de Protector",
				                                            "El poder supremo corresponderia a Bolivar, en tanto permaneciera en el pais",
				                                            "El poder supremo corresponderia a Bolivar, en tanto permaneciera en el pais, con el nombre de Protector"};
                    preg.RespuestaC = "El poder supremo corresponderia a Bolivar, en tanto permaneciera en el pais, con el nombre de Protector";
                    preg.DetalleRespuesta = "Correspondiente a la ley de reconocimiento de los libertadores de Bolivia.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 498;
                    preg.Descripcion = "___________________ la asamblea dicto una ley por la cual se adoptaba el regimen unitario con la division de tres poderes: ejecutivo, legislativo y judicial";
                    preg.RespuestasO = new List<string> { "El 13 de enero",
			                                            	"El 13 de agosto",
			                                            	"El 6 de agosto"};
                    preg.RespuestaC = "El 13 de agosto";
                    preg.DetalleRespuesta = "El 13 de agosto la asamblea dicto una ley por la cual se adoptaba el regimen unitario con la division de tres poderes.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 567;
                    preg.Descripcion = "_________________ Bolivar habia cruzado el Desaguadero, las poblaciones en masa salian a recibirlo";
                    preg.RespuestasO = new List<string> { "El 30 de julio",
				                                            "En agosto de 1825",
				                                            "Sucre y" };
                    preg.RespuestaC = "En agosto de 1825";
                    preg.DetalleRespuesta = "En agosto de 1825 Bolivar habia cruzado el Desaguadero, las poblaciones en masa salian a recibirlo. El libertador se dirigio a La Paz y de alli a Potosí.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 658;
                    preg.Descripcion = "¿Que promesa cumplio Bolivar al dirigirse a Potosí?";
                    preg.RespuestasO = new List<string> { "Llegar a la casa de la moneda",
				                                            "Formar una asamblea",
				                                            "Subir al cerro para enarbolar en el la bandera de la libertad" };
                    preg.RespuestaC = "Subir al cerro para enarbolar en el la bandera de la libertad";
                    preg.DetalleRespuesta = "El libertador se dirigio a La Paz y de alli a Potosí donde cumplio su promesa de subir al cerro para enarbolar en el la bandera de la libertad. Asi recordaba la promesa hecha en el monte Aventino de Roma de libertar America";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 765;
                    preg.Descripcion = "Entre los actos administrativos de Bolivar, se creo:";
                    preg.RespuestasO = new List<string> { "La contaduría para el manejo de los fondos publicos",
				                                            "La primera bandera",
				                                            "La moneda y los símbolos nacionales"};
                    preg.RespuestaC = "La contaduría para el manejo de los fondos publicos";
                    preg.DetalleRespuesta = "Entre los actos administrativos creo la contaduría para el manejo de los fondos publicos y encargo a Simon Rodríguez confeccionar un proyecto educativo.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 819;
                    preg.Descripcion = "Bolivar determino que fuera considerado como principal puerto de la Republica a:";
                    preg.RespuestasO = new List<string> { "Calama",
			                                            	"Puerto de Mejillones",
				                                            "Puerto La Mar" };
                    preg.RespuestaC = "Puerto La Mar";
                    preg.DetalleRespuesta = "Bolivar determino que el puerto de Cobija fuera denominado puerto La Mar como el principal  puerto de la Republica.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 987;
                    preg.Descripcion = "El 13 de agosto la asamblea dicto una ley, creando:";
                    preg.RespuestasO = new List<string> { "La moneda y los símbolos patrios",
				                                            "El departamento de Potosí",
				                                            "La capital de la republica" };
                    preg.RespuestaC = "La moneda y los símbolos patrios";
                    preg.DetalleRespuesta = "El 13 de agosto mediante ley de la asamblea se creo los símbolos patrios.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 965;
                    preg.Descripcion = "____________________  tenia una banda horizontal roja con cinco estrellas rodeadas por ramos de laurel";
                    preg.RespuestasO = new List<string> { "La tercera bandera",
				                                            "El escudo",
				                                            "La primera bandera" };
                    preg.RespuestaC = "La primera bandera";
                    preg.DetalleRespuesta = "La primera bandera tenia una banda horizontal roja con cinco estrellas rodeadas por ramos de laurel.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 471;
                    preg.Descripcion = "¿Cuál de estos enunciados es una afirmación verdadera?";
                    preg.RespuestasO = new List<string> { "Tres figuras femeninas sostenian una cinta con la leyenda: REPUBLICA BOLIVAR",
				                                            "Dos figuras femeninas sostenian una cinta con la leyenda: REPUBLICA BOLIVAR",
				                                            "Dos figuras femeninas sostenian una cinta con la leyenda: REPUBLICA DE BOLIVIA" };
                    preg.RespuestaC = "Dos figuras femeninas sostenian una cinta con la leyenda: REPUBLICA BOLIVAR";
                    preg.DetalleRespuesta = "El escudo tenia en su interior tres campos horizontales, del cual dos figuras femeninas sostenian una cinta con la leyenda REPUBLICA BOLIVAR";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 943;
                    preg.Descripcion = "¿Cuál de estos enunciados es una afirmación verdadera?";
                    preg.RespuestasO = new List<string> { "En noviembre de 1826, Sucre pedia una declaracion pormenorizada de las finanzas de cada departamento",
				                                            "En diciembre de 1826, Sucre pedia Estadisticas de los ingresos y gastos de las cinco provincias",
				                                            "En noviembre de 1825, Sucre pedia una declaracion pormenorizada de las finanzas de cada departamento"};
                    preg.RespuestaC = "En noviembre de 1826, Sucre pedia una declaracion pormenorizada de las finanzas de cada departamento";
                    preg.DetalleRespuesta = "Sucre pedia una declaracion pormenorizada de las finanzas de cada departamento, en noviembre de 1825";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    //gral
                    preg = new Pregunta();
                    preg.Codigo = 1605;
                    preg.Descripcion = "Pregunta Gral";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1305;
                    preg.Descripcion = "Pregunta Gral";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1785;
                    preg.Descripcion = "Pregunta Gral";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                }
                else
                {
                    Pregunta preg = new Pregunta();
                    preg.Codigo = 604;
                    preg.Descripcion = "En qué fecha se dio la revolución de Chuquisaca:";
                    preg.DetalleRespuesta = "La víspera del 25 de mayo el presidente de la audiencia, García León y Pizarro, mando arrestar a los oidores y también ordenó el arresto del fiscal Andreu y de los hermanos Manuel y Jaime Zudáñez, miembros del cabildo. Solo pudo encontrar a Jaime Zudáñez que fue llevado a la cárcel de la audiencia. La gente se amotino y recorría las calles gritando “viva el rey, que prendan a los señores oidores”. Un grupo se dirigió a la casa del arzobispo pidiendo la liberación de Zudáñez que, una vez puesto en libertad, fue llevado en hombros por el pueblo. Moxó decidió huir y Pizarro quedo vigilado en su residencia. Uno de los abogados, Manuel de Lemoine toco las campanas a rebato desde la torre de san Francisco, con el estaban Michel, Monteagudo y los Zudáñez. Los oidores pidieron la renuncia del presidente quien decidió entregar el mando político y militar. Según el historiador Estanislao Just era la primera vez que se daba el caso de una “ audiencia alzada”.";
                    preg.RespuestaC = "25 de mayo de 1809";
                    preg.RespuestasO = new List<string> { "25 de mayo de 1809",
				                                            "16 de julio de 1809",
				                                            "25 de mayo de 1810" };
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 123;
                    preg.Descripcion = "A quién se nombró presidente de la Junta Tuitiva:";
                    preg.RespuestasO = new List<string> { "Pedro Domingo Murillo",
			                                            	"Pedro  Antonio de Indaburo",
		                                            		"Tadeo Davila"};
                    preg.RespuestaC = "Pedro Domingo Murillo";
                    preg.DetalleRespuesta = "trabajandooooo";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 251;
                    preg.Descripcion = "_______________  de corte liberal fue elaborada en Cadiz en ausencia de Fernando VII";
                    preg.RespuestasO = new List<string> {"La constitución de 1900",
				                                            "La constitución de 1812",
				                                            "La asamblea" };
                    preg.RespuestaC = "La constitución de 1812";
                    preg.DetalleRespuesta = "La constitución de 1812 de corte liberal fue elaborada en Cadiz en ausencia de Fernando VII, que todavía se hallaba en Francia.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 321;
                    preg.Descripcion = "El año de 1814, Fernando VII derogo la constitución convirtiendose en un rey con poder absoluto";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "Al ser repuesto en el trono, el año de 1814, Fernando VII derogo la constitución convirtiendose en un rey con poder absoluto.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 407;
                    preg.Descripcion = "¿Que impidio para que salieran tropas españolas (20000 hombres) a America?";
                    preg.RespuestasO = new List<string> { "Una revolucion",
				                                            "El levantamiento de Riego",
				                                            "Una constitucion" };
                    preg.RespuestaC = "El levantamiento de Riego";
                    preg.DetalleRespuesta = "La situación en America era grave y se habia previsto el envio de 20000 hombres, pero el levantamiento de Riego en contra del regimen de Fernando lo impidio";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 512;
                    preg.Descripcion = "¿Cuál de estos enunciados es una afirmación verdadera?";
                    preg.RespuestasO = new List<string> { "Riego obligo al rey Fernado a reponer la constitución de 1812",
				                                            "Riego no obligo al rey a reponer la constitución",
				                                            "Riego obligo al rey Fernado a reponer la constitución de 1825" };
                    preg.RespuestaC = "Riego obligo al rey Fernado a reponer la constitución de 1812";
                    preg.DetalleRespuesta = "Como consecuencia del levantamiento, Riego obligo al rey Fernado a reponer la constitución de 1812.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 607;
                    preg.Descripcion = "El levantamiento de Riego en contra del rey. Riego obliga al rey Fernado a reponer la constitución de 1812. Estos acontecimientos provocaron que:";
                    preg.RespuestasO = new List<string> { "Los españoles fueran partidarios de la constitución",
				                                            "Los españoles quedaron divididos en  liberales y absolutistas",
				                                            "Los españoles quedaron divididos en  democratas y nacionalistas" };
                    preg.RespuestaC = "Los españoles quedaron divididos en  liberales y absolutistas";
                    preg.DetalleRespuesta = "A raiz de estos acontecimientos los españoles quedaron divididos en liberales, partidarios de la constitución y absolutistas, que eran los partidarios de Fernando VII.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 707;
                    preg.Descripcion = "En America el  ______________________  eran partidarios del rey";
                    preg.RespuestasO = new List<string> { "Virrey Pezuela y el general Olañeta",
				                                            "Virrey La Serna y el general Cantemac",
				                                            "General Valdes" };
                    preg.RespuestaC = "Virrey Pezuela y el general Olañeta";
                    preg.DetalleRespuesta = "La situacion en España se dio en America, asi el virrey Pezuela y el general Olañeta eran partidarios del rey.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 809;
                    preg.Descripcion = "El virrey La Serna y los generales Cantemac y Valdes eran liberales";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "El virrey La Serna y los generales Cantemac y Valdes eran liberales, no eran partidarios del rey.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 909;
                    preg.Descripcion = "El año 1823 la situación para los patriotas era critica, se organizo una campaña para atacar a las fuerzas realistas por los puertos intermedios. Se encomendo esta operación a:";
                    preg.RespuestasO = new List<string> { "Olañeta",
				                                            "Andres de Santa Cruz",
				                                            "Sucre" };
                    preg.RespuestaC = "Andres de Santa Cruz";
                    preg.DetalleRespuesta = "Andres de Santa Cruz decidio como lo hicieron muchos oficiales oriundos de estas tierras, elegir el servicio de la patria.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 910;
                    preg.Descripcion = "Después de tomar Arica y Tacna el general Santa Cruz se dirigio al ______________ , entrando en esta ciudad en agosto de 1823";
                    preg.RespuestasO = new List<string> { "lugar de Oruro",
				                                        "Desaguadero y Lima",
				                                        "Desaguadero y La Paz" };
                    preg.RespuestaC = "Desaguadero y La Paz";
                    preg.DetalleRespuesta = "Después de tomar Arica y Tacna el general Santa Cruz se dirigio al Desaguadero y La Paz , entrando en esta ciudad en agosto de 1823.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 145;
                    preg.Descripcion = "Lanza comandaba a los guerrilleros de Ayopaya";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "Santa Cruz recibio la ayuda de Lanza quien comandaba a los guerrilleros de Ayopaya.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 245;
                    preg.Descripcion = "Santa Cruz recibio el titulo de mariscal de zepita por:";
                    preg.RespuestasO = new List<string> { "El encuentro con Zepita",
				                                            "La batalla que tuvo lugar en Zepita",
				                                            "Los refuerzos esperados en Zepita" };
                    preg.RespuestaC = "La batalla que tuvo lugar en Zepita";
                    preg.DetalleRespuesta = "Al tener noticia de la invasión patriota las tropas españolas, al mando de Valdes, se dirigieron  al encuentro de Santa Cruz, la batalla tuvo lugar en Zepita.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 345;
                    preg.Descripcion = "¿Cuál de estos enunciados es una afirmación verdadera?";
                    preg.RespuestasO = new List<string> { "Antes de Junin ambas tropas, realistas y patriotas estaban equilibradas",
				                                            "Antes de Junin las tropas realistas eran mas numerosas que las tropas patriotas",
				                                            "Antes de Junin las tropas patriotas eran mas numerosas que las tropas realistas" };
                    preg.RespuestaC = "Antes de Junin ambas tropas, realistas y patriotas estaban equilibradas";
                    preg.DetalleRespuesta = "Antes de Junin ambas tropas, realistas y patriotas estaban equilibradas.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 445;
                    preg.Descripcion = "El 6 de agosto de 1824 tuvo lugar la batalla de:";
                    preg.RespuestasO = new List<string> { "Zepita",
				                                            "Ayacucho",
				                                            "Junin" };
                    preg.RespuestaC = "Junin";
                    preg.DetalleRespuesta = "La batalla de Junin tuvo lugar el 6 de agosto de 1824.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 545;
                    preg.Descripcion = "______________________ tuvo lugar la batalla de Ayacucho";
                    preg.RespuestasO = new List<string> { "El 9 de diciembre de 1824",
				                                            "En enero de 1825",
				                                            "El 9 de diciembre de 1825" };
                    preg.RespuestaC = "El 9 de diciembre de 1824";
                    preg.DetalleRespuesta = "El 9 de diciembre de 1824 tuvo lugar la batalla de Ayacucho.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    //gral
                    preg = new Pregunta();
                    preg.Codigo = 1605;
                    preg.Descripcion = "Pregunta Gral";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1305;
                    preg.Descripcion = "Pregunta Gral";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1785;
                    preg.Descripcion = "Pregunta Gral";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "José Manuel de Goyeneche llega a la Charcas, como delegado de la junta. Su misión consistía en ir a Buenos Aires y Lima pasando por Charcas, con el objeto de conseguir el apoyo de ambos virreinatos.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);
                }
            }


        }

        private void ordenar()
        {
            for (int i = 0; i < codigosTemporales.Count - 1; i++)
            {
                for (int j = i + 1; j < codigosTemporales.Count; j++)
                {
                    if (codigosTemporales[i].Codigo > codigosTemporales[j].Codigo)
                    {
                        Pregunta pp = new Pregunta();
                        pp = codigosTemporales[i];
                        codigosTemporales[i] = codigosTemporales[j];
                        codigosTemporales[j] = pp;
                    }
                }
            }
        }

        async void ReadingJson()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("CarpetaJson");
            var file = await folder.GetFileAsync("ListaPreguntas.json");
            var json = await Windows.Storage.FileIO.ReadTextAsync(file);
            var LstPregunta = JsonConvert.DeserializeObject<ListaPreguntas>(json);
            this.DataContext = LstPregunta;
            ListaPreg = LstPregunta;
            //lstPreguntas.ItemsSource = ListaXNivel(ListaPreg.Preguntas, "3");
            //lstPreguntas.ItemsSource = LstPregunta.Preguntas;
        }
        public List<Pregunta> ListaXNivel(List<Pregunta> ListAll, string nivelX)
        {

            //ReadingJson();
            //Metodo que devuelve lista con un parametro nivel q es eleccion del usuario
            List<Pregunta> listPreg = new List<Pregunta>();
            int i = 0;
            for (i = 0; i < ListaPreg.Preguntas.Count; i++)
            {
                if (ListaPreg.Preguntas.ElementAt(i).Nivel.Equals(nivelX))
                {
                    listPreg.Add(ListaPreg.Preguntas.ElementAt(i));
                }
            }
            return listPreg;
        }

       

        PopupHelper popHelper = new PopupHelper();

        private void el_Tapped(object sender, TappedRoutedEventArgs e)
        {

            txtIncorrecto.Text = "txt";
            PreguntaControl pregCtrl = new PreguntaControl();
            var eli = (Ellipse)sender;
            if (eli.ActualWidth == 22)
            {
                int ii = 0;
            }
            else
            {
                if (numeroPreguntaDelCamino == 999)
                { //mensaje();
                    int y = 0;
                }
                else
                {
                    //eli.Fill = new SolidColorBrush(Colors.Orange);
                    //eli.Width = 22;//valor q me ayudara a definir si le di touch o no!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    //pregCtrl = new PreguntaControl();
                    var location = (default(Point));
                    location.Y = ((grid0.ActualHeight / 2) - (pregCtrl.Height) / 2);
                    location.X = ((grid0.ActualWidth / 2) - (pregCtrl.Width) / 2);

                    //antes de agregar probando q se envie datos de pregunta
                    Pregunta pregTemporal = new Pregunta();
                    pregTemporal = codigosTemporales[numeroPreguntaDelCamino];


                    pregCtrl.MyPregunta = pregTemporal.Descripcion;//"¿Cual es la capital constitucional de Bolivia?";
                    listaRespuestas = new List<Respuesta>();

                    //antes creando obj respuesta
                    List<string> listaRespuestaAuxiliar = new List<string>();
                    listaRespuestaAuxiliar = pregTemporal.RespuestasO;
                    for (int i = 0; i < listaRespuestaAuxiliar.Count; i++)
                    {
                        Respuesta resp = new Respuesta();
                        resp.Opcion = listaRespuestaAuxiliar[i];
                        listaRespuestas.Add(resp);
                    }

                    //listaRespuestas.Add(new Respuesta("Oruro"));
                    //listaRespuestas.Add(new Respuesta("Sucre"));
                    //listaRespuestas.Add(new Respuesta("La Paz"));

                    pregCtrl.MyListaRespuestas = listaRespuestas;

                    pregCtrl.DetalleR = pregTemporal.DetalleRespuesta;
                    pregCtrl.RespuestaCorrecta = pregTemporal.RespuestaC;

                    popHelper.Show(pregCtrl, location);


                    popHelper.Incorrecto += (s, a) =>
                    {
                        if (!txtIncorrecto.Text.Equals(a.Result))
                        {
                            txtPorcentaje.Text = " " + (int.Parse(txtPorcentaje.Text) - 10) + " ";
                        }
                        txtIncorrecto.Text = a.Result;

                    };

                    popHelper.Correcto += (s, a) =>
                    {
                        eli.Fill = new SolidColorBrush(Colors.Orange);
                        eli.Width = 22;

                        if (!txtIncorrecto.Text.Equals(a.Result))
                        {
                            numeroPreguntaDelCamino++;//el pop helper se muestra y dispara este evento  asi q supongo q debo restar las veces q se muestra
                            txtPorcentaje.Text = " " + (int.Parse(txtPorcentaje.Text) + 10) + " ";
                            if (numeroPreguntaDelCamino < ((numPregPorNivel * 9)) + 3)
                            {
                                if (numeroPreguntaDelCamino == ((numPregPorNivel * 9)))
                                {
                                    gridGenerales.Visibility = Visibility.Visible;
                                }
                                dibujaSiguientePunto();
                            }
                            else
                            {
                                numeroPreguntaDelCamino = 999;
                                for (int i = 0; i < 999999; i++)
                                {

                                }
                                //ACA PUEDE SER LOGICA DE MENSAJE PARA PUNTAJE BUENO, MALO Y REGULAR
                                if (int.Parse(txtPorcentaje.Text) <= 0)
                                {
                                    //malo
                                    mensaje("Obtuviste " + txtPorcentaje.Text + " puntos.\nEse es un puntaje MALO en verdad debes repasar.");
                                }
                                else
                                {
                                    if (int.Parse(txtPorcentaje.Text) > 0 && int.Parse(txtPorcentaje.Text) <= (60 * numPregPorNivel))
                                    {
                                        //regular
                                        mensaje("Obtuviste " + txtPorcentaje.Text + " puntos.\nEse es un puntaje REGULAR estudia más.");
                                    }
                                    else
                                    {
                                        if (int.Parse(txtPorcentaje.Text) > (60 * numPregPorNivel) && int.Parse(txtPorcentaje.Text) < (100 * numPregPorNivel))
                                        {
                                            //bueno 
                                            mensaje("Obtuviste " + txtPorcentaje.Text + " puntos.\nEse es un puntaje BUENO podrias mejorarlo.");
                                        }
                                        else
                                        {
                                            if (int.Parse(txtPorcentaje.Text) >= (100 * numPregPorNivel) && int.Parse(txtPorcentaje.Text) < (120 * numPregPorNivel))
                                            {
                                                //muy bueno
                                                mensaje("Obtuviste " + txtPorcentaje.Text + " puntos.\nEse es un puntaje MUY BUENO estas a punto de llegar a la excelencia.");
                                            }
                                            else
                                            {
                                                if (int.Parse(txtPorcentaje.Text) == (120 * numPregPorNivel))
                                                {
                                                    //excelente
                                                    mensaje("Obtuviste " + txtPorcentaje.Text + " puntos.\nEse es un puntaje EXCELENTE sigue adelante.");
                                                }
                                            }
                                        }
                                    }
                                }

                                //mensaje();
                            }
                        }
                        txtIncorrecto.Text = a.Result;
                    };

                }
            }
        }
        void eli_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var re = 2;
        }
        private async void mensaje(string men)
        {
            var msg = new MessageDialog("" + men, "FELICIDADES JUEGO TERMINADO");
            await msg.ShowAsync();

        }
        private void dibujaSiguientePunto()
        {
            //dibujar pregunta
            var el = new Ellipse() { Fill = new SolidColorBrush(Colors.Red) };
            el.SetValue(Grid.HeightProperty, 23);
            el.SetValue(Grid.WidthProperty, 26);
            el.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            el.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);
            el.SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.Black));

            //aca probando q se grafique
            el.SetValue(Grid.MarginProperty, listaPosiciones[indicePosiciones[numeroPreguntaDelCamino]]);
            el.Tapped += el_Tapped;
            grid0.Children.Add(el);
        }
        private void cargaDatosElipses()
        {

            indicePosiciones = new List<int>();
            int[] tamanio = { 4, 9,15,22,27,32,42, 50, 55 };

            int limiteSup = tamanio[0];
            int limiteInf = 0;

            listaColores.Add(Colors.Orange);
            listaColores.Add(Colors.Violet);
            listaColores.Add(Colors.GreenYellow);
            listaColores.Add(Colors.Chocolate);
            listaColores.Add(Colors.LightBlue);
            listaColores.Add(Colors.DarkTurquoise);
            listaColores.Add(Colors.Yellow);
            listaColores.Add(Colors.Red);
            listaColores.Add(Colors.BlueViolet);

            //pando(0-4)
            listaPosiciones.Add(new Thickness(435, 112, 0, 0));
            listaPosiciones.Add(new Thickness(474, 91, 0, 0));
            listaPosiciones.Add(new Thickness(502, 121, 0, 0));
            listaPosiciones.Add(new Thickness(524, 77, 0, 0));
            listaPosiciones.Add(new Thickness(565, 49, 0, 0));

            //lp(5-9)
            listaPosiciones.Add(new Thickness(480, 184, 0, 0));
            listaPosiciones.Add(new Thickness(445, 222, 0, 0));
            listaPosiciones.Add(new Thickness(445, 272, 0, 0));
            listaPosiciones.Add(new Thickness(470, 362, 0, 0));
            listaPosiciones.Add(new Thickness(425, 392, 0, 0));
            
          
            
            //or(10-15)
            listaPosiciones.Add(new Thickness(482, 450, 0, 0));
            listaPosiciones.Add(new Thickness(512, 424, 0, 0));
            listaPosiciones.Add(new Thickness(532, 466, 0, 0));
            listaPosiciones.Add(new Thickness(500, 486, 0, 0));
            listaPosiciones.Add(new Thickness(452, 424, 0, 0));
            listaPosiciones.Add(new Thickness(460, 474, 0, 0));
           //pot(16-22)
            listaPosiciones.Add(new Thickness(480, 536, 0, 0));
            listaPosiciones.Add(new Thickness(540, 516, 0, 0));
            listaPosiciones.Add(new Thickness(530, 566, 0, 0));
            listaPosiciones.Add(new Thickness(500, 626, 0, 0));
            listaPosiciones.Add(new Thickness(575, 486, 0, 0));
            listaPosiciones.Add(new Thickness(575, 546, 0, 0));
            listaPosiciones.Add(new Thickness(580, 596, 0, 0)); 
            //tar(23-27)
            listaPosiciones.Add(new Thickness(620, 586, 0, 0));
            listaPosiciones.Add(new Thickness(665, 582, 0, 0));
            listaPosiciones.Add(new Thickness(718, 581, 0, 0));
            listaPosiciones.Add(new Thickness(644, 616, 0, 0));
            listaPosiciones.Add(new Thickness(692, 603, 0, 0));

            //sucre(28-32)
            listaPosiciones.Add(new Thickness(655, 496, 0, 0));
            listaPosiciones.Add(new Thickness(625, 536, 0, 0));
            listaPosiciones.Add(new Thickness(715, 555, 0, 0));
            listaPosiciones.Add(new Thickness(665, 550, 0, 0));
            listaPosiciones.Add(new Thickness(612, 468, 0, 0));

            //sc(33-42)
            listaPosiciones.Add(new Thickness(692, 344, 0, 0));
            listaPosiciones.Add(new Thickness(762, 264, 0, 0));
            listaPosiciones.Add(new Thickness(772, 354, 0, 0));
            listaPosiciones.Add(new Thickness(732, 474, 0, 0));
            listaPosiciones.Add(new Thickness(872, 399, 0, 0));
            listaPosiciones.Add(new Thickness(812, 454, 0, 0));
            listaPosiciones.Add(new Thickness(892, 454, 0, 0));
            listaPosiciones.Add(new Thickness(735, 414, 0, 0));
            listaPosiciones.Add(new Thickness(812, 404, 0, 0));
            listaPosiciones.Add(new Thickness(662, 424, 0, 0));

            //beni(43-50)
            listaPosiciones.Add(new Thickness(575, 100, 0, 0));
            listaPosiciones.Add(new Thickness(559, 160, 0, 0));
            listaPosiciones.Add(new Thickness(525, 220, 0, 0));
            listaPosiciones.Add(new Thickness(545, 290, 0, 0));
            listaPosiciones.Add(new Thickness(585, 230, 0, 0));
            listaPosiciones.Add(new Thickness(629, 190, 0, 0));
            listaPosiciones.Add(new Thickness(645, 270, 0, 0));
            listaPosiciones.Add(new Thickness(705, 200, 0, 0));
            //cbba(51-55)
            listaPosiciones.Add(new Thickness(543, 356, 0, 0));
            listaPosiciones.Add(new Thickness(605, 342, 0, 0));
            listaPosiciones.Add(new Thickness(603, 390, 0, 0));
            listaPosiciones.Add(new Thickness(554, 394, 0, 0));
            listaPosiciones.Add(new Thickness(614, 433, 0, 0));


            //para general
            listaPosiciones.Add(new Thickness(130, 310, 0, 0));
            listaPosiciones.Add(new Thickness(180, 360, 0, 0));
            listaPosiciones.Add(new Thickness(230, 410, 0, 0));



            int k = 0;
            for (int i = 0; i < 9; i++)// length seria el nro de departamentos
            {

                for (int j = 0; j < numPregPorNivel; j++) // length seria el num de preg
                {
                    var el = new Ellipse() { Fill = new SolidColorBrush(listaColores[k]) };
                    el.SetValue(Grid.HeightProperty, 23);
                    el.SetValue(Grid.WidthProperty, 26);
                    el.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    el.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);
                    el.SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.Black));

                    int numeroPosicionGenerada = numeroDentroDelRango(limiteInf, limiteSup);

                    while (estaEnIndicePosiciones(numeroPosicionGenerada))
                    {
                        numeroPosicionGenerada = numeroDentroDelRango(limiteInf, limiteSup);
                    }

                    indicePosiciones.Add(numeroPosicionGenerada);//agregando al indicePosiciones


                    //aca probando q se grafique pero luego se adicionara 1 por 1 ya q los datos estan guardados aqui
                    //el.SetValue(Grid.MarginProperty, listaPosiciones[indicePosiciones[indicePosiciones.Count-1]]);

                    //grid0.Children.Add(el);
                    //
                }

                k++;

                limiteInf = limiteSup + 1;
                if (i < (9 - 1))//length -1
                    limiteSup = tamanio[i + 1];
                //in=sup+1
            }
            indicePosiciones.Add(56);
            indicePosiciones.Add(57);
            indicePosiciones.Add(58);
        }
        private int numeroDentroDelRango(int a, int b)
        {
            return ran.Next(a, (b + 1));
        }
        private bool estaEnIndicePosiciones(int pos)
        {
            for (int i = 0; i < indicePosiciones.Count; i++)
            {
                if (indicePosiciones[i] == pos)
                    return true;
            }
            return false;
        }
        private async void mensaje()
        {
            
            
            var msg = new MessageDialog("TERMINASTE EL JUEGO", "FELICIDADES");
            await msg.ShowAsync();
            
        }

        void BotonAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        void LaPaz_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(Start));
           // Frame.Navigate(typeof(Detail));
        }

        public void cargaRandomPreguntaPorNivel(int n)
        {
            //donde n es el numero de preguntas por nivel

            //y en codigosTemporales estan las preguntas q se mostraran en el nivel
            int d1, d2, d3, d4, d5, d6, d7, d8, d9;
            d1 = 0;
            d2 = 0;
            d3 = 0;
            d4 = 0;
            d5 = 0;
            d6 = 0;
            d7 = 0;
            d8 = 0;
            d9 = 0;
            while (!(d1 == n && d2 == n && d3 == n && d4 == n && d5 == n && d6 == n && d7 == n && d8 == n && d9 == n))
            {
                Pregunta preg = new Pregunta();

                //if (d1 == 0 && d2 == 0 && d3 == 0 && d4 == 0 && d5 == 0 && d6 == 0 && d7 == 0 && d8 == 0 && d9 == 0)
                //{
                //    preg = codigos[0];
                //}
                //else
                //{
                    double randomico = ran.NextDouble();
                    int itemRan = Convert.ToInt32(randomico * (codigos.Count()) - 1);
                    if (itemRan >= 0 &&itemRan<codigos.Count)
                        preg = codigos[itemRan];
                //}
                if (preg.Codigo >= 100 && preg.Codigo < 200)
                {
                    if (d1 < n)
                    {
                        d1++;
                        codigosTemporales.Add(preg);
                    }
                }
                else
                {
                    if (preg.Codigo >= 200 && preg.Codigo < 300)
                    {
                        if (d2 < n)
                        {
                            d2++;
                            codigosTemporales.Add(preg);
                        }
                    }
                    else
                    {
                        if (preg.Codigo >= 300 && preg.Codigo < 400)
                        {
                            if (d3 < n)
                            {
                                d3++;
                                codigosTemporales.Add(preg);
                            }
                        }
                        else
                        {
                            if (preg.Codigo >= 400 && preg.Codigo < 500)
                            {
                                if (d4 < n)
                                {
                                    d4++;
                                    codigosTemporales.Add(preg);
                                }
                            }
                            else
                            {
                                if (preg.Codigo >= 500 && preg.Codigo < 600)
                                {
                                    if (d5 < n)
                                    {
                                        d5++;
                                        codigosTemporales.Add(preg);
                                    }
                                }
                                else
                                {
                                    if (preg.Codigo >= 600 && preg.Codigo < 700)
                                    {
                                        if (d6 < n)
                                        {
                                            d6++;
                                            codigosTemporales.Add(preg);
                                        }
                                    }
                                    else
                                    {
                                        if (preg.Codigo >= 700 && preg.Codigo < 800)
                                        {
                                            if (d7 < n)
                                            {
                                                d7++;
                                                codigosTemporales.Add(preg);
                                            }
                                        }
                                        else
                                        {
                                            if (preg.Codigo >= 800 && preg.Codigo < 900)
                                            {
                                                if (d8 < n)
                                                {
                                                    d8++;
                                                    codigosTemporales.Add(preg);
                                                }
                                            }
                                            else
                                            {
                                                if (preg.Codigo >= 900 && preg.Codigo < 1000)
                                                {
                                                    if (d9 < n)
                                                    {
                                                        d9++;
                                                        codigosTemporales.Add(preg);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            //luego del while adiciono preguntas generales
           
            
            int su = 0;
            while (su<3)
            {
                Pregunta preg2 = new Pregunta();
                double randomico2 = ran.NextDouble();
                int itemRan2 = Convert.ToInt32(randomico2 * (codigos.Count()) - 1);
                if (itemRan2 >= 0 && itemRan2 < codigos.Count)
                    preg2 = codigos[itemRan2];
                if (preg2.Codigo >= 1000 && preg2.Codigo < 2000 && su < 3)
                {
                    su++;
                    codigosTemporales.Add(preg2);
                }
               
            }
        }

        //debo graficar en el View lo de codigos Temporales
        //pero debe ser 1 a 1 y para eso dibujare los de codigo 100 luego de 200 etc





        public int siguientePregunta(int codPre, int n)
        {
            //donde n es el numero de preguntas por nivel
            //y codPre es el codigo de pregunta
            //            codigosTemporales;
            return 0;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectImagePage));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string mensaje = e.Parameter as string;
            if (!string.IsNullOrWhiteSpace(mensaje))
            {
                nivelX = mensaje;
            }
            //ReadingJson();

            //ListaPreg = ReadingJson2();


        }
    }
    public class MyEventArgs : EventArgs
    {
        public string Result { get; set; }
        public MyEventArgs(string result)
        {
            this.Result = result;
        }
    }
}
