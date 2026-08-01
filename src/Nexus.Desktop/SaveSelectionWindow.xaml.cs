using System.Windows;
using Nexus.Desktop.Services;

namespace Nexus.Desktop;

public partial class SaveSelectionWindow : Window
{
    private readonly SaveGameService _saveGameService = new();

    public SaveSelectionWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => RefreshSaves();
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        if (SavesListBox.SelectedItem is not SaveSlotInfo selected)
        {
            MessageBox.Show(
                "Selecione uma carreira.",
                "Nexus Manager",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        var career = _saveGameService.Load(selected.FilePath);
        if (career is null)
        {
            MessageBox.Show(
                "Não foi possível carregar este save.",
                "Nexus Manager",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var shell = new ShellWindow(career);
        Application.Current.MainWindow = shell;
        shell.Show();

        Owner?.Close();
        Close();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (SavesListBox.SelectedItem is not SaveSlotInfo selected)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Excluir a carreira de {selected.ManagerName}?",
            "Excluir carreira",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        _saveGameService.Delete(selected.FilePath);
        RefreshSaves();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void RefreshSaves()
    {
        SavesListBox.ItemsSource = _saveGameService.GetSaveSlots();
        if (SavesListBox.Items.Count > 0)
        {
            SavesListBox.SelectedIndex = 0;
        }
    }
}
