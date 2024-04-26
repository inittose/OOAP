using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using View.Model.Services;

namespace View.Model
{
    /// <summary>
    /// Хранит информацию о контакте
    /// </summary>
    public class Contact : INotifyPropertyChanged, ICloneable, IDataErrorInfo
    {
        public const int NameLengthLimit = 100;

        public const int PhoneNumberLengthLimit = 100;

        public const int EmailLengthLimit = 100;

        public const string PhoneNumberMask = "1234567890+-() ";

        public const string EmailMask = "@";

        /// <summary>
        /// Имя контакта.
        /// </summary>
        private string _name;

        /// <summary>
        /// Номер телефона.
        /// </summary>
        private string _phoneNumber;

        /// <summary>
        /// Электронная почта.
        /// </summary>
        private string _email;

        /// <summary>
        /// Возвращает и задает имя контакта.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Возвращает и задает номер телефона.
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Возвращает и задает электронную почту.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string this[string propertyName]
        {
            get
            {
                string error = String.Empty;
                switch (propertyName)
                {
                    case nameof(Name):
                        try
                        {
                            ValueValidator.AssertStringOnLength(
                                Name,
                                NameLengthLimit,
                                nameof(Name));
                        }
                        catch (ArgumentException ex)
                        {
                            error = ex.Message;
                        }

                        break;
                    case nameof(PhoneNumber):
                        try
                        {
                            ValueValidator.AssertStringOnLength(
                                PhoneNumber,
                                PhoneNumberLengthLimit,
                                nameof(PhoneNumber));

                            ValueValidator.AssertStringOnMask(
                                PhoneNumberMask,
                                PhoneNumber,
                                nameof(PhoneNumber));
                        }
                        catch (ArgumentException ex)
                        {
                            error = ex.Message;
                        }

                        break;
                    case nameof(Email):
                        try
                        {
                            ValueValidator.AssertStringOnLength(
                                Email,
                                EmailLengthLimit,
                                nameof(Email));

                            ValueValidator.AssertStringOnMask(
                                Email,
                                EmailMask,
                                nameof(Email));
                        }
                        catch (ArgumentException ex)
                        {
                            error = ex.Message;
                        }

                        break;
                }

                return error;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string Error => string.Empty;

        /// <summary>
        /// Событие, которое происходит при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Созадет экземпляр класса <see cref="Contact"/>.
        /// </summary>
        public Contact()
        {
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        /// <summary>
        /// Созадет экземпляр класса <see cref="Contact"/>.
        /// </summary>
        /// <param name="name">Имя контакта.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="email">Электронная почта.</param>
        public Contact(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
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
        /// Создает новый объект, который является копией текущего экземпляра.
        /// </summary>
        /// <returns>Новый объект, являющийся копией этого экземпляра.</returns>
        public object Clone()
        {
            return new Contact(Name, PhoneNumber, Email);
        }
    }
}