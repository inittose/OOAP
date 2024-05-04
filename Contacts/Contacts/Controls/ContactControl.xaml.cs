using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using View.Model;

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
            if (!Contact.PhoneNumberMask.Contains(e.Text))
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
            var correctText = string.Empty;

            foreach (var character in (string)e.DataObject.GetData(typeof(string)))
            {
                if (Contact.PhoneNumberMask.Contains(character))
                {
                    correctText += character;
                }
            }

            var correctData = new DataObject();
            correctData.SetData(DataFormats.Text, correctText);
            e.DataObject = correctData;
        }
    }
}
