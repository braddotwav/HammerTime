using HammerTime.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HammerTime.Services
{
    internal class ProcessTracker(string[] processNames) : IProcessTracker
    {
        #region DLL Imports
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        #endregion

        public bool RecentlyFocused => recentlyFocused;

        public event Action<ProcessInfomation>? OnProcessChanged;
        public event Action? OnProcessStarted;
        public event Action? OnProcessLostFocus;
        public event Action? OnProcessStopped;

        private readonly string[] processNames = processNames;
        private Process? currentProcess;
        private bool recentlyFocused;
        private bool isOpen;

        public void Ping()
        {
            Process process = GetActiveProcessInfomation();

            if (processNames.Contains(process.ProcessName))
            {
                if (!isOpen)
                {
                    OnProcessStarted?.Invoke();
                }

                if (currentProcess == null || currentProcess.MainWindowTitle != process.MainWindowTitle || !recentlyFocused)
                {
                    if (string.IsNullOrEmpty(process.MainWindowTitle))
                        return;

                    currentProcess = process;
                    OnProcessChanged?.Invoke(new ProcessInfomation(process.ProcessName, process.MainWindowTitle));
                }

                recentlyFocused = true;
                isOpen = true;
            }
            else
            {
                if (currentProcess != null && currentProcess.HasExited)
                {
                    OnProcessStopped?.Invoke();
                    isOpen = false;
                    currentProcess = null;
                    return;
                }

                if (currentProcess != null && recentlyFocused)
                {
                    OnProcessLostFocus?.Invoke();
                    recentlyFocused = false;
                }
            }
        }

        private Process GetActiveProcessInfomation()
        {
            _ = GetWindowThreadProcessId(GetForegroundWindow(), out var processId);

            return Process.GetProcessById(processId);
        }
    }
}
