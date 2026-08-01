using System.Windows;
using Nexus.Desktop.Services;

namespace Nexus.Desktop;

public partial class MainMenuWindow : Window
{
    private readonly SaveGameService _saveGameService = new();

    public MainMenuWindow()
    {
        InitializeComponent();
    }

    private void ContinueCareerButton_Click(object sender, RoutedEventArgs e)
    {
        var career = _saveGameService.LoadLatest();

        if (career is null)
        {
            MenuStatusText.Text = "Nenhuma carreira foi encontrada. Crie um novo universo.";
            return;
        }

        OpenShell(career);
    }

    private void NewCareerButton_Click(object sender, RoutedEventArgs e)
    {
        var wizard = new NewCareerWindow
        {
            Owner = this
        };

        wizard.ShowDialog();
    }

    private void LoadCareerButton_Click(object sender, RoutedEventArgs e)
    {
        var saves = new SaveSelectionWindow
        {
            Owner = this
        };

        saves.ShowDialog();
    }

    private void EditorButton_Click(object sender, RoutedEventArgs e)
    {
        MenuStatusText.Text = "Editor Nexus em desenvolvimento.";
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var settings = new SettingsWindow
        {
            Owner = this
        };

        settings.ShowDialog();
    }

    private void CreditsButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "NEXUS MANAGER STUDIO\n\nCreator & Game Director\nChristian Marlon\n\nSprint 009 • The Birth of a World",
            "Créditos",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OpenShell(Models.CareerSave career)
    {
        var shell = new ShellWindow(career);
        Application.Current.MainWindow = shell;
        shell.Show();
        Close();
    }
}
