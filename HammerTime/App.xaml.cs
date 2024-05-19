using CommandLine;
using System.Windows;
using HammerTime.Helpers;
using System.Diagnostics;

namespace HammerTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Options Arguments { get; private set; } = new Options();

        public class Options
        {
            [Option("launch", Required = false)]
            public string Launch { get; set; } = string.Empty;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Parser.Default.ParseArguments<Options>(e.Args)
                .WithParsed((x) =>
                {
                    Arguments = x;

                    //I should likly move this but this is what currently opens hammer on start up
                    if (StringHelpers.DirectoryIsHammer(x.Launch))
                    {
                        Process.Start(x.Launch);
                    }
                });
        }
    }
}
