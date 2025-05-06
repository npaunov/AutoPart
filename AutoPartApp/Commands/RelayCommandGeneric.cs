using System.Windows.Input;

namespace AutoPartApp;

/// <summary>
/// A generic implementation of the ICommand interface.
/// Enables binding actions and conditions to UI elements with parameterized input.
/// </summary>
public class RelayCommandGeneric<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool>? _canExecute;

    /// <summary>
    /// Initializes a new instance of the RelayCommandGeneric class with parameterized input.
    /// </summary>
    /// <param name="execute">The action to execute when the command is invoked, accepting a parameter of type T.</param>
    /// <param name="canExecute">A function that determines whether the command can execute, based on a parameter of type T. Optional.</param>
    public RelayCommandGeneric(Action<T> execute, Func<T, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Determines whether the command can execute in its current state, based on the provided parameter.
    /// </summary>
    /// <param name="parameter">The command parameter of type T.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    public bool CanExecute(object? parameter) => _canExecute?.Invoke((T)parameter!) ?? true;

    /// <summary>
    /// Executes the command with the provided parameter.
    /// </summary>
    /// <param name="parameter">The command parameter of type T.</param>
    public void Execute(object? parameter) => _execute((T)parameter!);

    /// <summary>
    /// Occurs when changes in the command's ability to execute should be reevaluated.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
