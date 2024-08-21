using System.IO;
using System.Text.Json;
using HammerTime.Helpers;
using HammerTime.Models;

namespace HammerTime.Services.Data;

internal sealed class ProjectDataService
{
    public event Action<Project>? OnProjectAdded;

    private readonly string filePath;
    private readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

    public ProjectDataService(string filePath)
    {
        this.filePath = filePath;

        if (!Directory.Exists(FileHelpers.GetRootDirectoryPath()))
            Directory.CreateDirectory(FileHelpers.GetRootDirectoryPath());
    }

    public void AddProject(Project project)
    {
        OnProjectAdded?.Invoke(project);
    }

    public List<Project> LoadProjects()
    {
        if (!File.Exists(filePath))
            return [];

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Project>>(json)!;
    }

    public void SaveProjects(List<Project> projects)
    {
        string json = JsonSerializer.Serialize(projects, serializerOptions);
        using StreamWriter sw = new(filePath, false);
        sw.Write(json);
    }
}