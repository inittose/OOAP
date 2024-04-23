using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace View.ViewModel
{
    /// <summary>
    /// Хранит логику преобразования <see cref="bool"/> в <see cref="Visibility"/>.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует <see cref="bool"/> в <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">Значение, произведенное исходной привязкой.</param>
        /// <param name="targetType">Тип целевого свойства привязки.</param>
        /// <param name="parameter">Используемый параметр преобразователя.</param>
        /// <param name="culture">Язык и региональные параметры, используемые в
        /// преобразователе.</param>
        /// <returns>Если <see cref="true"/>, то возвращает <see cref="Visibility.Visible"/>,
        /// иначе <see cref="Visibility.Hidden"/>.</returns>
        public object Convert(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            if (value is not bool)
            {
                return null;
            }

            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Преобразует <see cref="Visibility"/> в <see cref="bool"/>.
        /// </summary>
        /// <param name="value">Значение, произведенное исходной привязкой.</param>
        /// <param name="targetType">Тип целевого свойства привязки.</param>
        /// <param name="parameter">Используемый параметр преобразователя.</param>
        /// <param name="culture">Язык и региональные параметры, используемые в
        /// преобразователе.</param>
        /// <returns>Если <see cref="Visibility.Visible"/>, то возвращает <see cref="true"/>,
        /// иначе <see cref="false"/>.</returns>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (Equals(value, Visibility.Visible))
            {
                return true;
            }

            if (Equals(value, Visibility.Hidden))
            {
                return false;
            }

            return null;
        }
    }
}
