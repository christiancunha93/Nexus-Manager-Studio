using System.Windows;
using System.Windows.Controls;
using Nexus.Desktop.Models;

namespace Nexus.Desktop;

public partial class NewCareerWindow : Window
{
    private int _currentStep = 1;
    private readonly CareerSetup _setup = new();

    public NewCareerWindow()
    {
        InitializeComponent();
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentStep == 1)
        {
            _setup.ManagerName = string.IsNullOrWhiteSpace(ManagerNameTextBox.Text)
                ? "Treinador"
                : ManagerNameTextBox.Text.Trim();

            _setup.Nationality = GetSelectedText(NationalityComboBox);
            _setup.Background = GetSelectedText(BackgroundComboBox);

            ShowStep(2);
            return;
        }

        if (_currentStep == 2)
        {
            ShowStep(3);
            return;
        }

        _setup.Country = GetSelectedText(CountryComboBox);
        _setup.League = GetSelectedText(LeagueComboBox);
        _setup.Club = GetSelectedText(ClubComboBox);

        var generator = new WorldGenerationWindow(_setup)
        {
            Owner = Owner ?? this
        };

        generator.Show();
        Hide();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentStep == 1)
        {
            Close();
            return;
        }

        ShowStep(_currentStep - 1);
    }

    private void ContinentButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        _setup.Continent = button.Content?.ToString() switch
        {
            "EUROPA" => "Europa",
            "AMÉRICA DO NORTE" => "América do Norte",
            _ => "América do Sul"
        };

        SouthAmericaButton.Opacity = button == SouthAmericaButton ? 1 : 0.55;
        EuropeButton.Opacity = button == EuropeButton ? 1 : 0.55;
        NorthAmericaButton.Opacity = button == NorthAmericaButton ? 1 : 0.55;
    }

    private void ShowStep(int step)
    {
        _currentStep = step;

        StepOnePanel.Visibility = step == 1 ? Visibility.Visible : Visibility.Collapsed;
        StepTwoPanel.Visibility = step == 2 ? Visibility.Visible : Visibility.Collapsed;
        StepThreePanel.Visibility = step == 3 ? Visibility.Visible : Visibility.Collapsed;

        StepSubtitleText.Text = step switch
        {
            1 => "Etapa 1 de 3 • Perfil do treinador",
            2 => "Etapa 2 de 3 • Escolha do continente",
            _ => "Etapa 3 de 3 • País, liga e clube"
        };

        StepNumberText.Text = $"0{step} / 03";
        WizardProgressBar.Value = step switch
        {
            1 => 33,
            2 => 66,
            _ => 100
        };

        NextButton.Content = step == 3 ? "CRIAR UNIVERSO" : "CONTINUAR";
    }

    private static string GetSelectedText(ComboBox comboBox)
    {
        return comboBox.SelectedItem is ComboBoxItem item
            ? item.Content?.ToString() ?? string.Empty
            : string.Empty;
    }
}
