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
        /// Возвращает и задает контакт из файла сериализации.
        /// </summary>
        public static Contact Contact
        {
            get
            {
                return ConvertJsonToContact() ?? new Contact();
            }
            set
            {
                ConvertContactToJson(value);
            }
        }

        /// <summary>
        /// Возвращает путь до файла сериализации.
        /// </summary>
        private static string FilePath { get; } = 
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
            "\\Contacts\\Contact.json";

        /// <summary>
        /// Возвращает и задает информацию о контакте в виде json.
        /// </summary>
        private static string ContactJson { get; set; } = string.Empty;

        /// <summary>
        /// Выгружает данные о контакте, если они есть.
        /// </summary>
        static ContactSerializer()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            try
            {
                ContactJson = File.ReadAllText(FilePath);
            }
            catch
            {
                ContactJson = string.Empty;
            }
        }

        /// <summary>
        /// Десериализует данные о контакте.
        /// </summary>
        /// <returns>Экзепляр класса <see cref="Model.Contact"/>.</returns>
        private static Contact? ConvertJsonToContact()
        {
            if (ContactJson == string.Empty)
            {
                return new Contact();
            }

            Contact? contact = null;

            try
            {
                contact = JsonConvert.DeserializeObject<Contact>(
                    ContactJson,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            }
            catch
            {
                ContactJson = string.Empty;
                MessageBox.Show("Data is corrupted.\nSave files have been cleared.");
            }

            return contact;
        }

        /// <summary>
        /// Сериализует данные о контакте.
        /// </summary>
        /// <param name="contact">Экзепляр класса <see cref="Model.Contact"/>.</param>
        private static void ConvertContactToJson(Contact contact)
        {
            ContactJson = JsonConvert.SerializeObject(
                contact,
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
            File.WriteAllText(FilePath, ContactJson);
        }
    }
}
