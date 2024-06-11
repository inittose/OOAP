namespace View.Model.Services
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