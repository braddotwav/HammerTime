using System.Windows;

namespace HammerTime.Views
{
    internal abstract class WindowFactory : IWindowFactory
    {
        public Window? Window { get; private set; }

        public event Action? OnWindowOpened;
        public event Action? OnWindowClosed;

        public bool IsOpen { get; private set; }
        public abstract Window CreateNewWindowInstance();

        public void Open(object dataContext)
        {
            Window = CreateNewWindowInstance();
            Window.DataContext = dataContext;

            Window.Closed += WindowClosed;

            IsOpen = true;
            OnWindowOpened?.Invoke();
            Window.Show();
        }

        private void WindowClosed(object? sender, EventArgs e)
        {
            IsOpen = false;
            OnWindowClosed?.Invoke();
            Window.Closed -= WindowClosed;
        }
    }
}
