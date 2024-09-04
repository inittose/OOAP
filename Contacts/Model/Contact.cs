using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.RegularExpressions;

namespace Model
{
    /// <summary>
    /// Хранит информацию о контакте.
    /// </summary>
    public class Contact : ObservableObject, ICloneable
    {
        /// <summary>
        /// Максимальное количество символов свойства <see cref="Name"/>.
        /// </summary>
        public const int NameLengthLimit = 100;

        /// <summary>
        /// Минимальное количество символов свойства <see cref="PhoneNumber"/>.
        /// </summary>
        public const int PhoneNumberLowerLengthLimit = 2;

        /// <summary>
        /// Максимальное количество символов свойства <see cref="PhoneNumber"/>.
        /// </summary>
        public const int PhoneNumberUpperLengthLimit = 15;

        /// <summary>
        /// Минимальное количество символов свойства <see cref="Email"/>.
        /// </summary>
        public const int EmailLowerLengthLimit = 6;

        /// <summary>
        /// Максимальное количество символов свойства <see cref="Email"/>.
        /// </summary>
        public const int EmailUpperLengthLimit = 100;

        /// <summary>
        /// Регулярное выражение, которое должно содержаться в свойстве <see cref="PhoneNumber"/>.
        /// </summary>
        // TODO: Используй более сложную маску для проверки номера телефона. Плюс используй Regex
        // https://ihateregex.io/expr/phone
        // UDP: Добавил регулярное выражение для номера телефона, поправил валидатор
        public const string PhoneNumberRegex = 
            @"^\+?\d{1,3}\s?\(?\d{3}\)?\s?\d{3}[-\s\.]?\d{2}[-\s\.]?\d{2}$";

        /// <summary>
        /// Маска-строка, из которой должно составляться свойство <see cref="PhoneNumber"/>.
        /// </summary>
        public const string PhoneNumberMask = @"[0-9+\-().]+";

        /// <summary>
        /// Регулярное выражение, которое должно содержаться в свойстве <see cref="Email"/>.
        /// </summary>
        // TODO: Используй более сложную маску для проверки почты. Плюс используй Regex
        // https://ihateregex.io/expr/email
        // UDP: Изменил маску на регулярное выражение, поправил валидатор
        public const string EmailRegex = @"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+";

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
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Возвращает и задает номер телефона.
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        /// <summary>
        /// Возвращает и задает электронную почту.
        /// </summary>
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="Contact"/>.
        /// </summary>
        public Contact()
        {
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="Contact"/>.
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
        /// Создает новый объект, который является копией текущего экземпляра.
        /// </summary>
        /// <returns>Новый объект, являющийся копией этого экземпляра.</returns>
        public object Clone()
        {
            return new Contact(Name, PhoneNumber, Email);
        }
    }
}