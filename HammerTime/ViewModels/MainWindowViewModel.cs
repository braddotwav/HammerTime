using HammerTime.Models;
using HammerTime.Commands;
using System.Windows.Input;
using HammerTime.Services;
using HammerTime.Helpers;
using HammerTime.ViewModels.Base;
using HammerTime.Views;

namespace HammerTime.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Models
        public TimerModel TimerModel { get; }
        public LauncherModel? LauncherModel { get; }
        #endregion

        #region Services
        private readonly HammerData dataService = new(Constants.HAMMERTIME_DATA_FILE_NAME);
        private readonly ProcessTracker processTracker = new(Constants.HAMMER_PROCESSES);
        #endregion

        #region Commands
        public ICommand LaunchHammerCommand { get; }
        public ICommand OpenProjectsWindowCommand { get; }
        #endregion

        #region View Models
        private ViewProjectViewModel ViewprojectViewModel { get; }
        #endregion

        public MainWindowViewModel()
        {
            ViewprojectViewModel = new(new ViewProjectWindowFactory(), dataService);
            OpenProjectsWindowCommand = new OpenProjectsWindowCommand(ViewprojectViewModel);

            LauncherModel = new(processTracker, App.Arguments.Launch);
            LaunchHammerCommand = new LaunchHammerCommand(LauncherModel);

            TimerModel = new(processTracker, dataService);
            
            TimerModel.Start();
        }

        public override void Dispose()
        {
            base.Dispose();

            ViewprojectViewModel.Dispose();
            TimerModel.Dispose();
            LauncherModel?.Dispose();
        }
    }
}
