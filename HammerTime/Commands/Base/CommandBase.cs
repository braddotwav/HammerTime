using System.Windows.Input;

namespace HammerTime.Commands.Base;

internal abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter) { return true; }

    public abstract void Execute(object? parameter);

    protected void OnCanExecuteChanged() 
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
