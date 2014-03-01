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

            Window.Current.SizeChanged += Current_SizeChanged;
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
                preg.DetalleRespuesta = "PANDO \n Creado el 24 de Septiembre de 1938 en el gobierno de Tcnl. German Buch. \n Fecha cívica: 24 de Septiembre (1938) \n Héroe departamental: José Manuel Pando";
                preg.RespuestaC = "24 de septiembre de 1938";
                preg.RespuestasO = new List<string> { "24 de septiembre de 1950", "24 de septiembre de 1938", "9 de febrero de 1906" };
                preg.Puntaje = 10;
                codigos.Add(preg);



                preg = new Pregunta();
                preg.Codigo = 201;
                preg.Descripcion = "La ciudad de La Paz fue fundada el:";
                preg.RespuestasO = new List<string> {  "16 de julio de 1809 ",
				                                        "20 de octubre de 1548",
				                                        "9 de marzo de 1809"};
                preg.RespuestaC = "20 de octubre de 1548";
                preg.DetalleRespuesta = "LA PAZ \n Creado el 23 de Enero de 1826 en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial) \n Fecha cívica: 16 de Julio de (1809) Héroe departamental: Pedro Domingo Murillo La Capital del departamento  es La Paz fundada el 20 de octubre de 1548 por el capitán Alonzo de Mendoza originalmente en lo que hoy es el pueblo de Laja, para trasladarse a los tres días al valle de Chuquiago. Nombre español: Nuestra Señora de La Paz Nombre indígena: Chuquiago";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 202;
                preg.Descripcion = "Cuando se conformó la junta Tuitiva:";
                preg.RespuestasO = new List<string> {  "24 de julio",
				                                        "16 de julio",
				                                        "17 de julio"};
                preg.RespuestaC = "24 de julio";
                preg.DetalleRespuesta = "El día 24 de julio se conformó la Junta Tuitiva, la que empezó a trabajar junto al cabildo que ejercía las funciones del gobierno.";
                preg.Puntaje = 100;
                codigos.Add(preg);


                preg = new Pregunta();
                preg.Codigo = 203;
                preg.Descripcion = "Pedro Domingo Murillo antes de morir dijo: “La tea que dejo encendida nadie la podrá apagar”";
                preg.RespuestasO = new List<string> {  "Falso",
				                                        "Verdadero"};
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "La tradición indica que Murillo antes de morir dijo:  “La tea que dejo encendida nadie la podrá apagar”.";
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
                preg.RespuestasO = new List<string> { "23 de enero de 1826",
		                                                "23 de febrero de 1825 ",
		                                                "10 noviembre de 1810 " };
                preg.RespuestaC = "23 de enero de 1826";
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
                preg.DetalleRespuesta = "CHUQUISACA Creado el 23 de Enero de 1826   en el gobierno del Gral. José Ballivián y Segurola Fecha cívica: 25 de Mayo (1809) Héroe departamental: Jaime Zudañez La capital del departamento  es Sucre hay tres versiones de la fundación\n1.El 29 de Septiembre de 1538 por Gonzalo Pizarro\n2.El 16 de Abril de 1540 por Pedro Anzurez\n3.No hubo acto oficial de fundación\nNombre español: La Plata Nombre indígena: Choquechaca La ciudad además ha tenido además la denominación de Charcas, ciudad Blanca y tras la fundación de la Republica, Sucre en honor al Mariscal Antonio José de Sucre";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 602;
                preg.Descripcion = "En qué fecha se dio la revolución de Chuquisaca:";
                preg.RespuestasO = new List<string> { "25 de mayo de 1809",
				                                        "16 de julio de 1809",
				                                        "25 de mayo de 1810" };
                preg.RespuestaC = "25 de mayo de 1809";
                preg.DetalleRespuesta = "La víspera del 25 de mayo el presidente de la audiencia, García León y Pizarro, mando arrestar a los oidores y también ordenó el arresto del fiscal Andreu y de los hermanos Manuel y Jaime Zudáñez, miembros del cabildo. Solo pudo encontrar a Jaime Zudáñez que fue llevado a la cárcel de la audiencia. La gente se amotino y recorría las calles gritando “viva el rey, que prendan a los señores oidores”. Un grupo se dirigió a la casa del arzobispo pidiendo la liberación de Zudáñez que, una vez puesto en libertad, fue llevado en hombros por el pueblo. Moxó decidió huir y Pizarro quedo vigilado en su residencia. Uno de los abogados, Manuel de Lemoine toco las campanas a rebato desde la torre de san Francisco, con el estaban Michel, Monteagudo y los Zudáñez. Los oidores pidieron la renuncia del presidente quien decidió entregar el mando político y militar. Según el historiador Estanislao Just era la primera vez que se daba el caso de una “audiencia alzada”.";
                preg.Puntaje = 100;
                codigos.Add(preg);


                preg = new Pregunta();
                preg.Codigo = 701;
                preg.Descripcion = "Heroe departamental  de Santa Cruz:";
                preg.RespuestasO = new List<string> { "Moto Méndez ",
				                                        "Ignacio Warnes",
				                                        "Jaime Zudañez "};
                preg.RespuestaC = "Ignacio Warnes";
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
                preg.DetalleRespuesta = "La capital del departamento es Trinidad fundada 1686 por el Padre Jesuita Cipriano Barace.";
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


                //gral nivel facil
                preg = new Pregunta();
                preg.Codigo = 1001;
                preg.Descripcion = "Cuando se conformó la junta Tuitiva:";
                preg.RespuestasO = new List<string> {  "24 de julio",
				                                        "16 de julio",
				                                        "17 de julio"};
                preg.RespuestaC = "24 de julio";
                preg.DetalleRespuesta = "El día 24 de julio se conformó la Junta Tuitiva, la que empezó a trabajar junto al cabildo que ejercía las funciones del gobierno.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1002;
                preg.Descripcion = "Cuál  fue la primera revolución realizada en América";
                preg.RespuestasO = new List<string> { "La revolución de La Paz",
				                                            "La revolución de Chuquisaca",
				                                            "La revolución de Quito"};
                preg.RespuestaC = "La revolución de Chuquisaca";
                preg.DetalleRespuesta = "Entre 1809 y 1811 se producen en América Hispana una serie de levantamientos que desembocan en la independencia. Los dos primeros en toda América española fueron la revolución de Chuquisaca, y el levantamiento de La Paz, en julio del mismo año.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1003;
                preg.Descripcion = "La Junta Suprema envía como emisarios a Londres a:";
                preg.RespuestasO = new List<string> { "Francisco de Miranda",
				                                        "Simón Bolívar y Andrés Bello",
				                                        "Antonio José de Sucre" };
                preg.RespuestaC = "Simón Bolívar y Andrés Bello";
                preg.DetalleRespuesta = "En Caracas se había constituido la Junta Suprema conservadora, la cual empezó a actuar en forma autónoma. Esta junta envió como emisarios a Londres a Simón Bolívar y Andrés Bello. Allí Bolívar conoció a Francisco de Miranda y ambos retornaron a Venezuela en 1810. En 1811 el Congreso creado en Caracas con la participación de Miranda, declaro la independencia de Venezuela, naciendo una precaria republica pues el general realista Monteverde, después de abatir a los rebeldes, restableció el antiguo régimen. Miranda cayó preso y Bolívar embarco para Nueva Granada (Colombia), formando allí de las tropas revolucionarias.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1004;
                preg.Descripcion = "El sueño de Bolívar era la libertad  de toda América";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "Después de los triunfos en Boyaca (1819) y Carabobo (1821) los ejércitos Libertadores estaban consolidados en el norte y, es entonces, que Bolívar emprende un gran sueño: la libertad de toda América. Vence la resistencia de Pasto y Popayan (sur de Colombia) y avanza rumbo a Quito, esta vez encomendando la tropas al general Antonio José de Sucre quien vence en Pichincha. Andrés Santa Cruz participo en esta batalla al lado de los patriotas.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1005;
                preg.Descripcion = " En América el  ______________________  eran partidarios del rey.";
                preg.RespuestasO = new List<string> { "Virrey Pezuelo y el general Olañeta",
				                                        "Virrey La Serna y el general Cantemac",
				                                        "General Valdés" };
                preg.RespuestaC = "Virrey Pezuelo y el general Olañeta";
                preg.DetalleRespuesta = "La situación en España se dio en América, así el virrey Pezuela y el general Olañeta eran partidarios del rey.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1006;
                preg.Descripcion = " El virrey La Serna y los generales Cantemac y Valdés eran liberales.";
                preg.RespuestasO = new List<string> { "Falso",
				                                        "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = ": El virrey La Serna y los generales Cantemac y Valdés eran liberales, no eran partidarios del rey.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1007;
                preg.Descripcion = "José Miguel Lanza comandaba a los guerrilleros de Ayopaya.";
                preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                preg.RespuestaC = "Verdadero";
                preg.DetalleRespuesta = "Andrés Santa Cruz recibió la ayuda de José Miguel  Lanza quien comandaba a los guerrilleros de Ayopaya.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1008;
                preg.Descripcion = " Andrés Santa Cruz recibió el título de Mariscal de Zepita por:";
                preg.RespuestasO = new List<string> { " El encuentro con Zepita", "La batalla que tuvo lugar en Zepita", " Los refuerzos esperados en Zepita" };
                preg.RespuestaC = "La batalla que tuvo lugar en Zepita";
                preg.DetalleRespuesta = "Al tener noticia de la invasión patriota las tropas españolas, al mando de Valdés, se dirigieron  al encuentro de Santa Cruz, la batalla cuyo resultado fue incierto tuvo lugar en Zepita.  A raíz de este hecho Andrés Santa Cruz recibió el título de Mariscal de Zepita.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1009;
                preg.Descripcion = "El 6 de agosto de 1824 tuvo lugar la batalla de:";
                preg.RespuestasO = new List<string> { "Zepita", "Ayacucho", "Junín" };
                preg.RespuestaC = "Junín";
                preg.DetalleRespuesta = ":  La batalla de Junín tuvo lugar el 6 de agosto de 1824, para entonces, Andrés Santa Cruz formaba parte del Estado Mayor de Bolívar, en tanto que Sucre, Gamarra y Burdett O’Connor comandaban el cuartel general.  En el ejército realista estaba como jefe el general José Canterac .";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1010;
                preg.Descripcion = "En la batalla de Junín el triunfo fue para las topas del:";
                preg.RespuestasO = new List<string> { "General Simón Bolívar", "General José Canterac" };
                preg.RespuestaC = "General Simón Bolívar";
                preg.DetalleRespuesta = "En Junín presencio la batalla el general Miller quien en sus “Memorias” relata el desarrollo de la misma que fue favorable a las tropas de Bolívar.";
                preg.Puntaje = 100;
                codigos.Add(preg);

                preg = new Pregunta();
                preg.Codigo = 1011;
                preg.Descripcion = "En homenaje al libertador el nuevo estado llevaría el nombre de:";
                preg.RespuestasO = new List<string> { "El Alto Peru",
                                                        "República de Bolivia",
				                                        "República de Bolivar"};
                preg.RespuestaC = "República de Bolivar";
                preg.DetalleRespuesta = "En homenaje al libertador el nuevo estado llevaría el nombre de “Republica de Bolívar y su capital recibiría de nombre de Sucre”.";
                preg.Puntaje = 100;
                codigos.Add(preg);
            }
            else
            {
                if (nivelX.Equals("2"))
                {
                    Pregunta preg = new Pregunta();


                    preg.Codigo = 101;
                    preg.Descripcion = "Cobija fue fundada el 9 de febrero de 1906 por el Coronel Enrique Cornejo:";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "La capital del departamento de Pando es Cobija fundada el 9 de febrero de 1906 por el Coronel Enrique Cornejo. Fue bautizada con el nombre de Cobija en recuerdo al Puerto de Cobija de nuestro perdido Litoral.";

                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 201;
                    preg.Descripcion = "A quién se nombró presidente de la Junta Tuitiva:";
                    preg.RespuestasO = new List<string> {  "Pedro Domingo Murillo",
				                                       "Pedro  Antonio de Indaburo",
				                                        "Tadeo Davila"};
                    preg.RespuestaC = "Pedro Domingo Murillo";
                    preg.DetalleRespuesta = "Murillo fue nombrado presidente de la junta Tuitiva la cual tenía doce vocales además de tres representantes indígenas: uno por los Yungas otro por Omasuyos y otro por Sorata.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 301;
                    preg.Descripcion = "En Oruro , el 6 de octubre de 1810 se levantan tropas a la cabeza de :";
                    preg.RespuestasO = new List<string> {  "Esteban Arce",
				                                        "Tomas Barrón",
				                                        "Melchor  Daza"};
                    preg.RespuestaC = "Tomas Barrón";
                    preg.DetalleRespuesta = "En Oruro,  el 6 de octubre de 1810 se levanta Tomas Barrón que reúne tropas a fin de unirse a Ribero y Arce, estas tropas participaron en la batalla de aroma.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 401;
                    preg.Descripcion = "La capital del departamento es Potosí fundada en 1545 por ___________, aunque recibió el título de fundado por Célula Real.";
                    preg.RespuestasO = new List<string> { "Juan de Villarroel",
		                                                "Mcal. Antonio José de Sucre",
		                                                "Alonzo de Ibáñez" };
                    preg.RespuestaC = "Juan de Villarroel";
                    preg.DetalleRespuesta = "POTOSI Creado el  23 de Enero de 1826  en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial)\nFecha cívica: 10 de Noviembre (1810)\nHéroe departamental: Alonzo de Ibáñez\nLa capital del departamento es Potosí fundada en 1545 por Juan de Villarroel, aunque en realidad este recibió el título de fundado por Célula Real, ya que la ciudad nació espontáneamente por la riqueza del cerro rico.\nEl Virrey Francisco de Toledo realizo la verdadera fundación y planeamiento en 1572.\nNombre español: Villa Imperial de Potosí.\nNombre indígena: Potosí";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 501;
                    preg.Descripcion = "Moto Méndez participo en la batalla de:";
                    preg.RespuestasO = new List<string> { "La Tablada",
		                                                "Junín",
		                                                "Ayacucho" };
                    preg.RespuestaC = "La Tablada";
                    preg.DetalleRespuesta = "Moto Méndez participo en la batalla de la Tablada en que los patriotas vencieron a los realistas comandadas por Ramírez en la que venía Andrés de Santa Cruz.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 601;
                    preg.Descripcion = "De acuerdo a lo establecido por Antonio José de Sucre se eligieron los representantes para la asamblea que tuvo lugar en:";
                    preg.RespuestasO = new List<string> { "La Paz",
				                                            "Oruro",
				                                            "Chuquisaca"};
                    preg.RespuestaC = "Chuquisaca";
                    preg.DetalleRespuesta = " El 3 de junio Antonio José de Sucre  cambio la sede de la reunión por la siguiente razón: “siendo algunos de ellos (los diputados) de alguna edad les es imposible venir a Oruro, donde el excesivo frio de la presente estación los destruiría”. Estableció que la asamblea se reuniría en Chuquisaca.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 602;
                    preg.Descripcion = "¿Cuál de estos enunciados es una afirmación verdadera?";
                    preg.RespuestasO = new List<string> { "La Asamblea tuvo lugar en Chuquisaca el 10 de julio de 1825",
				                                            "La Asamblea tuvo lugar en Oruro el 10 de julio de 1825",
				                                            "La Asamblea tuvo lugar en Chuquisaca el 10 de julio de 1824"};
                    preg.RespuestaC = "La Asamblea tuvo lugar en Chuquisaca el 10 de julio de 1825";
                    preg.DetalleRespuesta = "Por dificultades del arribo de los representantes, la primera reunión se hizo recién el 10 de julio de 1825 en Chuquisaca";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 603;
                    preg.Descripcion = "La Asamblea en Chuquisaca fue presidida por:";
                    preg.RespuestasO = new List<string> { "Antonio José de Sucre",
				                                            "José Mariano Serrano",
				                                            "Simón Bolívar"};
                    preg.RespuestaC = "José Mariano Serrano";
                    preg.DetalleRespuesta = "La Asamblea en Chuquisaca fue presidida por José Mariano Serrano que se había destacado en el congreso de Tucumán de 1816.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 701;
                    preg.Descripcion = "El 24 de septiembre de 1812 se produjo la batalla de Tucumán en la que participo Ignacio Warnes";
                    preg.RespuestasO = new List<string> { "Falso",
				                                            "Verdadero"};
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "El 24 de septiembre, según el historiador J.L. Roca  es una fecha simbólica adoptada por Ignacio Warnes poco antes de su muerte en la batalla de Pari el año 1816, que hace referencia al triunfo de Belgrano en la batalla de Tucumán que se produjo el 24 de septiembre de 1812en la que participo Warnes.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 702;
                    preg.Descripcion = "Ignacio Warnes llega a Santa Cruz de la Sierra nombrado gobernador por Manuel Belgrano";
                    preg.RespuestasO = new List<string> { "Falso",
				                                            "Verdadero"};
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "Nacido en Buenos Aires en 1771, Warnes llega a Santa Cruz de la Sierra nombrado gobernador por Manuel Belgrano, jefe  del segundo ejercito auxiliar. En vista del acoso realista conforma un ejército al cual integra a cuantos deseaban luchar por la cusa de la patria, incluyendo Mestizos y mulatos, decretando la libertad para todo esclavo que se enrolara en su tropa. Había luchado junto a Álvarez de Arenales en la Florida (1814), obteniendo la victoria y el control de Santa Cruz por las tropas patriotas.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 801;
                    preg.Descripcion = "Beni fue creado el __________________ en el gobierno de Gral. José Ballivián y Segurola ";
                    preg.RespuestasO = new List<string> { "18 de Noviembre de 1842",
				                                            "18 de Diciembre de 1842",
                                                            "14 de Noviembre de 1842  "};
                    preg.RespuestaC = "18 de Noviembre de 1842";
                    preg.DetalleRespuesta = "BENI \n Creado el 18 de Noviembre de 1842  en el gobierno de Gral. José Ballivián y Segurola \n Fecha cívica: 18 de Noviembre (1842)\n Héroe departamental: José Ballivián y Segurola \n La capital del departamento es Trinidad fundada 1686 por el Padre Jesuita Cipriano Barace.\n Nombre español: Misión de la Santísima Trinidad";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 901;
                    preg.Descripcion = "Cuando se forma la Junta de Guerra en Cochabamba";
                    preg.RespuestasO = new List<string> { "10 de septiembre de 1810",
				                                    "24 de septiembre de 1810",
				                                    "14 de septiembre de 1810"};
                    preg.RespuestaC = "14 de septiembre de 1810";
                    preg.DetalleRespuesta = "En Cochabamba, el 14 de septiembre de 1810 Francisco del  Ribero proclama la causa de la independencia; se forma una junta de guerra y se llama a cabildo abierto el cual declara su adhesión el gobierno de Buenos Aires.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 902;
                    preg.Descripcion = "La batalla de la coronilla se dio en :";
                    preg.RespuestasO = new List<string> { "La colina de San Jorge",
				                                        "La colina de San Pedro",
				                                        "La colina de San Sebastián"};
                    preg.RespuestaC = "La colina de San Sebastián";
                    preg.DetalleRespuesta = "Se intentó un pacto con Goyeneche, pero este pedía que le entregaran a los cabecillas. La ciudad se negó a hacerlo, especialmente las mujeres que se atrincheraron en la colina de San Sebastián, en el lugar conocido como “la Coronilla”. Goyeneche entro en  Cochabamba el 27 de mayo, quebrando totalmente la heroica resistencia. Las tropas realistas se dieron al saqueo y los principales jefes revolucionarios fueron ejecutados.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);



                    //gral medio


                    preg = new Pregunta();

                    preg.Codigo = 1001;
                    preg.Descripcion = "En qué fecha se  disolvió la Junta Tuitiva";
                    preg.RespuestasO = new List<string> {  "15 de septiembre",
				                                        "30 de septiembre",
				                                        "10 de agosto"};
                    preg.RespuestaC = "30 de septiembre";
                    preg.DetalleRespuesta = "El virrey Abascal ordeno a José Manuel Goyeneche, presidente de la audiencia de Cuzco, que organice las tropas para dominar la sublevación de La Paz. El 30 de septiembre se disolvió la Junta tuitiva y Murillo quedo al mando de la revolución.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);




                    preg = new Pregunta();
                    preg.Codigo = 1003;
                    preg.Descripcion = "En qué fecha se convoca a cabildo abierto ";
                    preg.RespuestasO = new List<string> { "10 de noviembre de 1810",
				                                        "5 de noviembre de 1810",
				                                        "25 de noviembre de 1810" };
                    preg.RespuestaC = "10 de noviembre de 1810";
                    preg.DetalleRespuesta = "El 10 de noviembre de 1810 fue convocado un cabildo abierto que depuso al gobernador Sanz y proclamo la adhesión a la junta de Buenos Aires.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1004;
                    preg.Descripcion = "Según  Bartolomé Mitre se les denomina “Republiquetas” a los guerrilleros instalados en un territorio determinado";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "A  los guerrilleros instalados en un territorio determinado, el historiador argentino Bartolomé Mitre llama “Republiquetas”.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1005;
                    preg.Descripcion = "El año 1823 la situación para los patriotas era crítica, se organizó una campaña con tropas que debían  atacar a las fuerzas realistas por los puertos intermedios. Se encomendó esta operación a:";
                    preg.RespuestasO = new List<string> { "Pedro de Antonio de Olañeta", "Andrés de Santa Cruz", "Antonio José de Sucre" };
                    preg.RespuestaC = "Andrés de Santa Cruz";
                    preg.DetalleRespuesta = "Andrés de Santa Cruz decidió como lo hicieron muchos oficiales oriundos de estas tierras, elegir el servicio de la patria.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1006;
                    preg.Descripcion = "Después de tomar Arica y Tacna el general Santa Cruz se dirigió al ______________ , entrando en esta ciudad en agosto de 1823.";
                    preg.RespuestasO = new List<string> { "lugar de Oruro", 
                                                            "Desaguadero y Lima", 
                                                            "Desaguadero y La Paz" };
                    preg.RespuestaC = "Desaguadero y La Paz";
                    preg.DetalleRespuesta = "Después de tomar Arica y Tacna el general Santa Cruz se dirigió al Desaguadero y La Paz , entrando en esta ciudad en agosto de 1823.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 1007;
                    preg.Descripcion = " La victoria obtenida ______________________ , favoreció a las tropas patriotas y se firmó la capitulación de Ayacucho.";
                    preg.RespuestasO = new List<string> { "El 9 de diciembre de 1824", "En enero de 1825", "El 9 de diciembre de 1825" };
                    preg.RespuestaC = "El 9 de diciembre de 1824";
                    preg.DetalleRespuesta = "La victoria obtenida  el 9 de diciembre de 1824, favoreció a las tropas patriotas y se firmó la capitulación de Ayacucho que fue generosa para los vencidos quienes pudieron retornar a España sin sufrir ninguna clase de represarías.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1008;
                    preg.Descripcion = "En la batalla de Ayacucho las tropas patrióticas fueron comandadas por Antonio José de Sucre junto a él, el general La Mar, Gamarra, O’Connor y Miller.";
                    preg.RespuestasO = new List<string> { "Falso",
			                                        	"Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "En la batalla de Ayacucho, comandaban las fuerzas patriotas, junto a Sucre, el general La Mar, Gamarra, O’Connor y Miller, pero no estuvo presente el general Santa Cruz que había sido destinado como prefecto en Huamanga (Perú).";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1009;
                    preg.Descripcion = "En la Asamblea Constituyente se firmó el Acta de la Independencia el 6 de agosto de 1825, en conmemoración de:";
                    preg.RespuestasO = new List<string> { "La reunión de la Asamblea Constituyente",
				                                            "La batalla de Junín",
				                                            "La batalla de Ayacucho" };
                    preg.RespuestaC = "La batalla de Junín";
                    preg.DetalleRespuesta = "El Acta de la Independencia se firmó el 6 de agosto de 1825, en conmemoración de la batalla de Junín. Firmaron el documento para la historia cuarenta y siete diputados.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1010;
                    preg.Descripcion = "___________________ la Asamblea dicto una ley sencilla, de apenas tres artículos que adelantaron los principios republicanos.";
                    preg.RespuestasO = new List<string> { "El 13 de enero",
				                                            "El 13 de agosto",
				                                            "El 6 de agosto" };
                    preg.RespuestaC = "El 13 de agosto";
                    preg.DetalleRespuesta = "El 13 de agosto la Asamblea dicto una ley sencilla, de apenas tres artículos que adelantaron los principios republicanos que se incluyeron en nuestras constituciones desde 1826.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);



                    preg = new Pregunta();
                    preg.Codigo = 1011;
                    preg.Descripcion = " Para saber la situación exacta de la nación, ¿Qué información pedía Antonio José de Sucre?";
                    preg.RespuestasO = new List<string> {"Informe de las leyes aprobadas",
				                                        "La constitución proyectada por Simón  Bolívar",
				                                        "Estadisticas de los ingresos y gastos de las cinco provincias" };
                    preg.RespuestaC = "Estadisticas de los ingresos y gastos de las cinco provincias";
                    preg.DetalleRespuesta = "La tarea de Sucre era reactivar y sostener una política de reformas revolucionarias.  A fines de 1826 el Gobierno de Sucre pedía una declaración pormenorizada de las finanzas de cada departamento. La información fue útil pero no del todo completa que se esperaba. Pero pese a las difíciles y adversas circunstancias se iniciaron las reformas.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 1012;
                    preg.Descripcion = "Para saber la situación exacta de la nación, ¿Qué información pedía Antonio José de Sucre?";
                    preg.RespuestasO = new List<string> {"Informe de las leyes aprobadas",
				                                        "La constitución proyectada por Simón  Bolívar",
				                                        "Estadisticas de los ingresos y gastos de las cinco provincias" };
                    preg.RespuestaC = "Estadisticas de los ingresos y gastos de las cinco provincias";
                    preg.DetalleRespuesta = "La tarea de Antonio José de Sucre era reactivar y sostener una política de reformas revolucionarias.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);
                }
                else
                {
                    Pregunta preg = new Pregunta();

                    preg.Codigo = 101;
                    preg.Descripcion = "Fue bautizada con el nombre de Cobija en recuerdo al Puerto de Cobija de nuestro perdido Litoral.";
                    preg.RespuestasO = new List<string> {"Falso",
			                                        	"Verdadero"};
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "Fue bautizada con el nombre de Cobija en recuerdo al Puerto de Cobija de nuestro perdido Litoral.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 201;
                    preg.Descripcion = "En qué fecha José Manuel de Goyeneche entro a La Paz  e inicio un juicio a los sublevados";
                    preg.RespuestasO = new List<string> { "30 de octubre de 1809",
				                                        "25 de octubre de 1809",
				                                        "15 de agosto de 1809" };
                    preg.RespuestaC = "25 de octubre de 1809";
                    preg.DetalleRespuesta = "El 25 de octubre de 1809 Goyeneche entro a La Paz e inicio un juicio a los sublevados. Las sentencias se dictaron enero y febrero del  1810. En total 12 rebeldes fueron condenados a la horca, a os demás se los condenó a prisión y a destierro.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 301;
                    preg.Descripcion = "  _________________ de 1826 creo el departamento de Oruro.";
                    preg.RespuestasO = new List<string> { "Un decreto de Agosto",
				                                        "Una ley de septiembre",
				                                        "La asamblea" };
                    preg.RespuestaC = "Una ley de septiembre";
                    preg.DetalleRespuesta = "Después de que se crearon por decreto supremo cinco departamentos, una ley de septiembre del mismo año creo el departamento de Oruro.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 401;
                    preg.Descripcion = "Fecha cívica de Potosí";
                    preg.RespuestasO = new List<string> { "23 de enero",
				                                        "23 de febrero",
				                                        "10 noviembre" };
                    preg.RespuestaC = "10 noviembre";
                    preg.DetalleRespuesta = "POTOSI \n Creado el  23 de Enero de 1826  en el gobierno del Mcal. Antonio José de Sucre (sobre la base de la división política colonial) \n Fecha cívica: 10 de Noviembre (1810)\n Héroe departamental: Alonzo de Ibáñez \n La capital del departamento es Potosí fundada en 1545 por Juan de Villarroel, aunque en realidad este recibió el título de fundado por Célula Real, ya que la ciudad nació espontáneamente por la riqueza del cerro rico. El Virrey Francisco de Toledo realizo la verdadera fundación y planeamiento en 1572.\n Nombre español: Villa Imperial de Potosí \n Nombre indígena: Potosí";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 501;
                    preg.Descripcion = "Moto Méndez armo un ejército auxiliar denominado";
                    preg.RespuestasO = new List<string> {"Cuarto ejército auxiliar argentino",
				                                            "Segundo ejército auxiliar argentino ",
				                                            "Primer ejército auxiliar argentino"};
                    preg.RespuestaC = "Cuarto ejército auxiliar argentino";
                    preg.DetalleRespuesta = "Conoció a Camargo y con sus montoneros ayudo a Guemes y luego se incorporó a las tropas de La Madrid quien desde Tucumán, en forma espontánea, armo un ejército auxiliar que nuestra historia la conoce como cuarto ejército auxiliar argentino.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 601;
                    preg.Descripcion = "En la Asamblea de Chuquisaca solo estaban dos veteranos de la lucha por la independencia: José Miguel Lanza y José Ballivián.";
                    preg.RespuestasO = new List<string> {"Falso",
			                                        	"Verdadero"};
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = " Entre los asistentes de la Asamblea de Chuquisaca solo estaban dos veteranos de la lucha por la independencia: José Miguel Lanza y José Ballivián, lo que muestra que en los 16 años de contienda muchas cosas habían cambiado, en su mayoría los grandes idealistas habían muerto.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 602;
                    preg.Descripcion = "¿En la Asamblea realizada en Chuquisaca cuál de estas tres opciones tuvo el mayor apoyo, para su aprobación en pleno?";
                    preg.RespuestasO = new List<string> {"¿Se unirian las provincias del Alto Peru a la Argentina o se declaraban separadas, por ser esto mas conveniente a sus intereses y felicidad?",
				                                        "¿El Alto Peru quedara unido con la Republica del Bajo Peru, o tambien se declarara separado?",
				                                        "¿Los departamentos del Alto Peru se erigiran en Estado soberano e independiente de todas las naciones, tanto del Viejo como del nuevo Mundo?"};
                    preg.RespuestaC = "¿Los departamentos del Alto Peru se erigiran en Estado soberano e independiente de todas las naciones, tanto del Viejo como del nuevo Mundo?";
                    preg.DetalleRespuesta = "Prevaleció el sentimiento mayoritario por la autonomía, la primera opción no tuvo ningún voto, la segunda solo dos y la tercera fue aprobada por la asamblea en pleno.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 603;
                    preg.Descripcion = "Se formó una comisión para redactar la “Declaración de Independencia” en la Asamblea Constituyente, entre los que estaban:";
                    preg.RespuestasO = new List<string> {"José Mariano Serrano",
                                                        "Casimiro Olañeta",
				                                        "Manuel María Urcullu y José María Mendizábal",
				                                        "Todos los anteriores"};
                    preg.RespuestaC = "Todos los anteriores";
                    preg.DetalleRespuesta = "La comisión encargada de redactar la declaración estaba formada por siete miembros entre los que estaban José Mariano  Serrano, Casimiro Olañeta, Manuel María Urcullu y José María Mendizábal.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 701;
                    preg.Descripcion = "Los  cruceños a través de Castelli pidieron a la junta de Buenos Aires  no estar sujetos  a la  intendencia de ____________ lo que fue aceptado.";
                    preg.RespuestasO = new List<string> { "Cochabamba",
			                                            	"La Paz",
		                                            		"Sucre"};
                    preg.RespuestaC = "Cochabamba";
                    preg.DetalleRespuesta = "Los   cruceños a través de Castelli pidieron a la junta de Buenos Aires pidieron no estar sujetos  a la  intendencia de Cochabamba lo que fue aceptado.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 702;
                    preg.Descripcion = "Los ejércitos de Ignacio Warnes y Xavier de Aguilera se enfrentan en Pari el:";
                    preg.RespuestasO = new List<string> { "21 de noviembre de 1816",
				                                            "24 de noviembre de 1816",
				                                            "24 de septiembre de 1816"};
                    preg.RespuestaC = "21 de noviembre de 1816";
                    preg.DetalleRespuesta = "Cuando Buenos Aires decide nombrar un nuevo gobernador en la persona de Santiago de la Carrera, Warnes se retira para continuar luchando por la independencia de la región de Chiquitos. Carrera muere en una revuelta y Warnes asume nuevamente el mando en la región que había quedado completamente aislada, pues desde 1816 Xavier de Aguilera  controlaba toda la zona, desde Mizque hasta Vallegrande, el paso hacia la zona occidental dond había concentrado sus tropas. Ambos ejércitos se enfrentaron en Pari el 21 de noviembre de 1816, Aguilera con 1.200 hombres y Warnes con 1.000, resultando vencedor el coronel Francisco Xavier de Aguilera después de una sangrienta batalla.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 801;
                    preg.Descripcion = "Quién  es el héroe departamental del Beni";
                    preg.RespuestasO = new List<string> {"José Ballivián",
				                                            "Segurola",
				                                            "Todos los anteriores"};
                    preg.RespuestaC = "Todos los anteriores";
                    preg.DetalleRespuesta = "BENI \n Creado el 18 de Noviembre de 1842  en el gobierno de Gral. José Ballivián y Segurola \n Fecha cívica: 18 de Noviembre (1842)\n Héroe departamental: José Ballivián y Segurola \n La capital del departamento es Trinidad fundada 1686 por el Padre Jesuita Cipriano Barace.\n Nombre español: Misión de la Santísima Trinidad.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 901;
                    preg.Descripcion = "El enfrentamiento entre Esteban Arze y  José Manuel de Goyeneche fue en:";
                    preg.RespuestasO = new List<string> { "Septiembre de 1810",
				                                    "Mayo de 1812",
				                                    "Mayo de 1810"};
                    preg.RespuestaC = "Mayo de 1812";
                    preg.DetalleRespuesta = "El encuentro entre las tropas de Esteban Arze y José Manuel de Goyeneche tuvo lugar en Pocona, en mayo de 1812. Esteban Arze fue vencido y José Manuel de Goyeneche avanzo a Cochabamba que preparaba la resistencia con muy escasos recursos. ";
                    preg.Puntaje = 100;
                    codigos.Add(preg);






                    //gral dificil


                    preg = new Pregunta();
                    preg.Codigo = 1001;
                    preg.Descripcion = "Manuel Ascencio Padilla tenía sus tropas en la región  _______________________ ";
                    preg.RespuestasO = new List<string> { "La Laguna, Tomina y el Villar",
				                                            "El Villar y Palca ",
				                                            "Palca, Inquisivi y La Laguna" };
                    preg.RespuestaC = "La Laguna, Tomina y el Villar";
                    preg.DetalleRespuesta = "Manuel Ascencio Padilla tenía sus tropas en la región de La laguna, Tomina y el Villar en el actual departamento de Chuquisaca.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1002;
                    preg.Descripcion = "Quién derrotó  a Manuel Asencio Padilla en septiembre de 1816";
                    preg.RespuestasO = new List<string> { "Manuel Belgrano",
				                                            "Francisco  Xavier de Aguilera",
				                                            "Mariano Moreno" };
                    preg.RespuestaC = "Francisco  Xavier de Aguilera";
                    preg.DetalleRespuesta = "En 1816 Padilla había dejado un destacamento en Tarabuco conformado mayormente por indígenas, los cuales vencieron al batallón realista de “Los Verdes” comando por Herrera, dando muerte a al mayoría de los combatientes y fusilando a sus jefes. En septiembre de ese año fue enviado el general  cruceño Francisco Xavier de Aguilera para desbaratar a  los guerrilleros de Padilla. El enfrentamiento tuvo lugar en La Laguna y el triunfo fue de Aguilera. Padilla, junto con su esposa y su capellán, lograron huir, pero Manuel Asencio Padilla fue alcanzado por Aguilera quien le dio muerte.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1003;
                    preg.Descripcion = "Cuál de sus cuatro hijos sobrevivió y se quedó junto a Juana Azurduy de Padilla";
                    preg.RespuestasO = new List<string> { "La menor llamada Juana",
				                                            "La menor llamada Luisa",
				                                            "La menor llamada Mariana" };
                    preg.RespuestaC = "La menor llamada Luisa";
                    preg.DetalleRespuesta = "Manuel  Ascencio estuvo acompañado por su esposa Juana Azurduy,  la cual dejando su papel de ama de casa dedicada a criar a sus cuatro hijos, abandono su hogar y se sumó a la lucha por la independencia. Montada en su caballo seguía paso a paso la contienda y combatiendo llevando a sus hijos que, por su tierna edad, no pudieron soportar los avatares de la guerra, muriendo antes de que se termine. Solo le quedo la menor, Luisa, quien acompaño a Juana en su larga vejez.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 1004;
                    preg.Descripcion = "Qué  fue  lo que envió Simón Bolívar en los pocos meses que estuvo en Bolivia";
                    preg.RespuestasO = new List<string> { "Constitución Vitalicia",
				                                        "Constitución Boliviana" };
                    preg.RespuestaC = "Constitución Vitalicia";
                    preg.DetalleRespuesta = "Después de breve estadía en Bolivia, que duro solo unos pocos meses,  envió la “Constitución Vitalicia”, y encargo a Sucre el gobierno de la nación retornando a Colombia (1827) a la que encontró sumida en unja guerra civil a la consecuencia de la cual Colombia y Venezuela se separaron (1829). En tanto Perú había abolido la “Constitución Boliviana” y Bolivia había hecho otro tanto con la “Constitución Vitalicia”. La audiencia de Quito, por su parte, se había constituido en la república independiente.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1005;
                    preg.Descripcion = " _______________  de corte liberal fue elaborada en Cádiz en ausencia de Fernando VII.";
                    preg.RespuestasO = new List<string> { "La constitución de 1900",
				                                        "La constitución de 1812",
                                                         "La asamblea"};
                    preg.RespuestaC = "La constitución de 1812";
                    preg.DetalleRespuesta = "La constitución de 1812 de corte liberal fue elaborada en Cádiz en ausencia de Fernando VII, que todavía se hallaba en Francia.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1006;
                    preg.Descripcion = "El año de 1814, Fernando VII derogó  la constitución convirtiéndose en un rey con poder absoluto.";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "Al ser repuesto en el trono, el año de 1814, Fernando VII derogó  la constitución convirtiéndose en un rey con poder absoluto.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1007;
                    preg.Descripcion = " ¿Que impidió para que salieran tropas españolas (20000 hombres) a América?";
                    preg.RespuestasO = new List<string> { "Una revolución",
				                                            "El levantamiento de Riego",
				                                            "Una constitución" };
                    preg.RespuestaC = "El levantamiento de Riego";
                    preg.DetalleRespuesta = "La situación en América era grave y se había previsto el envió de 20000 hombres, pero el levantamiento de Riego en contra del régimen despótico de Fernando impidió que estas tropas salieran de España (1808).";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1008;
                    preg.Descripcion = "El levantamiento de Riego en contra del rey. Riego obliga al rey Fernando a reponer la constitución de 1812. Estos acontecimientos provocaron que:";
                    preg.RespuestasO = new List<string> { " Los españoles fueran partidarios de la constitución.",
				                                            "Ninguno",
				                                            "Los españoles quedaron divididos en  liberales y absolutistas.",
                                                             " Los españoles quedaron divididos en  demócratas y nacionalistas."};
                    preg.RespuestaC = "Los españoles quedaron divididos en  liberales y absolutistas.";
                    preg.DetalleRespuesta = "A raíz de estos acontecimientos los españoles quedaron divididos en liberales, partidarios de la Constitución y absolutistas, que eran los partidarios de Fernando VII.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1007;
                    preg.Descripcion = " ¿Que impidió para que salieran tropas españolas (20000 hombres) a América?";
                    preg.RespuestasO = new List<string> { "Una revolución",
				                                            "El levantamiento de Riego",
				                                            "Una constitución" };
                    preg.RespuestaC = "El levantamiento de Riego";
                    preg.DetalleRespuesta = "La situación en América era grave y se había previsto el envió de 20000 hombres, pero el levantamiento de Riego en contra del régimen despótico de Fernando impidió que estas tropas salieran de España (1808).";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 1008;
                    preg.Descripcion = "Después de la firma el Acta de Independencia, el congreso aprobó una ley de reconocimiento para:";
                    preg.RespuestasO = new List<string> { "José  Mendizábal",
				                                            "Pedro Blanco y Antonio José de  Sucre",
				                                            "Simón Bolívar y Antonio José de Sucre",
                                                            "José Mariano Serrano y Simón Bolívar"};
                    preg.RespuestaC = "Simón Bolívar y Antonio José de Sucre";
                    preg.DetalleRespuesta = "El día 10 de agosto de 1825, después de firmar el Acta de Independencia, el congreso aprobó una ley de reconocimiento a los generales Simón Bolívar y Antonio José de Sucre.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1009;
                    preg.Descripcion = " El poder supremo de la Nación correspondió a Bolívar “por el tiempo que resida entre los límites de ella y donde quiera que exista fuera de estos, tendrá los honores de Protector y presidente de ella”. ";
                    preg.RespuestasO = new List<string> { "Falso", "Verdadero" };
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = " El poder supremo de la Nación correspondió a Bolívar “por el tiempo que resida entre los límites de ella y donde quiera que exista fuera de estos, tendrá los honores de Protector y presidente de ella”. ";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1010;
                    preg.Descripcion = "¿Qué promesa cumplió Bolívar al dirigirse a Potosí?";
                    preg.RespuestasO = new List<string> {"Llegar a la casa de la moneda",
				                                        "Formar una asamblea",
				                                        "Subir al cerro para enarbolar en él la bandera de la libertad" };
                    preg.RespuestaC = "Subir al cerro para enarbolar en él la bandera de la libertad";
                    preg.DetalleRespuesta = "El libertador se dirigió a La Paz y de allí a Potosí donde cumplió su promesa de subir al cerro para enarbolar en el la bandera de la libertad. Así recordaba la promesa hecha en el monte Aventino de Roma de libertar América.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1011;
                    preg.Descripcion = "Entre los actos administrativos de Simón Bolívar, se creó:";
                    preg.RespuestasO = new List<string> {"La contaduría para el manejo de los fondos públicos",
				                                        "La primera bandera",
				                                        "La moneda y los símbolos nacionales"};
                    preg.RespuestaC = "Verdadero";
                    preg.DetalleRespuesta = "Entre los actos administrativos de Bolívar está la creación de la contaduría para el manejo de los fondos públicos y encargo a Simón Rodríguez confeccionar un proyecto educativo.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1012;
                    preg.Descripcion = "Simón Bolívar determino que el puerto Cobija fuera denominado:";
                    preg.RespuestasO = new List<string> {"Calama",
				                                        "Puerto de Mejillones",
				                                        "Puerto La Mar"};
                    preg.RespuestaC = "Puerto La Mar";
                    preg.DetalleRespuesta = "Bolívar determino que el puerto de Cobija, el principal puerto de Bolivia, fuera denominado puerto La Mar en homenaje al general de este nombre que gobernó el Perú.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);


                    preg = new Pregunta();
                    preg.Codigo = 1013;
                    preg.Descripcion = "El 17 de agosto de 1825 la Asamblea dicto una ley, creando:";
                    preg.RespuestasO = new List<string> {"La moneda y los símbolos patrios",
				                                            "El departamento de Potosí",
				                                            "La capital de la república"};
                    preg.RespuestaC = "La moneda y los símbolos patrios";
                    preg.DetalleRespuesta = ": El 17 de agosto de 1825 mediante ley de la Asamblea se creó  los símbolos patrios.";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1014;
                    preg.Descripcion = "____________________  tenía una banda horizontal roja con cinco estrellas rodeadas por ramos de laurel.";
                    preg.RespuestasO = new List<string> {"La tercera bandera",
				                                        "El escudo",
				                                        "La primera bandera"};
                    preg.RespuestaC = "La primera bandera";
                    preg.DetalleRespuesta = "La primera bandera tenía una banda horizontal roja con cinco estrellas rodeadas por ramos de laurel. ";
                    preg.Puntaje = 100;
                    codigos.Add(preg);

                    preg = new Pregunta();
                    preg.Codigo = 1015;
                    preg.Descripcion = "En el escudo, las cinco estrellas aludían a los departamentos de:";
                    preg.RespuestasO = new List<string> {"La Paz, Chuquisaca, Potosí, Cochabamba y Santa Cruz",
				                                    "La Paz, Chuquisaca, Beni, Cochabamba y Santa Cruz",
				                                    "La Paz, Chuquisaca, Potosí, Cochabamba y Oruro"};
                    preg.RespuestaC = "La Paz, Chuquisaca, Potosí, Cochabamba y Santa Cruz";
                    preg.DetalleRespuesta = " El escudo tenía en su interior tres campos horizontales, el superior con cinco estrellas que aludían a los departamentos de La Paz, Chuquisaca, Potosí, Cochabamba y Santa Cruz.";
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
               // int ii = 0;
            }
            else
            {
                if (numeroPreguntaDelCamino == 999)
                { //mensaje();
                   // int y = 0;
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
            //var re = 2;
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
