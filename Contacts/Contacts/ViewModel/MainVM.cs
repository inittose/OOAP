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
        /// <summary>
        /// Статус редактирования/создания контакта.
        /// </summary>
        private bool _isEditingStatus;

        /// <summary>
        /// Текущий контакт.
        /// </summary>
        private Contact _currentContact;

        /// <summary>
        /// Возвращает и задает список контактов.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Возвращает и задает контакт, который поддается редактированию.
        /// </summary>
        public Contact EditedContact { get; set; }

        /// <summary>
        /// Возвращает и задает текущий контакт.
        /// </summary>
        public Contact CurrentContact
        {
            get => _currentContact;
            set
            {
                if (_currentContact != value)
                {
                    _currentContact = value;
                    EditedContact = CurrentContact;
                    IsEditingStatus = false;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsReadonlyContactSelected));

                    foreach (var property in typeof(Contact).GetProperties())
                    {
                        OnPropertyChanged(property.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает и задает имя контакта.
        /// </summary>
        public string Name
        {
            get => EditedContact?.Name ?? string.Empty;
            set
            {
                EditedContact.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает и задает номер телефона контакта.
        /// </summary>
        public string PhoneNumber
        {
            get => EditedContact?.PhoneNumber ?? string.Empty;
            set
            {
                EditedContact.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает и задает электронную почту контакта.
        /// </summary>
        public string Email
        {
            get => EditedContact?.Email ?? string.Empty;
            set
            {
                EditedContact.Email = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает и задает статус редактирования/создания контакта.
        /// </summary>
        public bool IsEditingStatus
        {
            get
            {
                return _isEditingStatus;
            }
            set
            {
                _isEditingStatus = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadonlyContactSelected));
            }
        }

        /// <summary>
        /// Возвращает <see cref="true"/>, если контакт выбран и контакт не редактируется.
        /// </summary>
        public bool IsReadonlyContactSelected => CurrentContact != null && !IsEditingStatus;

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
        /// Создает экземпляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            _isEditingStatus = false;
            AddCommand = new RelayCommand(AddContact);
            EditCommand = new RelayCommand(EditContact);
            RemoveCommand = new RelayCommand(RemoveContact);
            ApplyCommand = new RelayCommand(ApplyContact);
            Contacts = ContactSerializer.Contacts;
        }

        /// <summary>
        /// Оповещает об изменении свойства.
        /// </summary>
        /// <param name="property">Имя свойства.</param>
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Добавляет нового контакта в список контактов.
        /// </summary>
        private void AddContact()
        {
            CurrentContact = null;
            EditedContact = new Contact();
            IsEditingStatus = true;
        }

        /// <summary>
        /// Редактирует информацию контакта.
        /// </summary>
        private void EditContact()
        {
            EditedContact = (Contact)CurrentContact.Clone();
            IsEditingStatus = true;
        }

        /// <summary>
        /// Удаляет контакт.
        /// </summary>
        private void RemoveContact()
        {
            var index = Contacts.IndexOf(CurrentContact);
            Contacts.Remove(CurrentContact);

            if (0 < index && index >= Contacts.Count)
            {
                CurrentContact = Contacts[Contacts.Count - 1];
            }
            else if (0 <= index && index < Contacts.Count)
            {
                CurrentContact = Contacts[index];
            }

            ContactSerializer.Contacts = Contacts;
        }

        /// <summary>
        /// Сохраняет изменения контакта.
        /// </summary>
        private void ApplyContact()
        {
            if (!Contacts.Contains(CurrentContact))
            {
                Contacts.Add(EditedContact);
                CurrentContact = EditedContact;
            }
            else
            {
                CurrentContact.Name = Name;
                CurrentContact.PhoneNumber = PhoneNumber;
                CurrentContact.Email = Email;
            }

            IsEditingStatus = false;
            ContactSerializer.Contacts = Contacts;
        }
    }
}
