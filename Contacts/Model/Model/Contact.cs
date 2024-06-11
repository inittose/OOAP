using CommunityToolkit.Mvvm.ComponentModel;

namespace View.Model
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
        /// Максимальное количество символов свойства <see cref="PhoneNumber"/>.
        /// </summary>
        public const int PhoneNumberLengthLimit = 100;

        /// <summary>
        /// Максимальное количество символов свойства <see cref="Email"/>.
        /// </summary>
        public const int EmailLengthLimit = 100;

        /// <summary>
        /// Маска-строка, из которой должно составляться свойство <see cref="PhoneNumber"/>.
        /// </summary>
        public const string PhoneNumberMask = "1234567890+-() ";

        /// <summary>
        /// Маска-строка, которая должна содержаться в свойстве <see cref="Email"/>.
        /// </summary>
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