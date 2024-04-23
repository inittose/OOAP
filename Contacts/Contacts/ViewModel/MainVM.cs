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
        /// 
        /// </summary>
        private bool _isApplyVisible;

        /// <summary>
        /// 
        /// </summary>
        private bool _isEditingStatus;

        /// <summary>
        /// 
        /// </summary>
        private Contact _currentContact;

        /// <summary>
        /// Возвращает и задает список контактов.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Contact EditedContact { get; set; }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public string? Name
        {
            get => EditedContact?.Name;
            set
            {
                EditedContact.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string? PhoneNumber
        {
            get => EditedContact?.PhoneNumber;
            set
            {
                EditedContact.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string? Email
        {
            get => EditedContact?.Email;
            set
            {
                EditedContact.Email = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
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
                IsApplyVisibile = IsEditingStatus;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadonlyContactSelected));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsApplyVisibile
        {
            get => _isApplyVisible;
            set
            {
                _isApplyVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
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
            EditedContact = CurrentContact;
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
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private void ApplyContact()
        {
            if (!Contacts.Contains(CurrentContact))
            {
                Contacts.Add(CurrentContact);
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
