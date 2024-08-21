namespace HammerTime.Helpers;

public static class StringHelpers
{
    public static string GetVMFFileName(string windowTitle)
    {
        int startIndex = windowTitle.LastIndexOf('\\');
        int endIndex = windowTitle.LastIndexOf('-');

        if (startIndex != -1 && endIndex != -1)
        {
            return windowTitle.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
        }

        return string.Empty;
    }

    public static bool TryGetVMFFileName(string windowTitle, out string fileName)
    {
        if (windowTitle.Contains(".vmf"))
        {
            fileName = GetVMFFileName(windowTitle);

            if (!string.IsNullOrEmpty(fileName))
                return true;
        }

        fileName = string.Empty;
        return false;
    }

    public static bool IsProcessNameExecutableInDirectory(string directory, string[] processNames)
    {
        foreach (var process in processNames)
        {
            if (directory.EndsWith(string.Join(".", process, "exe")))
                return true;
        }

        return false;
    }
}
