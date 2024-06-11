using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Collections;
using View.Model;
using View.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace View.ViewModel
{
    /// <summary>
    /// Управляет логикой работы программы.
    /// </summary>
    public class MainVM : ObservableObject, INotifyDataErrorInfo
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
        /// Возвращает словарь ошибок, где ключ - свойство, а значение - текст ошибки.
        /// </summary>
        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

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
                Validate(nameof(Name));
                Validate(nameof(Email));

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
                Validate(nameof(Name));
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
                Validate(nameof(PhoneNumber));
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
                Validate(nameof(Email));
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
        /// Возвращает <see cref="true"/>, если контакт выбран и контакт не редактируется.
        /// </summary>
        public bool IsReadonlyContactSelected => CurrentContact != null && !IsEditingStatus;

        /// <summary>
        /// Возвращает <see cref="Visibility.Visible"/>, если контакт редактируется.
        /// </summary>
        public Visibility IsApplyButtonVisible => 
            IsEditingStatus ? Visibility.Visible : Visibility.Hidden;

        /// <summary>
        /// Возвращает значение, указывающее, имеет ли сущность ошибки проверки.
        /// </summary>
        public bool HasErrors => Errors.Any();

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
        /// Происходит при изменении ошибок проверки для свойства или для всей сущности.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
        }

        /// <summary>
        /// Возвращает ошибки проверки для указанного свойства или для всей сущности.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        /// <summary>
        /// Выполняет валидацию выбранного свойства <see cref="MainVM"/>.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        private void Validate(string propertyName)
        {
            string error = String.Empty;

            if (IsSelectingStatus)
            {
                AddError(propertyName, error);
                OnPropertyChanged(nameof(IsContactCorrect));
                return;
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

            AddError(propertyName, error);
            OnPropertyChanged(nameof(IsContactCorrect));
        }

        /// <summary>
        /// Добавляет ошибку при валидации свойства в <see cref="Errors"/>.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="error">Текст ошибки.</param>
        private void AddError(string propertyName, string error)
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = string.Empty;
            }

            Errors[propertyName] = error;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
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
