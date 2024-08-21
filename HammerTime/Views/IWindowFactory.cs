namespace HammerTime.Views;

public interface IWindowFactory
{
    public event Action OnWindowOpened;
    public event Action OnWindowClosed;
    public bool IsOpen { get; }
    public void Open(object dataContext);
}
