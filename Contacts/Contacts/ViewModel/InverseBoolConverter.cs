using System.Globalization;
using System.Windows.Data;

namespace View.ViewModel
{
    /// <summary>
    /// Хранит логику инвертирования значения <see cref="bool"/>.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Инвертирует значение <see cref="bool"/>.
        /// </summary>
        /// <param name="value">Значение, произведенное исходной привязкой.</param>
        /// <param name="targetType">Тип целевого свойства привязки.</param>
        /// <param name="parameter">Используемый параметр преобразователя.</param>
        /// <param name="culture">Язык и региональные параметры, используемые в
        /// преобразователе.</param>
        /// <returns>Если <see cref="true"/>, то возвращает <see cref="false"/>,
        /// иначе <see cref="true"/>.</returns>
        public object Convert(
            object value, 
            Type targetType, 
            object parameter,
            CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean.");

            return !(bool)value;
        }

        /// <summary>
        /// Инвертирует значение <see cref="bool"/>.
        /// </summary>
        /// <param name="value">Значение, произведенное исходной привязкой.</param>
        /// <param name="targetType">Тип целевого свойства привязки.</param>
        /// <param name="parameter">Используемый параметр преобразователя.</param>
        /// <param name="culture">Язык и региональные параметры, используемые в
        /// преобразователе.</param>
        /// <returns>Возвращает <see cref="NotSupportedException"/>.</returns>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
