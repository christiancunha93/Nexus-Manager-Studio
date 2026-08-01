using System.Linq;
using System.Windows;
using Nexus.Desktop.Models;
using Nexus.Desktop.Services;

namespace Nexus.Desktop;

public partial class ShellWindow : Window
{
    private readonly SaveGameService _saveGameService = new();
    private CareerSave _career;

    public ShellWindow()
        : this(new SaveGameService().LoadLatest() ?? CreateFallbackCareer())
    {
    }

    public ShellWindow(CareerSave career)
    {
        _career = career;
        InitializeComponent();
        PopulateCareer();
    }

    private void PopulateCareer()
    {
        var club = SaveGameService.FindSelectedClub(_career);
        var clubsCount = _career.Universe.Leagues.Sum(league => league.Clubs.Count);
        var playersCount = _career.Universe.Leagues.Sum(
            league => league.Clubs.Sum(item => item.Players.Count));
        var news = _career.Universe.News
            .OrderByDescending(item => item.Date)
            .FirstOrDefault();

        ManagerNameText.Text = _career.ManagerName;
        ClubSeasonText.Text = $"{club?.Name ?? "Sem clube"} • {_career.Universe.CurrentDate.Year}";
        CurrentDateText.Text = _career.Universe.CurrentDate.ToString("dd/MM/yyyy");
        WelcomeText.Text = $"Bem-vindo ao {club?.Name ?? "Nexus"}";
        ClubsText.Text = clubsCount.ToString();
        PlayersText.Text = playersCount.ToString();
        BalanceText.Text = club is null
            ? "R$ 0"
            : $"R$ {club.Balance / 1_000_000.0:0.0} mi";
        ReputationText.Text = club?.Reputation.ToString() ?? "0";
        HeadlineText.Text = news?.Headline ?? "Universo criado";
        NewsSummaryText.Text = news?.Summary ?? "Sua nova carreira está pronta.";
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _saveGameService.Save(_career);
        StatusText.Text = $"Carreira salva às {_career.LastSavedAt:HH:mm:ss}.";
    }

    private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
    {
        _saveGameService.Save(_career);

        var menu = new MainMenuWindow();
        Application.Current.MainWindow = menu;
        menu.Show();
        Close();
    }

    private static CareerSave CreateFallbackCareer()
    {
        var setup = new CareerSetup();
        return new UniverseGenerator().Generate(setup);
    }
}
