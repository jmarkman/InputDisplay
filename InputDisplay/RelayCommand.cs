using System;
using System.Windows.Input;

namespace InputDisplay
{
    public class RelayCommand : ICommand
    {
        private readonly Action executeMethod;
        private readonly Func<bool> canExecuteMethod;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action exMethod)
        {
            executeMethod = exMethod;
        }

        public RelayCommand(Action exMethod, Func<bool> canExMethod)
        {
            executeMethod = exMethod;
            canExecuteMethod = canExMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            if (canExecuteMethod is not null)
            {
                return canExecuteMethod();
            }

            if (executeMethod is not null)
            {
                return true;
            }

            return false;
        }

        public void Execute(object? parameter)
        {
            if (executeMethod is not null)
            {
                executeMethod();
            }
        }
    }
}
