using System.Text.RegularExpressions;

namespace Model.Services
{
    /// <summary>
    /// Проводит валидацию входных данных.
    /// </summary>
    public static class ValueValidator
    {
        /// <summary>
        /// Проверяет, превышает ли строка максимальную длину.
        /// </summary>
        /// <param name="value">Входное значение.</param>
        /// <param name="maxLength">Максимальная длина строки.</param>
        /// <param name="propertyName">Имя свойства класса.</param>
        public static void AssertStringOnLength(string value, int maxLength, string propertyName)
        {
            if (value.Length > maxLength)
            {
                throw new
                    ArgumentException($"{propertyName} must be less than {maxLength} characters.");
            }
        }

        /// <summary>
        /// Проверяет, входит ли строка в диапазон длины.
        /// </summary>
        /// <param name="value">Входное значение.</param>
        /// <param name="minLength">Минимальная длина строки.</param>
        /// <param name="maxLength">Максимальная длина строки.</param>
        /// <param name="propertyName">Имя свойства класса.</param>
        /// <param name="unitName">Наименование единиц проверки.</param>
        public static void AssertStringOnLimits(
            string value,
            int minLength,
            int maxLength,
            string propertyName,
            string unitName = "characters")
        {
            if (value.Length > maxLength)
            {
                throw new
                    ArgumentException($"{propertyName} must be less than {maxLength} {unitName}.");
            }
            else if (value.Length < minLength)
            {
                throw new
                    ArgumentException(
                    $"{propertyName} must be greater than {minLength} {unitName}.");
            }
        }

        /// <summary>
        /// Проверяет, состоит ли строка из заданного диапазона числа цифр.
        /// </summary>
        /// <param name="value">Входное значение.</param>
        /// <param name="minLength">Минимальное количество цифр.</param>
        /// <param name="maxLength">Максимальное количество цифр.</param>
        /// <param name="propertyName">Имя свойства класса.</param>
        public static void AssertStringOnDigitLengthLimits(
            string value,
            int minLength,
            int maxLength,
            string propertyName)
        {
            var exctractedDigits = Regex.Match(value, @"\d+").Value;
            AssertStringOnLimits(exctractedDigits, minLength, maxLength, propertyName, "digits");
        }

        /// <summary>
        /// Проверяет, содержит ли строка символы маски-строки.
        /// </summary>
        /// <param name="value">Входное значение.</param>
        /// <param name="mask">Маска-строка.</param>
        /// <param name="propertyName">Имя свойства класса.</param>
        public static void AssertStringOnMask(string value, string mask, string propertyName)
        {
            foreach (var character in mask)
            {
                if (!value.Contains(character))
                {
                    throw new ArgumentException(
                        $"{propertyName} must contain the {character} symbol.");
                }
            }
        }
    }
}