using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using View.Model;

namespace View.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ContactControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public ContactControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Contact.PhoneNumberMask.Contains(e.Text))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
