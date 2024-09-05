using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model.Services;

namespace View.Controls
{
    /// <summary>
    /// Хранит логику работы пользовательского интерфейса <see cref="ContactControl"/>.
    /// </summary>
    public partial class ContactControl : UserControl
    {
        /// <summary>
        /// Создает экземпляр класса <see cref="ContactControl"/>.
        /// </summary>
        public ContactControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает событие ввода символа в <see cref="TextBox"/>.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Данные о событии.</param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, ContactValidator.PhoneNumberMask))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обрабатывает событие вставки символов в <see cref="TextBox"/>.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Данные о событии.</param>
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            var value = (string)e.DataObject.GetData(typeof(string));
            var correctText = string.Join("", Regex.Matches(value, ContactValidator.PhoneNumberMask));

            var correctData = new DataObject();
            correctData.SetData(DataFormats.Text, correctText);
            e.DataObject = correctData;
        }
    }
}
