using System.Diagnostics;
using HammerTime.Helpers;
using HammerTime.Services;
using System.Windows.Threading;
using HammerTime.ViewModels.Base;

namespace HammerTime.Models
{
    internal class TimerModel : ViewModelBase
    {
        public enum StatusType
        {
            Inactive,
            Idle,
            Active
        }

        private StatusType status;

        public StatusType Status
        {
            get { return status; }
            private set 
            {
                status = value; 
                OnPropertyChanged(nameof(Status));
            }
        }

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime
        {
            get { return elapsedTime; }
            private set 
            { 
                elapsedTime = value; 
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }

        private readonly IProcessTracker process;
        private readonly IData data;
        private readonly Stopwatch stopwatch = new();
        private readonly Project currentProject = new();
        private readonly DispatcherTimer dispatchTimer = new(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1) };

        public TimerModel(IProcessTracker process, IData data)
        {
            this.process = process;
            this.data = data;

            dispatchTimer.Tick += OnDispatchTimerTick;

            process.OnProcessChanged += OnProcessFocusChanged;
            process.OnProcessLostFocus += OnProcessLostFocus;
            process.OnProcessStopped += OnProcessStopped;
        }

        public void Start()
        {
            dispatchTimer.Start();
        }

        private void OnDispatchTimerTick(object? sender, EventArgs e)
        {
            process.Ping();

            if (status == StatusType.Active)
            {
                ElapsedTime = stopwatch.Elapsed;
            }
        }

        private void OnProcessFocusChanged(ProcessInfomation processInfo)
        {
            if (StringHelpers.TryGetVMFFileName(processInfo.WindowTitle, out var fileName))
            {
                if (!currentProject.Equals(fileName))
                {
                    OnProcessStopped();
                }

                if (process.RecentlyFocused && !currentProject.IsEmpty)
                {
                    return;
                }

                if (currentProject.IsEmpty)
                {
                    currentProject.Update(fileName);
                }

                SetStatus(StatusType.Active);
            }
            else
            {
                if (!currentProject.IsEmpty)
                {
                    OnProcessStopped();
                }
            }
        }

        private void OnProcessStopped()
        {
            if (currentProject.IsEmpty) { return; }

            data.Save(new ProjectData
            {
                Name = currentProject.Name,
                TimeSpent = elapsedTime,
                LastOpenDate = DateTime.Now,
            });

            SetStatus(StatusType.Inactive);

            currentProject.Empty();
        }

        private void OnProcessLostFocus()
        {
            if (currentProject.IsEmpty) { return; }
            SetStatus(StatusType.Idle);
        }

        private void SetStatus(StatusType statusType)
        {
            if (status != statusType)
                Status = statusType;

            switch (statusType)
            {
                case StatusType.Inactive:
                    stopwatch.Reset();
                    break;
                case StatusType.Idle:
                    stopwatch.Stop();
                    break;
                case StatusType.Active:
                    stopwatch.Start();
                    break;
            }

            ElapsedTime = stopwatch.Elapsed;
        }

        public override void Dispose()
        {
            base.Dispose();

            process.OnProcessChanged -= OnProcessFocusChanged;
            process.OnProcessLostFocus -= OnProcessLostFocus;
            process.OnProcessStopped -= OnProcessStopped;
        }
    }
}
