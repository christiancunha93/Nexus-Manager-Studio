using System;

namespace Nexus.Desktop.Models;

public sealed class NewsData
{
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}
