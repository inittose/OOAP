using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Управляет логикой работы программы.
    /// </summary>
    public class MainVM : INotifyPropertyChanged, IDataErrorInfo
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
        /// Контакт, который поддается редактированию.
        /// </summary>
        private Contact _editedContact;

        /// <summary>
        /// Возвращает и задает список контактов.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Возвращает и задает контакт, который поддается редактированию.
        /// </summary>
        public Contact EditedContact
        {
            get => _editedContact;
            set
            {
                _editedContact = value;

                foreach (var property in typeof(Contact).GetProperties())
                {
                    OnPropertyChanged(property.Name);
                }
            }
        }

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
            get => _isEditingStatus;
            set
            {
                _isEditingStatus = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadonlyContactSelected));
                OnPropertyChanged(nameof(IsApplyButtonVisible));
                OnPropertyChanged(nameof(IsSelectingStatus));
            }
        }

        /// <summary>
        /// Возвращает статус валидации контакта.
        /// </summary>
        public bool IsContactCorrect
        {
            get
            {
                foreach (var error in Errors)
                {
                    if (error.Value != string.Empty)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Возвращает словарь ошибок, где ключ - свойство, а значение - текст ошибки.
        /// </summary>
        public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Возвращает <see cref="true"/>, если контакт выбран и контакт не редактируется.
        /// </summary>
        public bool IsReadonlyContactSelected => CurrentContact != null && !IsEditingStatus;

        /// <summary>
        /// Возвращает <see cref="Visibility.Visible"/>, если контакт редактируется.
        /// </summary>
        public Visibility IsApplyButtonVisible => 
            IsEditingStatus ? Visibility.Visible : Visibility.Hidden;

        /// <summary>
        /// Возвращает <see cref="true"/>, если контакт не редактируется.
        /// </summary>
        public bool IsSelectingStatus => !IsEditingStatus;

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
        /// Выполняет валидацию выбранного свойства <see cref="MainVM"/>.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Возвращает <see cref="string.Empty"/>, если валидация прошла успешно,
        /// иначе вернет строку ошибки.</returns>
        public string this[string propertyName]
        {
            get
            {
                string error = String.Empty;

                if (IsSelectingStatus)
                {
                    Errors[propertyName] = error;
                    OnPropertyChanged(nameof(IsContactCorrect));
                    return error;
                }

                switch (propertyName)
                {
                    case nameof(Name):
                    {
                        try
                        {
                            ValueValidator.AssertStringOnLength(
                                Name,
                                Contact.NameLengthLimit,
                                nameof(Name));
                        }
                        catch (ArgumentException exception)
                        {
                            error = exception.Message;
                        }

                        break;
                    }
                    case nameof(PhoneNumber):
                    {
                        try
                        {
                            ValueValidator.AssertStringOnLength(
                                PhoneNumber,
                                Contact.PhoneNumberLengthLimit,
                                nameof(PhoneNumber));

                            ValueValidator.AssertStringOnMask(
                                Contact.PhoneNumberMask,
                                PhoneNumber,
                                nameof(PhoneNumber));
                        }
                        catch (ArgumentException exception)
                        {
                            error = exception.Message;
                        }

                        break;
                    }
                    case nameof(Email):
                    {
                        try
                        {
                            ValueValidator.AssertStringOnLength(
                                Email,
                                Contact.EmailLengthLimit,
                                nameof(Email));

                            ValueValidator.AssertStringOnMask(
                                Email,
                                Contact.EmailMask,
                                nameof(Email));
                        }
                        catch (ArgumentException exception)
                        {
                            error = exception.Message;
                        }

                        break;
                    }
                }

                Errors[propertyName] = error;
                OnPropertyChanged(nameof(IsContactCorrect));

                return error;
            }
        }

        /// <summary>
        /// Возвращает сообщение ошибки.
        /// </summary>
        public string Error => string.Empty;

        /// <summary>
        /// Событие, которое происходит при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Создает экзепляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            _isEditingStatus = false;
            Contacts = ContactSerializer.Contacts;
            AddCommand = new RelayCommand(AddContact);
            EditCommand = new RelayCommand(EditContact);
            RemoveCommand = new RelayCommand(RemoveContact);
            ApplyCommand = new RelayCommand(ApplyContact);
            Errors.Add(nameof(Name), string.Empty);
            Errors.Add(nameof(PhoneNumber), string.Empty);
            Errors.Add(nameof(Email), string.Empty);
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
            IsEditingStatus = true;
            EditedContact = new Contact();
        }

        /// <summary>
        /// Редактирует информацию контакта.
        /// </summary>
        private void EditContact()
        {
            IsEditingStatus = true;
            EditedContact = (Contact)CurrentContact.Clone();
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
