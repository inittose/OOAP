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
        private bool _isApplyVisible;

        private bool _isEditingStatus;

        private Contact _currentContact;

        /// <summary>
        /// Возвращает и задает список контактов.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }

        public string? Name
        {
            get => CurrentContact?.Name;
            set
            {
                CurrentContact.Name = value;
                OnPropertyChanged();
            }
        }

        public string? PhoneNumber
        {
            get => CurrentContact?.PhoneNumber;
            set
            {
                CurrentContact.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string? Email
        {
            get => CurrentContact?.Email;
            set
            {
                CurrentContact.Email = value;
                OnPropertyChanged();
            }
        }

        public bool IsApplyVisibile
        {
            get => _isApplyVisible;
            set
            {
                _isApplyVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditingStatus
        {
            get
            {
                return _isEditingStatus;
            }
            set
            {
                _isEditingStatus = value;
                IsApplyVisibile = IsEditingStatus;
                OnPropertyChanged();
            }
        }

        public Contact CurrentContact
        {
            get => _currentContact;
            set
            {
                if (_currentContact != value)
                {
                    if (IsEditingStatus)
                    {
                        if (Contacts.Contains(CurrentContact))
                        {
                            Name = ContactBeforeChanges.Name;
                            PhoneNumber = ContactBeforeChanges.PhoneNumber;
                            Email = ContactBeforeChanges.Email;
                        }

                        IsEditingStatus = false;
                    }

                    _currentContact = value;
                    OnPropertyChanged();

                    foreach (var prop in typeof(Contact).GetProperties())
                    {
                        OnPropertyChanged(prop.Name);
                    }
                }
            }
        }

        public Contact ContactBeforeChanges { get; set; }

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
            _isApplyVisible = false;
            _isEditingStatus = false;
            Contacts = ContactSerializer.Contacts;
            AddCommand = new RelayCommand(AddContact);
            EditCommand = new RelayCommand(EditContact);
            RemoveCommand = new RelayCommand(RemoveContact);
            ApplyCommand = new RelayCommand(ApplyContact);
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
            CurrentContact = new Contact();
            IsEditingStatus = true;
        }

        /// <summary>
        /// Редактирует информацию контакта.
        /// </summary>
        private void EditContact()
        {
            ContactBeforeChanges = (Contact)CurrentContact.Clone();
            IsEditingStatus = true;
        }

        private void RemoveContact()
        {
            var index = Contacts.IndexOf(CurrentContact);
            Contacts.Remove(CurrentContact);

            if (index > 0 && index >= Contacts.Count)
            {
                CurrentContact = Contacts[Contacts.Count - 1];
            }
            else if (index >= 0 && index < Contacts.Count)
            {
                CurrentContact = Contacts[index];
            }

            ContactSerializer.Contacts = Contacts;
        }

        private void ApplyContact()
        {
            if (!Contacts.Contains(CurrentContact))
            {
                Contacts.Add(CurrentContact);
            }

            IsEditingStatus = false;
            ContactSerializer.Contacts = Contacts;
        }
    }
}
