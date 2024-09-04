using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using Model.Services;

namespace ViewModel
{
    /// <summary>
    /// Управляет логикой работы программы.
    /// </summary>
    public class MainVM : ObservableObject, INotifyDataErrorInfo
    {
        /// <summary>
        /// Текст поисковой строки.
        /// </summary>
        private string _searchLine = string.Empty;

        /// <summary>
        /// Статус редактирования/создания контакта.
        /// </summary>
        private bool _isEditingStatus;

        /// <summary>
        /// Текущий контакт.
        /// </summary>
        private Contact _currentContact;

        /// <summary>
        /// Индекс редактируемого контакта.
        /// </summary>
        private int EditingContactIndex { get; set; } = -1;

        // TODO: Убрать поле и свойство. Оставить только CurrentContact.
        // При изменении можно делать клон элемента, который будет устанавливаться в CurrentContact.
        // Его и надо будет изменять.
        // UPD: +

        /// <summary>
        /// Возвращает словарь ошибок, где ключ - свойство, а значение - текст ошибки.
        /// </summary>
        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Возвращает и задает список контактов.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Возвращает список отображенных контактов.
        /// </summary>
        public ObservableCollection<Contact> ShowedContacts
        {
            get
            {
                var contacts = new ObservableCollection<Contact>();

                foreach (var contact in Contacts)
                {
                    if (contact.Name.Contains(SearchLine) || 
                        contact.PhoneNumber.Contains(SearchLine))
                    {
                        contacts.Add(contact);
                    }
                }

                return contacts;
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
                    IsEditingStatus = false;

                    Validate(nameof(Name));
                    Validate(nameof(PhoneNumber));
                    Validate(nameof(Email));

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
            get => CurrentContact?.Name ?? string.Empty;
            set
            {
                // TODO: Сделать проверку на то, что не присваивается такое же значение. Ниже приведен пример:
                // if(SetProperty(EditedContact.Name, value, EditedContact, (contact, value) => contact.Name = value))
                // UPD: +
                if (
                    SetProperty(
                        CurrentContact.Name, 
                        value, 
                        CurrentContact, 
                        (contact, value) => contact.Name = value))
                {
                    OnPropertyChanged();
                    Validate(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Возвращает и задает номер телефона контакта.
        /// </summary>
        public string PhoneNumber
        {
            get => CurrentContact?.PhoneNumber ?? string.Empty;
            set
            {
                // TODO: Сделать проверку на то, что не присваивается такое же значение.
                // UPD: +
                if (
                    SetProperty(
                        CurrentContact.PhoneNumber, 
                        value, 
                        CurrentContact, 
                        (contact, value) => contact.PhoneNumber = value))
                {
                    OnPropertyChanged();
                    Validate(nameof(PhoneNumber));
                }
            }
        }

        /// <summary>
        /// Возвращает и задает электронную почту контакта.
        /// </summary>
        public string Email
        {
            get => CurrentContact?.Email ?? string.Empty;
            set
            {
                // TODO: Сделать проверку на то, что не присваивается такое же значение.
                // UPD: +
                if (
                    SetProperty(
                        CurrentContact.Email, 
                        value, 
                        CurrentContact, 
                        (contact, value) => contact.Email = value))
                {
                    OnPropertyChanged();
                    Validate(nameof(Email));
                }
            }
        }

        /// <summary>
        /// Возвращает и задает текст поисковой строки.
        /// </summary>
        public string SearchLine
        {
            get => _searchLine;
            set
            {
                // TODO: Сделать проверку на то, что не присваивается такое же значение.
                // UPD: +
                if (
                    SetProperty(
                        _searchLine, 
                        value, 
                        SearchLine, 
                        (searchLine, value) => searchLine = value))
                {
                    OnPropertyChanged(nameof(ShowedContacts));
                }
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
                // TODO: Сделать проверку на то, что не присваивается такое же значение.
                // UPD: +

                if (IsEditingStatus != value)
                {
                    _isEditingStatus = value;
                    Validate(nameof(Name));
                    Validate(nameof(PhoneNumber));
                    Validate(nameof(Email));
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsReadonlyContactSelected));
                    OnPropertyChanged(nameof(IsSelectingStatus));
                }
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

        // TODO: Убрать связь с типами, которые относятся к отрисовки (Visibility).
        // Сделать его bool свойством. В проекте View сделать конвертер BoolToVisibility
        // или использовать готовый реализованный конвертер в WPF.
        // UPD: +

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
        // TODO: Вынести в отдельный класс ContactValidator.
        // TODO: В него же вынести константы диапазонов, которые находятся в классе Contact.
        private void Validate(string propertyName)
        {
            string error = string.Empty;

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
                            ValueValidator.AssertStringOnRegex(
                                PhoneNumber,
                                Contact.PhoneNumberRegex,
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
                            ValueValidator.AssertStringOnLimits(
                                Email,
                                Contact.EmailLowerLengthLimit,
                                Contact.EmailUpperLengthLimit,
                                nameof(Email));

                            ValueValidator.AssertStringOnRegex(
                                Email,
                                Contact.EmailRegex,
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
        /// Добавляет новый контакт в список контактов.
        /// </summary>
        private void AddContact()
        {
            CurrentContact = null;
            CurrentContact = new Contact();
            IsEditingStatus = true;
            EditingContactIndex = -1;
        }

        /// <summary>
        /// Редактирует информацию контакта.
        /// </summary>
        private void EditContact()
        {
            EditingContactIndex = Contacts.IndexOf(CurrentContact);
            CurrentContact = (Contact)CurrentContact.Clone();
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
            OnPropertyChanged(nameof(ShowedContacts));
        }

        /// <summary>
        /// Сохраняет изменения контакта.
        /// </summary>
        private void ApplyContact()
        {
            if (EditingContactIndex < 0)
            {
                Contacts.Add(CurrentContact);
            }
            else
            {
                Contacts[EditingContactIndex].Name = Name;
                Contacts[EditingContactIndex].PhoneNumber = PhoneNumber;
                Contacts[EditingContactIndex].Email = Email;
            }

            EditingContactIndex = -1;
            IsEditingStatus = false;
            ContactSerializer.Contacts = Contacts;
            OnPropertyChanged(nameof(ShowedContacts));
        }
    }
}
