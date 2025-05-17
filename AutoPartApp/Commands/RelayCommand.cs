using System.Windows.Input;

namespace AutoPartApp;

/// <summary>
/// A reusable implementation of the <see cref="ICommand"/> interface.
/// Allows binding parameterless actions and conditions to UI elements in WPF.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// The command does not use the parameter passed to <see cref="Execute(object?)"/> or <see cref="CanExecute(object?)"/>.
    /// </summary>
    /// <param name="execute">The action to execute when the command is invoked.</param>
    /// <param name="canExecute">A function that determines whether the command can execute. Optional.</param>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Determines whether the command can execute in its current state.
    /// This implementation ignores the <paramref name="parameter"/> argument.
    /// </summary>
    /// <param name="parameter">Command parameter (ignored).</param>
    /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">Command parameter (not used in this implementation).</param>
    public void Execute(object? parameter) => _execute();

    /// <summary>
    /// Occurs when changes in the command's ability to execute should be reevaluated.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

