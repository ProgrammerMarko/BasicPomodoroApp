using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace PomodoroApp;

public partial class MainWindow : Window
{
    private DispatcherTimer _timer;
    private int _preostaloSekundi;
    private bool _jePokrenuto = false;
    private bool _jeRadFaza = true;

    public MainWindow()
    {
        InitializeComponent();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

        PostaviPočetnoStanje();
    }

    private void PostaviPočetnoStanje()
    {
        _jeRadFaza = true;
        _preostaloSekundi = int.TryParse(tbRad.Text, out int min) ? min * 60 : 25 * 60;
        AzurirajPrikaz();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _preostaloSekundi--;

        if (_preostaloSekundi <= 0)
        {
            _jeRadFaza = !_jeRadFaza;
            string? tekst = _jeRadFaza ? tbRad.Text : tbOdmor.Text;
            int zadano = _jeRadFaza ? 25 : 5;
            _preostaloSekundi = int.TryParse(tekst, out int min) ? min * 60 : zadano * 60;
        }

        AzurirajPrikaz();
    }

    private void AzurirajPrikaz()
    {
        int min = _preostaloSekundi / 60;
        int sek = _preostaloSekundi % 60;
        string prikaz = $"{min:D2}:{sek:D2}";

        lblVrijeme.Content = prikaz;
        Title = _jePokrenuto ? $"[{prikaz}] PomodoroApp" : "PomodoroApp";
    }

    private void BtnStartStop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_jePokrenuto)
        {
            _timer.Stop();
            _jePokrenuto = false;
            btnStartStop.Content = "Start";
            tbRad.IsEnabled = true;
            tbOdmor.IsEnabled = true;
        }
        else
        {
            _timer.Start();
            _jePokrenuto = true;
            btnStartStop.Content = "Stop";
            tbRad.IsEnabled = false;
            tbOdmor.IsEnabled = false;
        }

        AzurirajPrikaz();
    }

    private void BtnReset_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _timer.Stop();
        _jePokrenuto = false;
        btnStartStop.Content = "Start";
        tbRad.IsEnabled = true;
        tbOdmor.IsEnabled = true;
        PostaviPočetnoStanje();
    }
}