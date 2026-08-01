using System;

namespace Nexus.Desktop.Models;

public sealed class CareerSave
{
    public string SaveId { get; set; } = Guid.NewGuid().ToString("N");
    public string ManagerName { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string ManagerBackground { get; set; } = string.Empty;
    public string SelectedClubId { get; set; } = string.Empty;
    public DateTime LastSavedAt { get; set; }
    public int DaysPlayed { get; set; }
    public UniverseData Universe { get; set; } = new();
}
