using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Vizuelizacija.BFS;
using Vizuelizacija.Model;
using Point = System.Windows.Point;

namespace Vizuelizacija
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Sve sto koristim od promenljivih

        //matrica
        List<Point> matrica = new List<Point>();
        public Dictionary<Point, PowerEntity> dictTackaElement = new Dictionary<Point, PowerEntity>();

        //elementi
        public List<PowerEntity> listaElemenataIzXML = new List<PowerEntity>();
        public List<LineEntity> listaVodova = new List<LineEntity>();
        public List<LineEntity> vodDuplikat = new List<LineEntity>();

        //ucitavanje elemenata
        public double noviX, noviY;

        //trazi dimenzije lat, lon
        public int checkMinMax = 1;
        public double praviXmax, praviXmin, praviYmax, praviYmin;

        //mesto na canvasu
        public double relativnoX, relativnoY;
        public double odstojanjeLon, odstojanjeLat;

        //pokupi poziciju klika
        public double poX, poY;

        //za poligon
        public List<double> koordinatePoX = new List<double>();
        public List<double> koordinatePoY = new List<double>();

        //undo, redo, clear
        public int numberChildren = 0;
        List<UIElement> obrisaniListaZaBrojanje = new List<UIElement>();
        List<UIElement> ponovoIscrtaj = new List<UIElement>();
        List<UIElement> saMape = new List<UIElement>(); //za clear

        //VIZUELIZACIJA 

        //bojenje vodova
        public string obojVod = "nemoj";
        public string obojPoOtpornosti = "nemoj";
        public List<UIElement> zaBrisanje = new List<UIElement>();

        //aktivan deo mreze
        public List<SwitchEntity> listaSviceva = new List<SwitchEntity>();
        public List<LineEntity> vodoviZaBrisanje = new List<LineEntity>();
        public List<LineEntity> listaNeobrisanihVodova = new List<LineEntity>();
        public List<PowerEntity> entitetiZaBrisanje = new List<PowerEntity>();
        public List<PowerEntity> listaNeobrisanihEntiteta = new List<PowerEntity>();
        public List<UIElement> elementZaBrisanje = new List<UIElement>();

        //Promena boje entiteta po broju konekcija
        public int brojac;
        public int brojac2;
        public int brojac3;
        public List<long> listaKonekcija = new List<long>();

        //bojenje slikom
        public string izabraoSliku = "ne";
        public string putanjaIzabraneSlike;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            UcitavanjeMatrice();
            UcitavanjeElemenata(); //iz Geographic.xml(/bin/debug)
            //Za broj konekcija entiteta
            FirstISecondEnd();
        }

        private void UcitavanjeMatrice()
        {
            //Pravim matricu
            Point rt;

            for (int i = 0; i <= 300; i++)
            {
                for (int j = 0; j <= 300; j++)
                {
                    rt = new Point(i, j);
                    matrica.Add(rt);
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            CrtajEntitete();
            CrtajVodove();

            // ovo mi treba za undo i redo da ne bih clearovao pocetnu mapu
            numberChildren = canvas.Children.Count;
        }

        private void CrtajEntitete()
        {
            foreach (var element in listaElemenataIzXML)
            {
                ToLatLon(element.X, element.Y, 34, out noviX, out noviY);
                MestoNaCanvasu(noviX, noviY, out relativnoX, out relativnoY);

                //za svaki slucaj RECT da ne okine edit na elipsu
                Rectangle rect = new Rectangle();
                rect.Fill = element.Boja;
                rect.ToolTip = element.ToolTip; //za hover prikaze info o elementu
                rect.Width = 2;
                rect.Height = 2;

                #region Bojenje po broju konekcija
                if(element.BrojKonekcija == "ignore")
                {
                    rect.Fill = element.Boja;
                }
                else if(element.BrojKonekcija == "do3")
                {
                    rect.Fill = Brushes.IndianRed;
                }
                else if(element.BrojKonekcija == "do5")
                {
                    rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                else if(element.BrojKonekcija == "preko5")
                {
                    rect.Fill = new SolidColorBrush(Color.FromRgb(195, 9, 19));
                }
                #endregion

                #region Bojenje slikom
                if (element.ObojSlikom == 1)
                {
                    if (izabraoSliku == "ne")
                    {
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        ofd.ShowDialog();
                        putanjaIzabraneSlike = ofd.FileName; //cela adresa selektovane slike

                        if (putanjaIzabraneSlike == "") {
                            string trenutno = Environment.CurrentDirectory;
                            putanjaIzabraneSlike = System.IO.Path.GetFullPath(System.IO.Path.Combine(trenutno, @"..\..\"));
                            putanjaIzabraneSlike = putanjaIzabraneSlike + "Screenshots\\" + "img1.png";
                        }

                        izabraoSliku = "da";
                    }
                    
                    BitmapImage bitmapImage = new BitmapImage(new Uri(putanjaIzabraneSlike, UriKind.Relative));
                    rect.Fill = new ImageBrush(bitmapImage);
                }
                #endregion

                #region Bez preklapanja
                Point mojaTacka = matrica.Find(pomocnaTacka => pomocnaTacka.X == relativnoX && pomocnaTacka.Y == relativnoY);

                int brojac = 1;
                if (!dictTackaElement.ContainsKey(mojaTacka))
                {
                    dictTackaElement.Add(mojaTacka, element);
                }
                else
                {
                    bool flag = false;
                    while (true)
                    {
                        for (double iksevi = relativnoX - brojac * 3; iksevi <= relativnoX + brojac * 3; iksevi += 3) //opet na oba 3 da se ne bi preklapali
                        {
                            if (iksevi < 0)
                                continue;
                            for (double ipsiloni = relativnoY - brojac * 3; ipsiloni <= relativnoY + brojac * 3; ipsiloni += 3)
                            {
                                if (ipsiloni < 0)
                                    continue;
                                mojaTacka = matrica.Find(t => t.X == iksevi && t.Y == ipsiloni);
                                if (!dictTackaElement.ContainsKey(mojaTacka))
                                {
                                    dictTackaElement.Add(mojaTacka, element);
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                                break;
                        }
                        if (flag)
                            break;

                        brojac++;
                    }
                }
                #endregion
                //mnozim sa 3 da ne bi bilo preklapanja
                Canvas.SetBottom(rect, mojaTacka.X*3);
                Canvas.SetLeft(rect, mojaTacka.Y*3);
                canvas.Children.Add(rect);
            }
        }

        bool iscrtaoPresek = false;
        private void CrtajVodove()
        {
            //crtanje vodova
            foreach (LineEntity line in listaVodova)
            {
                Point start, end;
                pronadiTacke(line, out start, out end);

                //ako je error za aktivan deo mreze
                if((end.X ==1000 && end.Y == 1000) || (start.X==1000 && start.Y==1000))
                {
                    continue;
                }

                //BFS
                line.PocetakX = start.X;
                line.PocetakY = start.Y;
                line.KrajX = end.X;
                line.KrajY = end.Y;
                BFSProlaz(line,start,end);
            }

            foreach (LineEntity line in listaVodova)
            {
                Point start, end;
                pronadiTacke(line, out start, out end);

                //ako je error za aktivan deo mreze
                if ((end.X == 1000 && end.Y == 1000) || (start.X == 1000 && start.Y == 1000))
                {
                    continue;
                }

                //BFS
                if (line.Prolaz != "1")
                {
                    BFSProlaz2(line, start, end);
                }

                //Ovo je da bih kasnije mogao posle da ih brisem i crtam ponovo
                line.Prolaz = "0";
            }
            iscrtaoPresek = true;
        }

        private void BFSProlaz(LineEntity line, Point start,Point end)
        {
            PozicijaPolja pocetak = new PozicijaPolja((int)line.PocetakX, (int)line.PocetakY);
            PozicijaPolja kraj = new PozicijaPolja((int)line.KrajX, (int)line.KrajY);
            PozicijaPolja[,] prev = BFSPath.BFSPronadji(line, BFSprom.BFSlinije);
            List<PozicijaPolja> putanja = BFSPath.RekonstruisanjePutanje(pocetak, kraj, prev);

        // ovo mi je bilo dok nisam imao BFS da proveri preklapanje vodova
        //    bool vecPostojiLinija;
            if(putanja == null)
            {
                //Nije nacrtao
                line.Prolaz = "0";
            }
            else
            {
                //Nacrtao
                line.Prolaz = "1";
                
                Point p1 = new Point(pocetak.PozY*3+1, -pocetak.PozX*3 + 900-1);
                Point p3 = new Point(kraj.PozY*3+1, -kraj.PozX*3 + 900-1);
                putanja.Remove(pocetak);
                putanja.Remove(kraj);

                if (start.X != end.X)
                {

                    Polyline polyline = new Polyline();
                    polyline.Stroke = Brushes.Orchid;
                    polyline.StrokeThickness = 0.5;
                    polyline.ToolTip = "Line\nID: " + line.Id + " Name: " + line.Name;

                    polyline.MouseRightButtonDown += promeniBoju_MouseRightButtonDown;

                    PointCollection points = new PointCollection();
                    points.Add(p1);
                    foreach (PozicijaPolja zauzeto in putanja)
                    {
                        BFSprom.BFSlinije[zauzeto.PozX, zauzeto.PozY] = 'X';
                        points.Add(new Point(zauzeto.PozY*3+1, -zauzeto.PozX*3+900-1));
                    }
                    points.Add(p3);
                    polyline.Points = points;

                        #region Bojenje voda po materijalu
                        if (obojVod == "oboj")
                        {
                            if (line.ConductorMaterial == "Steel")
                            {
                                polyline.Stroke = Brushes.Gray;
                            }
                            else if (line.ConductorMaterial == "Acsr")
                            {
                                polyline.Stroke = Brushes.LightSkyBlue;
                            }
                            else if (line.ConductorMaterial == "Copper")
                            {
                                polyline.Stroke = Brushes.Brown;
                            }
                        }
                        #endregion

                        #region Bojenje vodova po otpornosti
                        if (obojPoOtpornosti == "oboj")
                        {
                            if (line.R < 1)
                            {
                                polyline.Stroke = Brushes.Red;
                            }
                            else if (line.R >= 1 && line.R <= 2)
                            {
                                polyline.Stroke = Brushes.Orange;
                            }
                            else if (line.R > 2)
                            {
                                polyline.Stroke = Brushes.Yellow;
                            }
                        }
                        #endregion

                        canvas.Children.Add(polyline);
                }
            }
        }

        private void BFSProlaz2(LineEntity line, Point start, Point end)
        {
            PozicijaPolja pocetak = new PozicijaPolja((int)line.PocetakX, (int)line.PocetakY);
            PozicijaPolja kraj = new PozicijaPolja((int)line.KrajX, (int)line.KrajY);
            PozicijaPolja[,] prev = BFSPath.BFSPronadji(line, BFSprom.BFSlinije2);
            List<PozicijaPolja> putanja = BFSPath.RekonstruisanjePutanje(pocetak, kraj, prev);

            // ovo mi je bilo dok nisam imao BFS da proveri preklapanje vodova
            //    bool vecPostojiLinija;
            if (putanja == null)
            {
                //Nije nacrtao
                line.Prolaz = "0";
            }
            else
            {
                //Nacrtao
                line.Prolaz = "2";

                Point p1 = new Point(pocetak.PozY * 3 + 1, -pocetak.PozX * 3 + 900 - 1);
                Point p3 = new Point(kraj.PozY * 3 + 1, -kraj.PozX * 3 + 900 - 1);
                putanja.Remove(pocetak);
                putanja.Remove(kraj);

                if (start.X != end.X)
                {

                    Polyline polyline = new Polyline();
                    polyline.Stroke = Brushes.Orchid;
                    polyline.StrokeThickness = 0.5;
                    polyline.ToolTip = "Line\nID: " + line.Id + " Name: " + line.Name;

                    polyline.MouseRightButtonDown += promeniBoju_MouseRightButtonDown;

                    PointCollection points = new PointCollection();
                    points.Add(p1);
                    foreach (PozicijaPolja zauzeto in putanja)
                    {
                        if(BFSprom.BFSlinije[zauzeto.PozX, zauzeto.PozY] == 'X')
                        {
                            if (iscrtaoPresek == false)
                            {
                                Ellipse preklapanje = new Ellipse();
                                preklapanje.Height = 0.5;
                                preklapanje.Width = 0.5;
                                preklapanje.Fill = Brushes.Crimson;
                                Canvas.SetLeft(preklapanje, zauzeto.PozY * 3 + 0.5);
                                Canvas.SetTop(preklapanje, -zauzeto.PozX * 3 + 900 - 1.5);
                                canvas.Children.Add(preklapanje);
                            }
                        }
                        points.Add(new Point(zauzeto.PozY * 3 + 1, -zauzeto.PozX * 3 + 900 - 1));
                    }
                    points.Add(p3);
                    polyline.Points = points;

                        #region Bojenje voda po materijalu
                        if (obojVod == "oboj")
                        {
                            if (line.ConductorMaterial == "Steel")
                            {
                                polyline.Stroke = Brushes.Gray;
                            }
                            else if (line.ConductorMaterial == "Acsr")
                            {
                                polyline.Stroke = Brushes.LightSkyBlue;
                            }
                            else if (line.ConductorMaterial == "Copper")
                            {
                                polyline.Stroke = Brushes.Brown;
                            }
                        }
                        #endregion

                        #region Bojenje vodova po otpornosti
                        if (obojPoOtpornosti == "oboj")
                        {
                            if (line.R < 1)
                            {
                                polyline.Stroke = Brushes.Red;
                            }
                            else if (line.R >= 1 && line.R <= 2)
                            {
                                polyline.Stroke = Brushes.Orange;
                            }
                            else if (line.R > 2)
                            {
                                polyline.Stroke = Brushes.Yellow;
                            }
                        }
                    #endregion

                        canvas.Children.Add(polyline);
                }
            }
        }

        //Menjam boju entiteta na klik voda
        private void promeniBoju_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Polyline mojVod = (Polyline)sender;
            Point p1 = new Point();
            Point p2 = new Point();
            p1 = mojVod.Points.First();
            p2 = mojVod.Points.ElementAt(mojVod.Points.Count - 1);

            Rectangle r = new Rectangle();
            r.Fill = Brushes.Gold;
            r.Width = 3;
            r.Height = 3;
            Canvas.SetBottom(r, 900 - 1.5 - p1.Y); //matrica je 300, a mnozio sam sa 3
            Canvas.SetLeft(r, -1.5 + p1.X);
            canvas.Children.Add(r);

            Rectangle r2 = new Rectangle();
            r2.Fill = Brushes.Gold;
            r2.Width = 3;
            r2.Height = 3;
            Canvas.SetBottom(r2, 900 - 1.5 - p2.Y);
            Canvas.SetLeft(r2, -1.5 + p2.X);
            canvas.Children.Add(r2);
        }

        private void pronadiTacke(LineEntity le, out Point start, out Point end)
        {
            PowerEntity elem;

            elem = listaElemenataIzXML.Find(x => x.Id == le.FirstEnd);
            //moze se desiti error kod aktivnog dela mreze da vod nema pocetak/kraj
            //jer je entitet obrisan pa zato try-catch
            try
            {
                start = dictTackaElement.Where(x => x.Value == elem).First().Key;
            }
            catch (Exception ex)
            {
                start = new Point(1000, 1000);
            }

            elem = listaElemenataIzXML.Find(x => x.Id == le.SecondEnd);
            try
            {
                end = dictTackaElement.Where(x => x.Value == elem).First().Key;
            }
            catch (Exception ex)
            {
                end = new Point(1000, 1000);
            }
        }

        private void FindLatLon(double x, double y)
        {
            if (checkMinMax == 1)
            {
                praviXmax = noviX;
                praviYmax = noviY;
                praviXmin = noviX;
                praviYmin = noviY;

                checkMinMax++;
            }
            else
            {
                //proveraMAX
                if (noviX > praviXmax)
                {
                    praviXmax = noviX;
                }

                if (noviY > praviYmax)
                {
                    praviYmax = noviY;
                }

                //proveraMIN
                if (noviX < praviXmin)
                {
                    praviXmin = noviX;
                }

                if (noviY < praviYmin)
                {
                    praviYmin = noviY;
                }
            }
            odstojanjeLon = (praviXmax - praviXmin) / 300;
            odstojanjeLat = (praviYmax - praviYmin) / 300;
        }

        private void MestoNaCanvasu(double noviX, double noviY, out double relativnoX, out double relativnoY)
        {
            //tacke na osi - trenutna pozicija na canvasu / razmak od jedne do druge kocke
            //delim sa odstojanjem da bi svaka tacka nasla svoje mesto na canvasu
            //namestam da krecu od 0 zato oduzimanje
            relativnoX = Math.Round((noviX - praviXmin) / odstojanjeLon);
            relativnoY = Math.Round((noviY - praviYmin) / odstojanjeLat);
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (canvas.Children.Count > 0)
            {
                obrisaniListaZaBrojanje.Add(canvas.Children[canvas.Children.Count - 1]);
                canvas.Children.Remove(canvas.Children[canvas.Children.Count - 1]);
            }

            if (canvas.Children.Count != numberChildren)
            {
                for (int i = 0; i < ponovoIscrtaj.Count; i++)
                {
                    if (ponovoIscrtaj[i] != null)
                        canvas.Children.Add(ponovoIscrtaj[i]);
                }
            }

            for (int i = 0; i < ponovoIscrtaj.Count; i++)
            {
                ponovoIscrtaj[i] = null;
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (obrisaniListaZaBrojanje.Count > 0)
            {
                //vraca na prethodnu i onda je brise sa liste
                canvas.Children.Add(obrisaniListaZaBrojanje[obrisaniListaZaBrojanje.Count - 1]);
                obrisaniListaZaBrojanje.RemoveAt(obrisaniListaZaBrojanje.Count - 1);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            //brise samo one iscrtane objekte, a ne celu mapu
            if (canvas.Children.Count > 0)
            {
                // da ne bih obrisao mapu
                foreach (UIElement jedanOdElemenata in canvas.Children)
                {
                    saMape.Add(jedanOdElemenata);
                }

                // cuvam one koje zelim da crtam ponovo
                if (canvas.Children.Count > numberChildren)
                {
                    for (int i = numberChildren; i < canvas.Children.Count; i++)
                    {
                        ponovoIscrtaj.Add(canvas.Children[i]);
                    }
                }

                //brisem samo nacrtane
                foreach (var item in ponovoIscrtaj)
                {
                    canvas.Children.Remove(item);
                }

                /* Ovo je slucaj da bas sve obrise sa mape. Tu bih onda izbacio ovaj foreach gore
                 
                canvas.Children.Clear();

                //ponovo crtam pocetnu mapu
                for (int i = 0; i < numberChildren; i++)
                {
                    canvas.Children.Add(saMape[i]);
                }
                saMape.Clear();
                */

                numberChildren = canvas.Children.Count;
            }
        }

        #region Vizuelizacija
        private void AktivanDeoMreze_Click(object sender, RoutedEventArgs e)
        {
            if (aktivanDeoMrezeChecked.IsChecked)
            {
                //ne prikazuje vod koji izlazi iz Open prekidaca
                foreach (var vod in listaVodova)
                {
                    bool obrisati = false;
                    foreach (var sw in listaSviceva)
                    {
                        if (sw.Status == "Open" && sw.Id == vod.FirstEnd)
                        {
                            vodoviZaBrisanje.Add(vod);
                            obrisati = true;
                            break;
                        }
                    }
                    if (obrisati == true) continue;
                    listaNeobrisanihVodova.Add(vod);
                }

                //ne prikazuje entitet ciji je Id za TAJ(obrisan) vod SecondEnd
                foreach (var entitet in listaElemenataIzXML)
                {
                    bool obrisati = false;
                    foreach (var vod in vodoviZaBrisanje)
                    {
                        if (vod.SecondEnd == entitet.Id)
                        {
                            entitetiZaBrisanje.Add(entitet);
                            obrisati = true;
                            break;
                        }
                    }
                    if (obrisati == true) continue;
                    listaNeobrisanihEntiteta.Add(entitet);
                }

                listaElemenataIzXML = listaNeobrisanihEntiteta.ToList();
                listaVodova = listaNeobrisanihVodova.ToList();

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if(element.GetType() == typeof(Polyline) || element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }
                
                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                dictTackaElement.Clear();

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                CrtajEntitete();
                CrtajVodove();

                elementZaBrisanje.Clear();
                listaNeobrisanihEntiteta.Clear();
                listaNeobrisanihVodova.Clear();
                vodoviZaBrisanje.Clear();
                entitetiZaBrisanje.Clear();
            }
            else
            {
                //prvo obrisem sve
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Polyline) || element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }
                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                dictTackaElement.Clear();
                listaElemenataIzXML.Clear();
                listaVodova.Clear();
                listaSviceva.Clear();

                UcitavanjeElemenata();

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                CrtajEntitete();
                CrtajVodove();
            }
        }

        private void FirstISecondEnd()
        {
            foreach (var item in listaVodova)
            {
                listaKonekcija.Add(item.FirstEnd);
                listaKonekcija.Add(item.SecondEnd);
            }
        }
        private void Konekcija03_Click(object sender, RoutedEventArgs e)
        {
            if (konekcija03.IsChecked)
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    brojac = 0;
                    foreach (var konekcija in listaKonekcija)
                    {
                        if (entitet.Id == konekcija)
                        {
                            brojac++;
                        }
                        if (brojac >= 3) break;
                    }
                    if (brojac >= 3)
                    {
                        continue;
                    }
                    else
                    {
                        entitet.BrojKonekcija = "do3";
                    }
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                CrtajEntitete();
            }
            else
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    brojac = 0;
                    foreach (var konekcija in listaKonekcija)
                    {
                        if (entitet.Id == konekcija)
                        {
                            brojac++;
                        }
                        if (brojac >= 3) break;
                    }
                    if (brojac >= 3)
                    {
                        continue;
                    }
                    else
                    {
                        entitet.BrojKonekcija = "ignore";
                    }
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                CrtajEntitete();
            }
        }

        private void Konekcija35_Click(object sender, RoutedEventArgs e)
        {
            if (konekcija35.IsChecked)
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    brojac2 = 0;
                    foreach (var konekcija in listaKonekcija)
                    {
                        if (entitet.Id == konekcija)
                        {
                            brojac2++;
                        }
                        if (brojac2 > 5) break;
                    }
                    if (brojac2 < 3 || brojac2 > 5)
                    {
                        continue;
                    }
                    else
                    {
                        entitet.BrojKonekcija = "do5";
                    }
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                CrtajEntitete();
            }
            else
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    brojac2 = 0;
                    foreach (var konekcija in listaKonekcija)
                    {
                        if (entitet.Id == konekcija)
                        {
                            brojac2++;
                        }
                        if (brojac2 > 5) break;
                    }
                    if (brojac2 < 3 || brojac2 > 5)
                    {
                        continue;
                    }
                    else
                    {
                        entitet.BrojKonekcija = "ignore";
                    }
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                CrtajEntitete();
            }
        }

        private void Konekcija5_Click(object sender, RoutedEventArgs e)
        {
            if (konekcija5.IsChecked)
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    brojac3 = 0;
                    foreach (var konekcija in listaKonekcija)
                    {
                        if (entitet.Id == konekcija)
                        {
                            brojac3++;
                        }
                        if (brojac3 > 5) break;
                    }
                    if (brojac3 <= 5)
                    {
                        continue;
                    }
                    else
                    {
                        entitet.BrojKonekcija = "preko5";
                    }
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                CrtajEntitete();
            }
            else
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    brojac3 = 0;
                    foreach (var konekcija in listaKonekcija)
                    {
                        if (entitet.Id == konekcija)
                        {
                            brojac3++;
                        }
                        if (brojac3 > 5) break;
                    }
                    if (brojac3 <= 5)
                    {
                        continue;
                    }
                    else
                    {
                        entitet.BrojKonekcija = "ignore";
                    }
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                CrtajEntitete();
            }
        }

        //po vrsti materijala
        private void BojenjeVodova_Click(object sender, RoutedEventArgs e)
        {
            if (bojenjeVodova.IsChecked)
            {
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Polyline))
                    {
                        zaBrisanje.Add(element);
                    }
                }

                obojVod = "oboj";
                foreach (var vod in zaBrisanje)
                {
                    canvas.Children.Remove(vod);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                CrtajVodove();
                zaBrisanje.Clear();
            }
            else
            {
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Polyline))
                    {
                        zaBrisanje.Add(element);
                    }
                }

                obojVod = "nemoj";
                foreach (var vod in zaBrisanje)
                {
                    canvas.Children.Remove(vod);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                CrtajVodove();
                zaBrisanje.Clear();
            }
        }

        //po otpornosti
        private void PromeniBojuVodovaOtpornost_Click(object sender, RoutedEventArgs e)
        {
            if (promeniBojuVodova.IsChecked)
            {
                foreach (UIElement element in canvas.Children)
                {
                    if(element.GetType() == typeof(Polyline))
                    {
                        zaBrisanje.Add(element);
                    }
                }

                obojPoOtpornosti = "oboj";
                foreach (var vod in zaBrisanje)
                {
                    canvas.Children.Remove(vod);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                CrtajVodove();
                zaBrisanje.Clear();
            }
            else
            {
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Polyline))
                    {
                        zaBrisanje.Add(element);
                    }
                }

                obojPoOtpornosti = "nemoj";
                foreach (var vod in zaBrisanje)
                {
                    canvas.Children.Remove(vod);
                }

                BFSprom.BFSlinije = new char[301, 301];
                BFSprom.BFSlinije2 = new char[301, 301];

                CrtajVodove();
                zaBrisanje.Clear();
            }
        }

        private void PromeniBojuEntitetaSlika_Click(object sender, RoutedEventArgs e)
        {
            if (bojaEntitetaSlika.IsChecked)
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    entitet.ObojSlikom = 1;
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                CrtajEntitete();
            }
            else
            {
                foreach (var entitet in listaElemenataIzXML)
                {
                    entitet.ObojSlikom = 0;
                }

                //brisem na ovaj nacin da ne bih obrisao oblike
                foreach (UIElement element in canvas.Children)
                {
                    if (element.GetType() == typeof(Rectangle))
                    {
                        elementZaBrisanje.Add(element);
                    }
                }

                foreach (UIElement el in elementZaBrisanje)
                {
                    canvas.Children.Remove(el);
                }

                dictTackaElement.Clear();
                elementZaBrisanje.Clear();
                izabraoSliku = "ne"; //restartujem da omogucim ponovni izbor slike
                putanjaIzabraneSlike = "";
                CrtajEntitete();
            }
        }

        //Cuvam ceo ekran jer bi zoom menjao izgled
        private void SacuvajKaoSliku_Click(object sender, RoutedEventArgs e)
        {
            // Ceo ekran
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            //uzme dimenzije canvasa, ali nisam uspeo da nadjem pocetne tacke
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                (int)canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            //Stavio sam ceo naziv da mi ne remeti Brushes i Rectangle
            // Project > Add reference > System.Drawing
            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                {
                    String filename = DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".png";
                    Opacity = .0;
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    string pozicijaFoldera = Environment.CurrentDirectory;
                    string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(pozicijaFoldera, @"..\..\"));
                    path = path+"Screenshots\\"+filename;
                    bmp.Save(path);
                    Opacity = 1;
                }

            }
        }
        #endregion

        private void LeviPromeniNesto_Click(object sender, MouseButtonEventArgs e)
        {
            //Update za kliknut objekat
            if (e.OriginalSource is Ellipse)
            {
                Ellipse ClickedRectangle = (Ellipse)e.OriginalSource;

                //otvori ovu elipsu
                canvas.Children.Remove(ClickedRectangle);

                //za textBlock
                string bojaTeksta = "Black", samTekst = "nekiTekst";
                foreach (FrameworkElement item in canvas.Children)
                {
                    if (item.Name == ClickedRectangle.Name + "eltb") //treba mi item.Name, a Name(F12) vodi na FrameworkElement
                    {
                        canvas.Children.Remove(item);
                        bojaTeksta = ((TextBlock)item).Foreground.ToString();
                        samTekst = ((TextBlock)item).Text;
                        break;
                    }
                }

                EditElipsa editElipsa = new EditElipsa(ClickedRectangle.Height, ClickedRectangle.Width, ClickedRectangle.StrokeThickness, ClickedRectangle.Fill, bojaTeksta, samTekst, ClickedRectangle.Opacity);
                editElipsa.Show();

            }
            else if (e.OriginalSource is Polygon)
            {
                Polygon ClickedRectangle = (Polygon)e.OriginalSource;

                canvas.Children.Remove(ClickedRectangle);

                //za textBlock
                string bojaTeksta = "Black", samTekst = "nekiTekst";
                foreach (FrameworkElement item in canvas.Children)
                {
                    if (item.Name == ClickedRectangle.Name + "pgtb") //treba mi item.Name, a Name(F12) vodi na FrameworkElement
                    {
                        canvas.Children.Remove(item);
                        bojaTeksta = ((TextBlock)item).Foreground.ToString();
                        samTekst = ((TextBlock)item).Text;
                        break;
                    }
                }

                EditPoligon editPoligon = new EditPoligon(ClickedRectangle.StrokeThickness, ClickedRectangle.Fill.ToString(), bojaTeksta, samTekst, ClickedRectangle.Points, ClickedRectangle.Opacity);
                editPoligon.Show();
            }
            else if (e.OriginalSource is TextBlock)
            {
                TextBlock ClickedRectangle = (TextBlock)e.OriginalSource;

                string slova = ClickedRectangle.Name;
                slova = slova.Substring(8, slova.Length - 8);
                if (slova != "pgtb" && slova != "eltb")
                {
                    //otvori ovaj tekst
                    canvas.Children.Remove(ClickedRectangle);
                    EditText editTekst = new EditText(ClickedRectangle.FontSize, ClickedRectangle.Foreground, ClickedRectangle.Text);
                    editTekst.Show();
                }
            }
        }

        private void LeviPoligon_Click(object sender, MouseButtonEventArgs e)
        {
            Poligon poligonCrtez = new Poligon();

            //grupisem da samo 1 moze da se izabere
            int i = 1;

            if (EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true || EllipseChecked.IsChecked == true && TextChecked.IsChecked == true ||
                EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true && TextChecked.IsChecked == true ||
                PolygonChecked.IsChecked == true && TextChecked.IsChecked == true)
            {
                i = 2;
                MessageBox.Show("Selektujte iskljucivo jednu opciju", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (i == 1 && PolygonChecked.IsChecked == true && koordinatePoX.Count >= 3)
            {
                poligonCrtez.Show();
            }
            else if (PolygonChecked.IsChecked == true)
            {
                MessageBox.Show("Morate izvrsiti desni klik bar 3 puta ako zelite da dodate nov poligon", "Greska!", MessageBoxButton.OK, MessageBoxImage.Information);
                koordinatePoX.Clear();
                koordinatePoY.Clear();
            }
        }

        private void Right_ClickBiloGde(object sender, MouseButtonEventArgs e)
        {
            //grupisem da samo 1 moze da se izabere
            int i = 1;

            if (EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true || EllipseChecked.IsChecked == true && TextChecked.IsChecked == true ||
                EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true && TextChecked.IsChecked == true ||
                PolygonChecked.IsChecked == true && TextChecked.IsChecked == true)
            {
                i = 2;
                MessageBox.Show("Selektujte iskljucivo jednu opciju", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (i == 1)
            {
                if (EllipseChecked.IsChecked == true)
                {
                    Elipsa elipsaCrtez = new Elipsa();

                    //Pokupio ih je
                    poX = Mouse.GetPosition(canvas).X;
                    poY = Mouse.GetPosition(canvas).Y;

                    elipsaCrtez.Show();
                }
                else if (PolygonChecked.IsChecked == true)
                {
                    poX = Mouse.GetPosition(canvas).X;
                    poY = Mouse.GetPosition(canvas).Y;

                    koordinatePoX.Add(poX);
                    koordinatePoY.Add(poY);
                }
                else if (TextChecked.IsChecked == true)
                {
                    AddText dodajTekstCrtez = new AddText();

                    poX = Mouse.GetPosition(canvas).X;
                    poY = Mouse.GetPosition(canvas).Y;

                    dodajTekstCrtez.Show();
                }
            }
        }

        #region UcitavanjeElemenata
        private void UcitavanjeElemenata()
        {
            // ---------Ucitavam elemente iz xml-a
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml"); // bin/debug
            XmlNodeList nodeList;

            //substations - trafostanice
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {
                SubstationEntity subEn = new SubstationEntity();
                subEn.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                subEn.Name = node.SelectSingleNode("Name").InnerText;
                subEn.X = double.Parse(node.SelectSingleNode("X").InnerText);
                subEn.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                subEn.ToolTip = "Substation\nID: " + subEn.Id + "  Name: " + subEn.Name;

                ToLatLon(subEn.X, subEn.Y, 34, out noviX, out noviY);
                FindLatLon(noviX, noviY);
                listaElemenataIzXML.Add(subEn);
            }

            //svicevi
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                SwitchEntity sw = new SwitchEntity();
                sw.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sw.Name = node.SelectSingleNode("Name").InnerText;
                sw.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sw.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                sw.Status = node.SelectSingleNode("Status").InnerText;
                sw.ToolTip = "Switch\nID: " + sw.Id + "  Name: " + sw.Name + " Status: " + sw.Status;

                ToLatLon(sw.X, sw.Y, 34, out noviX, out noviY);
                FindLatLon(noviX, noviY);
                listaElemenataIzXML.Add(sw);
                listaSviceva.Add(sw);
            }

            //nodovi
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                NodeEntity nod = new NodeEntity();
                nod.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                nod.Name = node.SelectSingleNode("Name").InnerText;
                nod.X = double.Parse(node.SelectSingleNode("X").InnerText);
                nod.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                nod.ToolTip = "Node\nID: " + nod.Id + "  Name: " + nod.Name;

                ToLatLon(nod.X, nod.Y, 34, out noviX, out noviY);
                FindLatLon(noviX, noviY);
                listaElemenataIzXML.Add(nod);
            }

            //ucitavanje vodova ->  u xml first end i second end
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                LineEntity l = new LineEntity();
                l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                l.Name = node.SelectSingleNode("Name").InnerText;
                if (node.SelectSingleNode("IsUnderground").InnerText.Equals("true"))
                {
                    l.IsUnderground = true;
                }
                else
                {
                    l.IsUnderground = false;
                }
                l.R = float.Parse(node.SelectSingleNode("R").InnerText);
                l.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                l.LineType = node.SelectSingleNode("LineType").InnerText;
                l.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);
                l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);

                // da li postoje firstEnd i secondEnd medju entitetima
                // ako ne ignorisi vod
                // gleda vodove medju vec postojecim entitetima
                if (listaElemenataIzXML.Any(x => x.Id == l.FirstEnd))
                {
                    if (listaElemenataIzXML.Any(x => x.Id == l.SecondEnd))
                    {
                        listaVodova.Add(l);
                    }
                }

                //brisanje duplikata
                while (listaVodova.Any(x => x.Id != l.Id && x.FirstEnd == l.FirstEnd && x.SecondEnd == l.SecondEnd))
                {
                    vodDuplikat = listaVodova.FindAll(x => x.Id != l.Id && x.FirstEnd == l.FirstEnd && x.SecondEnd == l.SecondEnd);
                    foreach (LineEntity dupli in vodDuplikat)
                    {
                        listaVodova.Remove(dupli);
                    }
                    vodDuplikat.Clear();
                }
            }
        }
        #endregion

        #region ToLatLon
        //From UTM to Latitude and longitude in decimal
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
        #endregion
    }
}
