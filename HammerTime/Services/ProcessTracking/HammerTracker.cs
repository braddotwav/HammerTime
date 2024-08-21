using HammerTime.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HammerTime.Services.ProcessTracking;

internal sealed class HammerTracker : IProcessTrackerService
{
    #region DLL Imports
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    #endregion

    public bool IsRunning => currentProcess != null;
    public string[] ProcessNames => processNames;

    public event Action? OnProcessExited;
    public event Action? OnProcessStarted;
    public event Action? OnProcessLostFocus;
    public event Action<ProcessInfomation>? OnProcessChanged;

    private readonly string[] processNames = ["hammer", "hammerplusplus"];
    private Process? currentProcess;
    private bool recentlyFocused;

    public void RunProcessCheck()
    {
        Process activeProcess = GetActiveProcessInfomation();

        if (ProcessNameIsTrackable(activeProcess.ProcessName))
        {
            if (currentProcess == null)
            {
                ProcessStarted(activeProcess);
            }

            if (ProcessHasUpdated(activeProcess))
            {
                ProcessUpdated(activeProcess);
            }

            recentlyFocused = true;
        }
        else if (currentProcess != null)
        {
            if (currentProcess.HasExited)
            {
                ProcessExited();
            }
            else if (recentlyFocused)
            {
                ProcessLostFocus();
            }
        }
    }

    private bool ProcessHasUpdated(Process activeProcess)
    {
        return (!ProcessWindowTitleIsEqual(activeProcess.MainWindowTitle) || !recentlyFocused) && !string.IsNullOrEmpty(activeProcess.MainWindowTitle);
    }

    private void ProcessLostFocus()
    {
        recentlyFocused = false;
        OnProcessLostFocus?.Invoke();
    }

    private void ProcessUpdated(Process activeProcess)
    {
        currentProcess = activeProcess;
        OnProcessChanged?.Invoke(new ProcessInfomation(activeProcess.ProcessName, activeProcess.MainWindowTitle, recentlyFocused));
    }

    private void ProcessStarted(Process activeProcess)
    {
        currentProcess = activeProcess;
        OnProcessStarted?.Invoke();
    }

    private void ProcessExited()
    {
        currentProcess = null;
        OnProcessExited?.Invoke();
    }

    private static Process GetActiveProcessInfomation()
    {
        _ = GetWindowThreadProcessId(GetForegroundWindow(), out var processId);

        return Process.GetProcessById(processId);
    }

    private bool ProcessNameIsTrackable(string name)
    {
        return processNames.Contains(name);
    }

    private bool ProcessWindowTitleIsEqual(string windowTitle)
    {
        return currentProcess?.MainWindowTitle == windowTitle;
    }
}