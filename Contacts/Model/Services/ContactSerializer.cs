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
        /// Возвращает и задает информацию о контактах в виде json.
        /// </summary>
        private static string ContactJson { get; set; } = string.Empty;

        /// <summary>
        /// Выгружает данные о контактах, если они есть.
        /// </summary>
        // TODO: Сделать чтение на уровне десериализации.
        // Весь код в статическом конструкторе вынести в отдельный приватный метод,
        // который будет возвращать JSON строку. Свойство ContactJson убрать.
        // Это нужно для того, чтобы при запуске программы статические элементы не заполнялись сразу
        // и не потребляли память, а делали это тогда, когда идет первый вызов метода, где они используются.
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
        /// <returns>Экземпляр класса <see cref="Contact"/>.</returns>
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
        /// <param name="contact">Экземпляр класса <see cref="Contact"/>.</param>
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
        // TODO: Когда свойство ContactJson уберется добавить аргумент данному методу.
        // TODO: Не забыть про XML-комментарий.
        private static void SaveFile()
        {
            File.WriteAllText(FilePath, ContactJson);
        }
    }
}