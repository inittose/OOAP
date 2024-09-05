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

        /// <summary>
        /// Десериализует данные о контактах.
        /// </summary>
        /// <returns>Экземпляр класса <see cref="Contact"/>.</returns>
        private static ObservableCollection<Contact> Deserialize()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            string contactJson;

            try
            {
                contactJson = File.ReadAllText(FilePath);
            }
            catch
            {
                return new ObservableCollection<Contact>();
            }

            ObservableCollection<Contact>? contacts = null;

            try
            {
                contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(
                    contactJson,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            }
            catch
            {

            }

            return contacts ?? new ObservableCollection<Contact>();
        }

        /// <summary>
        /// Сериализует данные о контактах.
        /// </summary>
        /// <param name="contact">Экземпляр класса <see cref="Contact"/>.</param>
        private static void Serialize(ObservableCollection<Contact> contact)
        {
            var contactJson = JsonConvert.SerializeObject(
                contact,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            File.WriteAllText(FilePath, contactJson);
        }
    }
}