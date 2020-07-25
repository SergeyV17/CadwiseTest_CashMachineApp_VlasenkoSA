using System;
using System.Windows.Input;

namespace CashMachineApp.Commands
{
    /// <summary>
    /// Класс реализации команд
    /// </summary>
    class RelayCommand : ICommand
    {
        private Action<object> execute;

        private Func<object, bool> canExecute;

        // Событие вызывается при изменении условий, указывающих может ли команда выполняться
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">событие выполнения команды</param>
        /// <param name="canExecute">событие проверки может ли команда выполняться</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Метож проверяющий возможность выполнения команды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>булевое значение</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Метод выполняющий команду
        /// </summary>
        /// <param name="parameter">параметр команды</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
