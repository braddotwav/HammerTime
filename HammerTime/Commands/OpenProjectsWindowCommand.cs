using HammerTime.Commands.Base;
using HammerTime.ViewModels;

namespace HammerTime.Commands
{
    internal class OpenProjectsWindowCommand : CommandBase
    {
        private readonly ViewProjectViewModel projectViewModel;

        public OpenProjectsWindowCommand(ViewProjectViewModel projectViewModel)
        {
            this.projectViewModel = projectViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return !projectViewModel.Window.IsOpen;
        }

        public override void Execute(object? parameter)
        {
            projectViewModel.Window.Open(projectViewModel);
        }
    }
}
