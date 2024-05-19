using HammerTime.Models;

namespace HammerTime.Services
{
    internal interface IData
    {
        public event Action OnSaveData;
        public void Save(ProjectData data);
        public void Update(List<ProjectData> data);
        public List<ProjectData> GetData();
    }
}
