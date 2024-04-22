using CommunityToolkit.Mvvm.Input;
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
        /// Возвращает и задает контакт.
        /// </summary>
        public Contact Contact { get; private set; } = new Contact();

        /// <summary>
        /// Возращает и задает имя контакта.
        /// </summary>
        public string Name
        {
            get => Contact.Name;
            set
            {
                Contact.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает и задает номер телефона.
        /// </summary>
        public string PhoneNumber
        {
            get => Contact.PhoneNumber;
            set
            {
                Contact.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает и задает электронную почту.
        /// </summary>
        public string Email
        {
            get => Contact.Email;
            set
            {
                Contact.Email = value;
                OnPropertyChanged();
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
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Создает экзепляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            LoadCommand = new RelayCommand(LoadContact);
            SaveCommand = new RelayCommand(SaveContact);
        }

        /// <summary>
        /// Оповещает об изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Загружает данные о контакте.
        /// </summary>
        /// <param name="obj">Экзепляр класса <see cref="object"/>.</param>
        private void LoadContact()
        {
            var contact = ContactSerializer.Contact;
            Name = contact.Name;
            PhoneNumber = contact.PhoneNumber;
            Email = contact.Email;
        }

        /// <summary>
        /// Сохраняет данные о контакте.
        /// </summary>
        /// <param name="obj">Экзепляр класса <see cref="object"/>.</param>
        private void SaveContact()
        {
            ContactSerializer.Contact = Contact;
        }
    }
}
