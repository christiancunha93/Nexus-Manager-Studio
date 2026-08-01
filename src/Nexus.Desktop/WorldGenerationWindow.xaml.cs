using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Nexus.Desktop.Models;
using Nexus.Desktop.Services;

namespace Nexus.Desktop;

public partial class WorldGenerationWindow : Window
{
    private readonly CareerSetup _setup;
    private readonly UniverseGenerator _generator = new();
    private readonly SaveGameService _saveGameService = new();

    public WorldGenerationWindow(CareerSetup setup)
    {
        _setup = setup;
        InitializeComponent();
        Loaded += WorldGenerationWindow_Loaded;
    }

    private async void WorldGenerationWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await UpdateProgress("Validando dados do treinador...", 10);
            await UpdateProgress("Criando estrutura continental...", 23);
            await UpdateProgress("Gerando liga e clubes...", 40);

            var career = await Task.Run(() => _generator.Generate(_setup));

            var clubs = career.Universe.Leagues.Sum(league => league.Clubs.Count);
            var players = career.Universe.Leagues.Sum(
                league => league.Clubs.Sum(club => club.Players.Count));

            ClubsCountText.Text = clubs.ToString();
            PlayersCountText.Text = players.ToString();

            await UpdateProgress("Criando jogadores e comissões...", 60);
            await UpdateProgress("Preparando economia e reputações...", 74);

            NewsCountText.Text = career.Universe.News.Count.ToString();

            await UpdateProgress("Gerando imprensa e notícias...", 86);
            await UpdateProgress("Gravando o primeiro save...", 94);

            await Task.Run(() => _saveGameService.Save(career));

            await UpdateProgress("Universo criado. Entrando na carreira...", 100);
            await Task.Delay(500);

            var shell = new ShellWindow(career);
            Application.Current.MainWindow = shell;
            shell.Show();

            Owner?.Close();
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                exception.ToString(),
                "Falha ao criar universo",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Close();
        }
    }

    private async Task UpdateProgress(string message, double progress)
    {
        GenerationStatusText.Text = message;
        GenerationProgressBar.Value = progress;
        await Task.Delay(380);
    }
}
