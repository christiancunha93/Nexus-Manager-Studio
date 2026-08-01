using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Nexus.Desktop.Models;

namespace Nexus.Desktop.Services;

public sealed class SaveGameService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public string SavesDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "NexusManagerStudio",
        "Saves");

    public string Save(CareerSave career)
    {
        ArgumentNullException.ThrowIfNull(career);

        Directory.CreateDirectory(SavesDirectory);
        career.LastSavedAt = DateTime.Now;

        var path = Path.Combine(SavesDirectory, $"{career.SaveId}.nexus.json");
        File.WriteAllText(path, JsonSerializer.Serialize(career, JsonOptions));
        return path;
    }

    public CareerSave? Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        return JsonSerializer.Deserialize<CareerSave>(
            File.ReadAllText(path),
            JsonOptions);
    }

    public IReadOnlyList<SaveSlotInfo> GetSaveSlots()
    {
        Directory.CreateDirectory(SavesDirectory);

        return Directory
            .GetFiles(SavesDirectory, "*.nexus.json")
            .Select(path =>
            {
                try
                {
                    var career = Load(path);
                    if (career is null)
                    {
                        return null;
                    }

                    var club = FindSelectedClub(career);

                    return new SaveSlotInfo
                    {
                        FilePath = path,
                        Career = career,
                        ClubName = club?.Name ?? "Clube desconhecido",
                        CurrentDate = career.Universe.CurrentDate
                    };
                }
                catch
                {
                    return null;
                }
            })
            .Where(slot => slot is not null)
            .Cast<SaveSlotInfo>()
            .OrderByDescending(slot => slot.Career.LastSavedAt)
            .ToList();
    }

    public CareerSave? LoadLatest()
    {
        var latest = GetSaveSlots().FirstOrDefault();
        return latest is null ? null : Load(latest.FilePath);
    }

    public void Delete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static ClubData? FindSelectedClub(CareerSave career)
    {
        return career.Universe.Leagues
            .SelectMany(league => league.Clubs)
            .FirstOrDefault(club => club.Id == career.SelectedClubId);
    }
}

public sealed class SaveSlotInfo
{
    public string FilePath { get; set; } = string.Empty;
    public CareerSave Career { get; set; } = new();
    public string ClubName { get; set; } = string.Empty;
    public DateTime CurrentDate { get; set; }

    public string ManagerName => Career.ManagerName;
    public string DateText => CurrentDate.ToString("dd/MM/yyyy");
    public string LastSavedText => Career.LastSavedAt.ToString("dd/MM/yyyy HH:mm");
    public string Summary => $"{ClubName} • {DateText}";
}
