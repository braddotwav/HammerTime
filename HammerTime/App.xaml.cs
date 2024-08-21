using CommandLine;
using System.Windows;

namespace HammerTime;

public partial class App : Application
{
    public static Options Arguments { get; private set; } = new Options();

    public class Options
    {
        [Option("hammer", Required = false)]
        public string HammerPath { get; set; } = string.Empty;

        [Option("launchonstart", Required = false)]
        public bool LaunchOnStart { get; set; }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Parser.Default.ParseArguments<Options>(e.Args).WithParsed((x) =>
        {
            Arguments = x;
        });
    }
}
