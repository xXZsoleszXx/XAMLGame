using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XAMLGame
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    
    static List<FontAwesomeIcon> fonts = new List<FontAwesomeIcon>() {FontAwesomeIcon.Microchip,FontAwesomeIcon.Money,FontAwesomeIcon.Mobile,FontAwesomeIcon.Opera,FontAwesomeIcon.Motorcycle};
    int elozo, jelenlegi;
    long pont;
    bool jatekvan=false;
    DispatcherTimer masodpercOra;
    
    TimeSpan jatekido = TimeSpan.FromSeconds(0);

    Stopwatch reakcioIdo;

    List<long> atlagosak = new List<long>();
    public MainWindow()
    {
      InitializeComponent();
      Alaphelyzet();
      JobbKepValtozas();
     
    }

    private void Balkepvaltozas(bool isGood)
    {
      var animacioKi = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
      
      if (isGood)
      {
        balkep.Icon = FontAwesomeIcon.Check;
        balkep.Foreground = Brushes.LightGreen;
      }
      else
      {
        balkep.Icon = FontAwesomeIcon.Times;
        balkep.Foreground = Brushes.Red;
      }
      balkep.BeginAnimation(OpacityProperty, animacioKi);
    }

    private void Pontszamitas(bool isGood)
    {

      if (isGood)
      {
        pont += 10000000 / atlagosak.Last()/100;
      }
      else
      {
        pont -= atlagosak.Last()/10;
      }

      lblpont.Content = pont.ToString();
    }

    private void JobbKepValtozas()
    {
      Random rnd = new Random(Guid.NewGuid().GetHashCode());
      elozo = jelenlegi;
      jelenlegi = rnd.Next(0, fonts.Count);
      jobbkep.Icon = fonts[jelenlegi];
      Debug.WriteLine($"{elozo}, {jelenlegi}");
      var animacioBe = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500));
      jobbkep.BeginAnimation(OpacityProperty,animacioBe);
      reakcioIdo.Restart();
    }

    private void Alaphelyzet()
    {
      btnIgen.IsEnabled = false;
      btnNem.IsEnabled = false;
      balkep.Icon = FontAwesomeIcon.None;
      lblpont.Content = 0;
      reakcioIdo = new Stopwatch();
      masodpercOra = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, Masodpercenkent, Application.Current.Dispatcher);
      masodpercOra.Stop();

    }

    private void Masodpercenkent(object sender, EventArgs e)
    {
      jatekido += TimeSpan.FromSeconds(1);
      lblido.Content = $"{jatekido.Minutes}:{jatekido.Seconds}";
    }

    private void btnInditas_Click(object sender, RoutedEventArgs e)
    {
      Inditasvalasz();
      
    }

    private void Inditasvalasz()
    {
      if (!jatekvan)
      {
        JobbKepValtozas();
        btnInditas.IsEnabled = false;
        btnIgen.IsEnabled = true;
        btnNem.IsEnabled = true;
        jatekvan = true;
        masodpercOra.Start();
      }
    }

    private void btnIgen_Click(object sender, RoutedEventArgs e)
    {
      //Debug.WriteLine("jobbgomb klikk");
      Igenvalasz();
    }

    private void Igenvalasz()
    {
      if (jatekvan)
      {
        Reakcioidoertekeles();
        if (elozo == jelenlegi)
        {
          Jovalaszesete();
        }
        else
        {
          Roszvalaszesete();
        }
      }
    }

    private void Reakcioidoertekeles()
    {
      reakcioIdo.Stop();
      atlagosak.Add(reakcioIdo.ElapsedMilliseconds);
      lblreakcio.Content = $"{atlagosak.Last()}ms/{atlagosak.Average():N4}ms";
    }

    private void Roszvalaszesete()
    {
      Debug.WriteLine("rossz");
      Pontszamitas(false);
      JobbKepValtozas();
      Balkepvaltozas(false);
    }

    private void Jovalaszesete()
    {
      Debug.WriteLine("jó");
      Pontszamitas(true);
      JobbKepValtozas();
      Balkepvaltozas(true);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Up:
          Inditasvalasz();
          break;
        case Key.Right:
          Nemvalasz();
          break;
        case Key.Left:
          Igenvalasz();
          break;
       
      }
    }

    private void btnNem_Click(object sender, RoutedEventArgs e)
    {
      //Debug.WriteLine("balgomb klikk");
      Nemvalasz();
    }

    private void Nemvalasz()
    {
      if (jatekvan)
      {
        Reakcioidoertekeles();
        if (elozo != jelenlegi)
        {
          Jovalaszesete();
        }
        else
        {
          Roszvalaszesete();
        }
      }
      
    }
  }
}
