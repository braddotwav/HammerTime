using System.Windows;

namespace HammerTime.Views
{
    internal class SessionWindowFactory : WindowFactory
    {
        public override Window CreateNewWindowInstance()
        {
            return new SessionWindow();
        }
    }

    public partial class SessionWindow : Window
    {
        public SessionWindow()
        {
            InitializeComponent();
        }
    }
}
