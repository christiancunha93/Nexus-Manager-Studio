using System;
using System.Collections.Generic;

namespace Nexus.Desktop.Models;

public sealed class UniverseData
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public int Seed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime CurrentDate { get; set; }
    public string Continent { get; set; } = string.Empty;
    public List<LeagueData> Leagues { get; set; } = new();
    public List<NewsData> News { get; set; } = new();
}
