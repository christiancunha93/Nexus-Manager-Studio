using System.Windows;

namespace Nexus.Desktop;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "Configurações salvas para demonstração.",
            "Nexus Manager",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        Close();
    }
}
