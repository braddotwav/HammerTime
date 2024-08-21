namespace HammerTime.Commands.Base;

internal class RelayCommand : CommandBase
{
    private readonly Action<object> execute;
    private readonly Func<object, bool> canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null!)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        if (canExecute == null)
        {
            return true;
        }
        
        return canExecute(parameter!);
    }

    public override void Execute(object? parameter)
    {
        execute(parameter!);
    }
}
