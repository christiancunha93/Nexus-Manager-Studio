using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Desktop.Models;

namespace Nexus.Desktop.Services;

public sealed class UniverseGenerator
{
    private static readonly string[] FirstNames =
    {
        "Rafael", "Lucas", "Gabriel", "Matheus", "Bruno", "João",
        "Caio", "Vitor", "Pedro", "Henrique", "Samuel", "Diego",
        "Igor", "Renato", "Eduardo", "Thiago", "Felipe", "André"
    };

    private static readonly string[] LastNames =
    {
        "Silva", "Souza", "Rocha", "Mendes", "Lima", "Costa",
        "Alves", "Pereira", "Nunes", "Gomes", "Barros", "Ribeiro",
        "Martins", "Freitas", "Teixeira", "Oliveira"
    };

    private static readonly string[] Positions =
    {
        "GOL", "GOL", "ZAG", "ZAG", "ZAG", "LAT", "LAT",
        "VOL", "VOL", "MEI", "MEI", "MEI", "PE", "PD", "ATA",
        "ATA", "ATA", "MEI"
    };

    public CareerSave Generate(CareerSetup setup)
    {
        var seed = HashCode.Combine(
            setup.ManagerName,
            setup.Country,
            setup.Club,
            DateTime.UtcNow.Ticks);

        var random = new Random(seed);
        var league = new LeagueData
        {
            Name = setup.League,
            Country = setup.Country
        };

        var clubNames = new[]
        {
            setup.Club,
            "Bahia Imperial",
            "Real Metropolitano",
            "União Paulista",
            "Atlético do Vale",
            "Nacional Serrano",
            "Ferroviário Azul",
            "Estrela do Norte"
        }
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();

        while (clubNames.Count < 8)
        {
            clubNames.Add($"Clube Nexus {clubNames.Count + 1}");
        }

        foreach (var clubName in clubNames.Take(8))
        {
            league.Clubs.Add(CreateClub(clubName, random));
        }

        var selectedClub = league.Clubs.FirstOrDefault(
            club => club.Name.Equals(setup.Club, StringComparison.OrdinalIgnoreCase))
            ?? league.Clubs[0];

        var universe = new UniverseData
        {
            Seed = seed,
            CreatedAt = DateTime.Now,
            CurrentDate = new DateTime(2026, 1, 5),
            Continent = setup.Continent,
            Leagues = new List<LeagueData> { league },
            News = new List<NewsData>
            {
                new()
                {
                    Date = new DateTime(2026, 1, 5),
                    Category = "Clube",
                    Headline = $"{selectedClub.Name} apresenta {setup.ManagerName}",
                    Summary = "O novo treinador assume o projeto esportivo com apoio da diretoria e expectativa da torcida."
                },
                new()
                {
                    Date = new DateTime(2026, 1, 5),
                    Category = "Temporada",
                    Headline = "Clubes iniciam preparação para a nova temporada",
                    Summary = "Elencos se apresentam, o mercado começa a se movimentar e os primeiros amistosos são agendados."
                }
            }
        };

        return new CareerSave
        {
            ManagerName = setup.ManagerName,
            Nationality = setup.Nationality,
            ManagerBackground = setup.Background,
            SelectedClubId = selectedClub.Id,
            LastSavedAt = DateTime.Now,
            DaysPlayed = 0,
            Universe = universe
        };
    }

    private static ClubData CreateClub(string name, Random random)
    {
        var club = new ClubData
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = name,
            ShortName = CreateShortName(name),
            Stadium = $"Arena {name.Split(' ')[0]}",
            Balance = random.NextInt64(8_000_000, 80_000_000),
            Reputation = random.Next(58, 86)
        };

        for (var index = 0; index < 18; index++)
        {
            club.Players.Add(new PlayerData
            {
                Name = $"{FirstNames[random.Next(FirstNames.Length)]} {LastNames[random.Next(LastNames.Length)]}",
                Position = Positions[index],
                Age = random.Next(18, 34),
                Strength = random.Next(58, 84),
                Potential = random.Next(66, 92),
                Morale = random.Next(68, 91)
            });
        }

        return club;
    }

    private static string CreateShortName(string name)
    {
        var letters = name
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(part => part.Length > 2)
            .Take(3)
            .Select(part => char.ToUpperInvariant(part[0]))
            .ToArray();

        return letters.Length >= 2
            ? new string(letters)
            : name[..Math.Min(3, name.Length)].ToUpperInvariant();
    }
}
