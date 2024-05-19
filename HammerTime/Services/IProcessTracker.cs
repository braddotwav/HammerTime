using HammerTime.Models;

namespace HammerTime.Services
{
    internal interface IProcessTracker
    {
        public bool RecentlyFocused { get; }
        public event Action<ProcessInfomation> OnProcessChanged;
        public event Action OnProcessStarted;
        public event Action OnProcessLostFocus;
        public event Action OnProcessStopped;
        public void Ping();
    }
}
