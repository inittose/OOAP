using System.Windows;
using View.ViewModel;

namespace Contacts
{
    /// <summary>
    /// Хранит логику работы <see cref="MainWindow"/>.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Создает экземпляр класса <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    }
}