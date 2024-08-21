using System.IO;
using HammerTime.Views;
using HammerTime.Models;
using HammerTime.Helpers;
using System.Diagnostics;
using System.Windows.Input;
using HammerTime.Services.Data;
using HammerTime.Commands.Base;
using System.Windows.Threading;
using HammerTime.ViewModels.Base;
using HammerTime.DataTypes.Enums;
using HammerTime.Services.ProcessTracking;

namespace HammerTime.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private Status status;

    public Status Status
    {
        get { return status; }
        private set
        {
            status = value;
            UpdateStopwatchState(status);
            ElapsedTime = stopwatch.Elapsed;
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(CanLaunch));
        }
    }

    public bool CanLaunch => CanLaunchHammerExecute();

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

    #region Commands
    public ICommand LaunchHammerCommand => new RelayCommand(LaunchHammer, x => CanLaunchHammerExecute());
    public ICommand OpenSessionWindowCommand => new RelayCommand(OpenSessionWindow, x => CanOpenSessionWindowExecute());
    #endregion

    #region Services
    private readonly HammerTracker processTracker = new();
    private readonly ProjectDataService projectDataService = new(FileHelpers.GetFilePathFromRootDirectory("sessions.json"));
    #endregion

    private readonly Stopwatch stopwatch = new();
    private readonly Project currentProject = new();
    private readonly SessionViewModel sessionViewModel;
    private readonly DispatcherTimer dispatchTimer = new(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1) };

    public MainWindowViewModel()
    {
        sessionViewModel = new(new SessionWindowFactory(), projectDataService);
        dispatchTimer.Tick += OnDispatchTimerTick;
        processTracker.OnProcessStarted += OnProcessStarted;
        processTracker.OnProcessChanged += OnProcessChanged;
        processTracker.OnProcessExited += OnProcessExited;
        processTracker.OnProcessLostFocus += OnProcessLostFocus;
        dispatchTimer.Start();

        if (LaunchHammerCommand.CanExecute(null))
        {
            LaunchHammer(null);
        }
    }

    private bool CanLaunchHammerExecute()
    {
        if (!Directory.Exists(Path.GetDirectoryName(App.Arguments.HammerPath)) || string.IsNullOrEmpty(App.Arguments.HammerPath))
            return false;

        return !processTracker.IsRunning && StringHelpers.IsProcessNameExecutableInDirectory(App.Arguments.HammerPath, processTracker.ProcessNames);
    }

    private bool CanOpenSessionWindowExecute()
    {
        return !sessionViewModel.Window.IsOpen;
    }

    private void OnProcessStarted()
    {
        OnPropertyChanged(nameof(CanLaunch));
    }

    private void OnProcessChanged(ProcessInfomation process)
    {
        if (StringHelpers.TryGetVMFFileName(process.WindowTitle, out string fileName))
        {
            if (currentProject.IsEmpty)
            {
                currentProject.Name = fileName;
            }

            if (!currentProject.Name.Equals(fileName))
            {
                OnProcessExited();
            }

            Status = Status.ACTIVE;
        }
        else if (!currentProject.IsEmpty)
        {
            OnProcessExited();
        }
    }

    private void OnProcessLostFocus()
    {
        if (currentProject.IsEmpty) return;

        Status = Status.IDLE;
    }

    private void OnProcessExited()
    {
        OnPropertyChanged(nameof(CanLaunch));

        if (currentProject.IsEmpty) return;

        projectDataService.AddProject(new Project()
        {
            Name = currentProject.Name,
            TimeSpent = elapsedTime,
            LastOpenDate = DateTime.Now,
        });

        Status = Status.INACTIVE;
        currentProject.Empty();
    }

    private void OnDispatchTimerTick(object? sender, EventArgs e)
    {
        processTracker.RunProcessCheck();

        if (status == Status.ACTIVE)
        {
            ElapsedTime = stopwatch.Elapsed;
        }
    }

    private void LaunchHammer(object? parameter)
    {
        Process.Start(App.Arguments.HammerPath);
    }

    private void OpenSessionWindow(object? parameter)
    {
        sessionViewModel.Window.Open(sessionViewModel);
    }

    private void UpdateStopwatchState(Status status) 
    {
        switch (status)
        {
            case Status.INACTIVE:
                stopwatch.Reset();
                break;
            case Status.IDLE:
                stopwatch.Stop();
                break;
            case Status.ACTIVE:
                stopwatch.Start();
                break;
        }
    }

    public override void Dispose()
    {
        sessionViewModel.Dispose();
        dispatchTimer.Tick -= OnDispatchTimerTick;
        processTracker.OnProcessChanged -= OnProcessChanged;
        processTracker.OnProcessExited -= OnProcessExited;
        processTracker.OnProcessLostFocus -= OnProcessLostFocus;
    }
}