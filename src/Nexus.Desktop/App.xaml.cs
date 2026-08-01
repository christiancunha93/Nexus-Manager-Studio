using System.Windows;
using System.Windows.Threading;

namespace Nexus.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += (_, args) =>
        {
            MessageBox.Show(
                args.Exception.ToString(),
                "Erro inesperado no Nexus Manager",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            args.Handled = true;
        };

        var splash = new SplashWindow();
        MainWindow = splash;
        splash.Show();
    }
}
