using System;

namespace EncodingLibrary.Commands
{
    public class RelayCommand : CommandBase
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null ? base.CanExecute(parameter) : _canExecute.Invoke(parameter);
        }

        public override void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
