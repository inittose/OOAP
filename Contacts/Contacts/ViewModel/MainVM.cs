using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Управляет логикой работы программы.
    /// </summary>
    public class MainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Контакт.
        /// </summary>
        public Contact _contact;

        /// <summary>
        /// Возвращает и задает контакт.
        /// </summary>
        public Contact Contact
        {
            get => _contact;
            set
            {
                if (_contact != value)
                {
                    _contact = value;
                    OnPropertyChanged(nameof(Contact));

                    foreach(var prop in typeof(Contact).GetProperties())
                    {
                        OnPropertyChanged(prop.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает команду загрузки данных.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Возвращает команду сохранения данных.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Событие, которое происходит при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Создает экзепляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            _contact = new Contact();
            LoadCommand = new RelayCommand(LoadContact);
            SaveCommand = new RelayCommand(SaveContact);
        }

        /// <summary>
        /// Оповещает об изменении свойства.
        /// </summary>
        /// <param name="prop">Имя свойства.</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// Загружает данные о контакте.
        /// </summary>
        /// <param name="obj">Экзепляр класса <see cref="object"/>.</param>
        private void LoadContact(object obj)
        {
            Contact = ContactSerializer.GetContact();
        }

        /// <summary>
        /// Сохраняет данные о контакте.
        /// </summary>
        /// <param name="obj">Экзепляр класса <see cref="object"/>.</param>
        private void SaveContact(object obj)
        {
            ContactSerializer.SetContact(Contact);
        }
    }
}
