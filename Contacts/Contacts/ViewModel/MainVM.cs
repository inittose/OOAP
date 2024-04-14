using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        private Contact _currentContact;

        /// <summary>
        /// Возвращает и задает список контактов.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }

        public Contact CurrentContact
        {
            get => _currentContact;
            set
            {
                if (_currentContact != value)
                {
                    _currentContact = value;
                    OnPropertyChanged();

                    foreach (var prop in typeof(Contact).GetProperties())
                    {
                        OnPropertyChanged(prop.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает команду добавления контакта.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Возвращает команду редактирования контакта.
        /// </summary>
        public ICommand EditCommand { get; }

        /// <summary>
        /// Возвращает команду удаления контакта.
        /// </summary>
        public ICommand RemoveCommand { get; }

        /// <summary>
        /// Возвращает команду успешного редактирования контакта.
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Событие, которое происходит при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Создает экзепляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            Contacts = ContactSerializer.GetContacts();
            Contacts.Add(new Contact("NewName", "NewPhoneNumber", "NewEmail"));
            AddCommand = new RelayCommand(AddContact);
            EditCommand = new RelayCommand(EditContact);
            RemoveCommand = new RelayCommand(RemoveContact);
            ApplyCommand = new RelayCommand(ApplyContact);
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
        /// Добавляет нового контакта в список контактов.
        /// </summary>
        private void AddContact()
        {
            
        }

        /// <summary>
        /// Редактирует информацию контакта.
        /// </summary>
        private void EditContact()
        {
            
        }

        private void RemoveContact()
        {

        }

        private void ApplyContact()
        {
            ContactSerializer.SetContacts(Contacts);
        }
    }
}
