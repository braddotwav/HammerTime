using HammerTime.Helpers;
using HammerTime.Services;
using HammerTime.ViewModels.Base;
using System.Diagnostics;

namespace HammerTime.Models
{
    internal class LauncherModel : ViewModelBase
    {
        public bool CanLaunch
        {
            get { return canLaunch; }
            private set
            {
                canLaunch = value;
                OnPropertyChanged(nameof(CanLaunch));
            }
        }

        private bool canLaunch = false;
		private readonly IProcessTracker process;
        private readonly string launchPath = string.Empty;
        
        public LauncherModel(IProcessTracker process, string launchPath)
        {
			this.process = process;
            this.launchPath = launchPath;

            if (!StringHelpers.DirectoryIsHammer(launchPath)) { return; }

            process.OnProcessStarted += OnProcessStarted;
            process.OnProcessStopped += OnProcessStopped;
        }

        public void Launch() => Process.Start(launchPath);
        private void OnProcessStopped() => CanLaunch = true;
        private void OnProcessStarted() => CanLaunch = false;
        public override void Dispose()
        {
            base.Dispose();
            
            process.OnProcessStarted -= OnProcessStarted;
            process.OnProcessStopped -= OnProcessStopped;
        }
    }
}
