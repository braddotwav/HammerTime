using HammerTime.Commands.Base;
using System.Windows.Controls;

namespace HammerTime.Commands;

internal sealed class DeselectDataGridRowCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is DataGrid grid && grid.SelectedItems != null)
        {
            grid.UnselectAll();
        }
    }
}
