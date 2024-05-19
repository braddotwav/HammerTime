using HammerTime.ViewModels;
using System.Windows;

namespace HammerTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            mainViewModel = new MainWindowViewModel();
            DataContext = mainViewModel;

            //Subscribe to the Loaded event
            Loaded += OnMainWindowLoaded;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            //Set the window to open in the bottom right corner with some margin
            Left = SystemParameters.WorkArea.Right - Width - 15f;
            Top = SystemParameters.WorkArea.Bottom - Height - 15f;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //Call the MainViewModels dispose method
            mainViewModel.Dispose();

            //Unsubscribe from the Loaded event
            Loaded -= OnMainWindowLoaded;
        }
    }
}