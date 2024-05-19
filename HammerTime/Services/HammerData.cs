using System.IO;
using System.Text.Json;
using HammerTime.Models;

namespace HammerTime.Services
{
    internal class HammerData(string filePath) : IData
    {
        public event Action? OnSaveData;
        private readonly string filePath = GetFullDataPath(filePath);
        private readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

        public List<ProjectData> GetData()
        {
            if (!Directory.Exists(GetDirectoryPath()))
            {
                Directory.CreateDirectory(GetDirectoryPath());
            }

            if (!File.Exists(filePath))
            {
                return [];
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<ProjectData>>(json)!;
        }

        public void Save(ProjectData data)
        {
            List<ProjectData> existingData = GetData();

            // Find existing project data, if any
            ProjectData existingProject = existingData.FirstOrDefault(p => p.Name == data.Name)!;

            if (existingProject != null)
            {
                // Update existing project data
                existingProject.TimeSpent += data.TimeSpent;
                existingProject.LastOpenDate = data.LastOpenDate;
            }
            else
            {
                // Add new project data
                existingData.Add(data);
            }

            WriteDataToJson(existingData);
            OnSaveData?.Invoke();
        }

        public void Update(List<ProjectData> data)
        {
            WriteDataToJson(data);
        }

        private void WriteDataToJson(object data)
        {
            string json = JsonSerializer.Serialize(data, serializerOptions);
            using StreamWriter sw = new(filePath, false);
            sw.Write(json);
        }

        private static string GetFullDataPath(string fileName)
        {
            return string.Join('/', GetDirectoryPath(), fileName);
        }

        private static string GetDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"HammerTime");
        }
    }
}
