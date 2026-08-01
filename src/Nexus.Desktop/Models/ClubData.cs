using System.Collections.Generic;

namespace Nexus.Desktop.Models;

public sealed class ClubData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public long Balance { get; set; }
    public int Reputation { get; set; }
    public List<PlayerData> Players { get; set; } = new();
}
