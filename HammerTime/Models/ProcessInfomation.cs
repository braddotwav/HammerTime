namespace HammerTime.Models
{
    internal class ProcessInfomation (string name, string title)
    {
        public string Name { get; } = name;
        public string WindowTitle { get; } = title;
    }
}
