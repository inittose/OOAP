namespace Model.Services
{
    public static class ContactValidator
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
        public const string PhoneNumberRegex =
            @"^\+?\d{1,3}\s?\(?\d{3}\)?\s?\d{3}[-\s\.]?\d{2}[-\s\.]?\d{2}$";

        /// <summary>
        /// Маска-строка, из которой должно составляться свойство <see cref="PhoneNumber"/>.
        /// </summary>
        public const string PhoneNumberMask = @"[0-9+\-().]+";

        /// <summary>
        /// Регулярное выражение, которое должно содержаться в свойстве <see cref="Email"/>.
        /// </summary>
        public const string EmailRegex = @"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+";

        public static string Validate(string propertyName, string value)
        {
            string error = string.Empty;

            switch (propertyName)
            {
                case nameof(Contact.Name):
                    {
                        try
                        {
                            ValueValidator.AssertStringOnLength(
                                value,
                                ContactValidator.NameLengthLimit,
                                nameof(Contact.Name));
                        }
                        catch (ArgumentException exception)
                        {
                            error = exception.Message;
                        }

                        break;
                    }
                case nameof(Contact.PhoneNumber):
                    {
                        try
                        {
                            ValueValidator.AssertStringOnRegex(
                                value,
                                ContactValidator.PhoneNumberRegex,
                                nameof(Contact.PhoneNumber));
                        }
                        catch (ArgumentException exception)
                        {
                            error = exception.Message;
                        }

                        break;
                    }
                case nameof(Contact.Email):
                    {
                        try
                        {
                            ValueValidator.AssertStringOnLimits(
                                value,
                                ContactValidator.EmailLowerLengthLimit,
                                ContactValidator.EmailUpperLengthLimit,
                                nameof(Contact.Email));

                            ValueValidator.AssertStringOnRegex(
                                value,
                                ContactValidator.EmailRegex,
                                nameof(Contact.Email));
                        }
                        catch (ArgumentException exception)
                        {
                            error = exception.Message;
                        }

                        break;
                    }
            }

            return error;
        }
    }
}
