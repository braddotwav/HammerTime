using HammerTime.Models;

namespace HammerTime.Services.ProcessTracking;

internal interface IProcessTrackerService
{
    public bool IsRunning { get; }
    public string[] ProcessNames { get; }

    public event Action<ProcessInfomation> OnProcessChanged;
    public event Action OnProcessStarted;
    public event Action OnProcessLostFocus;
    public event Action OnProcessExited;
    
    public void RunProcessCheck();
}