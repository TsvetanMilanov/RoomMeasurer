namespace RoomMeasurer.Client.Utilities
{
    using System;
    using System.Windows.Input;

    public class DelegateCommandWithParameter<T> : ICommand
    {
        private readonly Func<T, bool> canExecute;
        private readonly Action<T> execute;

        public DelegateCommandWithParameter(Action<T> execute)
                       : this(execute, null)
        {
        }

        public DelegateCommandWithParameter(Action<T> execute,
                       Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            return canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}