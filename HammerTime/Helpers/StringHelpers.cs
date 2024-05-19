using System.IO;

namespace HammerTime.Helpers
{
    internal static class StringHelpers
    {
        public static string GetVMFFileName(string windowTitle)
        {
            int startIndex = windowTitle.LastIndexOf('\\');
            int endIndex = windowTitle.LastIndexOf('-');

            if (startIndex != -1 && endIndex != -1)
            {
                string fileName = windowTitle.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                return fileName;
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

        public static bool DirectoryIsHammer(string directory)
        {
            if (!Directory.Exists(Path.GetDirectoryName(directory)) || string.IsNullOrEmpty(directory))
                return false;

            foreach (var process in Constants.HAMMER_PROCESSES)
            {
                if (directory.EndsWith(string.Join(".", process, "exe")))
                    return true;
            }

            return false;
        }
    }
}
