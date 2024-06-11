using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace View.Model.Services
{
    /// <summary>
    /// Сериализует и десериализует данные контактов.
    /// </summary>
    public static class ContactSerializer
    {
        /// <summary>
        /// Возвращает и задает контакты из файла сериализации.
        /// </summary>
        public static ObservableCollection<Contact> Contacts
        {
            get => Deserialize() ?? new ObservableCollection<Contact>();
            set => Serialize(value);
        }

        /// <summary>
        /// Возвращает путь до файла сериализации.
        /// </summary>
        private static string FilePath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
            "\\Contacts\\Contact.json";

        /// <summary>
        /// Возвращает и задает информацию о контактах в виде json.
        /// </summary>
        private static string ContactJson { get; set; } = string.Empty;

        /// <summary>
        /// Выгружает данные о контактах, если они есть.
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
        /// Десериализует данные о контактах.
        /// </summary>
        /// <returns>Экземпляр класса <see cref="Model.Contact"/>.</returns>
        private static ObservableCollection<Contact> Deserialize()
        {
            if (ContactJson == string.Empty)
            {
                return new ObservableCollection<Contact>();
            }

            ObservableCollection<Contact>? contacts = null;

            try
            {
                contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(
                    ContactJson,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            }
            catch
            {
                ContactJson = string.Empty;
            }

            return contacts ?? new ObservableCollection<Contact>();
        }

        /// <summary>
        /// Сериализует данные о контактах.
        /// </summary>
        /// <param name="contact">Экземпляр класса <see cref="Model.Contact"/>.</param>
        private static void Serialize(ObservableCollection<Contact> contact)
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
        /// Сохраняет данные о контактах в файл сериализации.
        /// </summary>
        private static void SaveFile()
        {
            File.WriteAllText(FilePath, ContactJson);
        }
    }
}