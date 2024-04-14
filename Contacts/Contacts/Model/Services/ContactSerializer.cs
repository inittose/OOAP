using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace View.Model.Services
{
    /// <summary>
    /// Сериализует и десериализует данные контакта.
    /// </summary>
    public static class ContactSerializer
    {
        /// <summary>
        /// Возвращает путь до файла сериализации.
        /// </summary>
        private static string FilePath { get; } = 
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
            "\\Contacts\\Contact.json";

        /// <summary>
        /// Возвращает и задает информацию о контакте в виде json.
        /// </summary>
        private static string ContactsJson { get; set; } = string.Empty;

        /// <summary>
        /// Выгружает данные о контакте, если они есть.
        /// </summary>
        static ContactSerializer()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            try
            {
                ContactsJson = File.ReadAllText(FilePath);
            }
            catch
            {
                ContactsJson = string.Empty;
            }
        }

        /// <summary>
        /// Десериализует данные о контакте.
        /// </summary>
        /// <returns>Экзепляр класса <see cref="Contact"/>.</returns>
        public static ObservableCollection<Contact> GetContacts()
        {
            if (ContactsJson == string.Empty)
            {
                return new ObservableCollection<Contact>();
            }
            else
            {
                try
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<Contact>>(
                        ContactsJson,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                }
                catch
                {
                    ContactsJson = string.Empty;
                    MessageBox.Show("Data is corrupted.\nSave files have been cleared.");

                    return new ObservableCollection<Contact>();
                }
            }
        }

        /// <summary>
        /// Сериализует данные о контакте.
        /// </summary>
        /// <param name="contact">Экзепляр класса <see cref="Contact"/>.</param>
        public static void SetContacts(ObservableCollection<Contact> contacts)
        {
            ContactsJson = JsonConvert.SerializeObject(
                contacts,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            SaveFile();
        }

        /// <summary>
        /// Сохраняет данные о контакте в файл сериализации.
        /// </summary>
        private static void SaveFile()
        {
            File.WriteAllText(FilePath, ContactsJson);
        }
    }
}
