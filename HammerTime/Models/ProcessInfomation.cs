namespace HammerTime.Models;

internal sealed class ProcessInfomation
{
    public string Name { get; }
    public string WindowTitle { get; }
    public bool RecentlyFocused { get; }

    public ProcessInfomation(string name, string title, bool recentlyFocused)
    {
        Name = name;
        WindowTitle = title;
        RecentlyFocused = recentlyFocused;
    }
}
