using System.Globalization;
using System.Windows.Data;

namespace View.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public object ConvertBack(
            object value, 
            Type targetType, 
            object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
