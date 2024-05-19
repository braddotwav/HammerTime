using HammerTime.Models;
using HammerTime.Commands.Base;

namespace HammerTime.Commands
{
    internal class LaunchHammerCommand : CommandBase
    {
        private readonly LauncherModel launcherModel;

        public LaunchHammerCommand(LauncherModel launcherModel)
        {
            this.launcherModel = launcherModel;
        }

        public override void Execute(object? parameter) => launcherModel.Launch();
    }
}
