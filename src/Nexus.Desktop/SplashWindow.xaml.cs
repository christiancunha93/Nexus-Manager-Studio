using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Nexus.Desktop;

public partial class SplashWindow : Window
{
    private readonly IReadOnlyList<(string Message, int Progress)> _steps =
        new List<(string Message, int Progress)>
        {
            ("Inicializando núcleo...", 15),
            ("Carregando interface...", 32),
            ("Verificando saves...", 51),
            ("Preparando banco de dados...", 70),
            ("Inicializando mundo...", 88),
            ("Entrando no Nexus...", 100)
        };

    public SplashWindow()
    {
        InitializeComponent();
        Loaded += SplashWindow_Loaded;
    }

    private async void SplashWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await AnimateOpacityAsync(0, 1, 500);

        foreach (var step in _steps)
        {
            LoadingStatusText.Text = step.Message;
            LoadingProgressBar.Value = step.Progress;
            await Task.Delay(350);
        }

        await Task.Delay(250);
        await AnimateOpacityAsync(1, 0, 400);

        var mainMenu = new MainMenuWindow();
        Application.Current.MainWindow = mainMenu;
        mainMenu.Show();
        Close();
    }

    private Task AnimateOpacityAsync(double from, double to, int milliseconds)
    {
        var completion = new TaskCompletionSource<bool>();

        var animation = new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = TimeSpan.FromMilliseconds(milliseconds)
        };

        animation.Completed += (_, _) => completion.TrySetResult(true);
        BeginAnimation(OpacityProperty, animation);

        return completion.Task;
    }
}
