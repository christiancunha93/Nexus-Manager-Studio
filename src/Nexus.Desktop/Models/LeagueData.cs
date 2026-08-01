using System.Collections.Generic;

namespace Nexus.Desktop.Models;

public sealed class LeagueData
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public List<ClubData> Clubs { get; set; } = new();
}
