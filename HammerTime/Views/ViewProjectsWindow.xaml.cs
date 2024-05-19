using System.Windows;

namespace HammerTime.Views
{
    internal class ViewProjectWindowFactory : WindowFactory
    {
        public override Window CreateNewWindowInstance()
        {
            return new ViewProjectsWindow();
        }
    }

    public partial class ViewProjectsWindow : Window
    {
        public ViewProjectsWindow()
        {
            InitializeComponent();
        }
    }
}
