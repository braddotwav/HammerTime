using System.Text.Json.Serialization;

namespace HammerTime.Models;

internal sealed class Project
{
    public string Name { get; set; } = string.Empty;
    public TimeSpan TimeSpent { get; set; }
    public DateTime LastOpenDate { get; set; }

    [JsonIgnore]
    public bool IsEmpty => string.IsNullOrEmpty(Name);

    public void Empty() => Name = string.Empty;

    public void Update(Project project) 
    {
        TimeSpent += project.TimeSpent;
        LastOpenDate = project.LastOpenDate;
    }
}
