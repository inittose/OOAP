using System.Windows.Input;

namespace View.ViewModel
{
    /// <summary>
    /// Реализует интерфейс <see cref="ICommand"/>.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Возвращает и задает делегат, который хранит логику обработки команды.
        /// </summary>
        private Action<object> ProcessCommand { get; set; }

        /// <summary>
        /// Событие, которое происходит при изменении доступности запуска команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add 
            { 
                CommandManager.RequerySuggested += value; 
            }

            remove 
            { 
                CommandManager.RequerySuggested -= value; 
            }
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="processCommand">Делегат, который хранит логику обработки команды.</param>
        public RelayCommand(Action<object> processCommand)
        {
            ProcessCommand = processCommand;
        }

        /// <summary>
        /// Определяет возможность выполнения команды.
        /// </summary>
        /// <param name="parameter">Экзепляр класса <see cref="object"/>.</param>
        /// <returns>Возвращает <see cref="true"/>.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Выполняет логику команды <see cref="ProcessCommand"/>.
        /// </summary>
        /// <param name="parameter">Экзепляр класса <see cref="object"/>.</param>
        public void Execute(object parameter)
        {
            ProcessCommand(parameter);
        }
    }
}
