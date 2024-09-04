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

        // TODO: Сделать чтение на уровне десериализации.
        // Весь код в статическом конструкторе вынести в отдельный приватный метод,
        // который будет возвращать JSON строку. Свойство ContactJson убрать.
        // Это нужно для того, чтобы при запуске программы статические элементы не заполнялись сразу
        // и не потребляли память, а делали это тогда, когда идет первый вызов метода, где они используются.
        // UPD: +

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
        // TODO: Когда свойство ContactJson уберется добавить аргумент данному методу.
        // UPD: +
        // TODO: Не забыть про XML-комментарий.
        // UPD: +
        private static void SaveFile(string ContactJson)
        {
            File.WriteAllText(FilePath, ContactJson);
        }
    }
}