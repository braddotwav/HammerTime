using System.IO;

namespace HammerTime.Helpers;

internal static class FileHelpers
{
    public static string GetFilePathFromRootDirectory(string file)
    {
        return string.Join('/', GetRootDirectoryPath(), file);
    }

    public static string GetRootDirectoryPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HammerTime");
    }
}