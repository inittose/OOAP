using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace Model.Services
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

        // TODO: XML
        // TODO: Я говорил о том, что все, что находится в данном методе перенести в сам метод Deserialize(). А этот метод удалить
        private static string DeserializeJson()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            try
            {
                return File.ReadAllText(FilePath);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Десериализует данные о контактах.
        /// </summary>
        /// <returns>Экземпляр класса <see cref="Contact"/>.</returns>
        private static ObservableCollection<Contact> Deserialize()
        {
            // TODO: MSDN
            var ContactJson = DeserializeJson();

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
        /// <param name="contact">Экземпляр класса <see cref="Contact"/>.</param>
        private static void Serialize(ObservableCollection<Contact> contact)
        {
            var ContactJson = JsonConvert.SerializeObject(
                contact,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            SaveFile(ContactJson);
        }

        /// <summary>
        /// Сохраняет данные о контактах в файл сериализации.
        /// </summary>
        /// <param name="ContactJson">Информация о контактах в формате json.</param>
        // TODO: Перенести запись в файл в метод Serialize, а этот метод убрать
        private static void SaveFile(string ContactJson)
        {
            File.WriteAllText(FilePath, ContactJson);
        }
    }
}