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