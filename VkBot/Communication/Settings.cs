using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace VkBot.Communication
{
    public class Settings
    {
        public long Admin { get; set; }
        public Status Status { get; set; }
        public string LastWord { get; set; }
        public int Frequency { get => FreqLimit; set => FreqLimit = value > 100 ? 100 : value < 0 ? 0 : value; }
        private int FreqLimit { get; set; }

        public UserStatus GetUserStatus(long? id) => id == null ? UserStatus.Simple
            : id.Value == Admin ? UserStatus.Admin : UserStatus.Simple;

        public void Save(string path)
        {
            var text = JsonSerializer.Serialize(this);
            File.WriteAllText(path, text);
        }

        public static Settings Get(string path)
        {
            if (!File.Exists(path))
                new Settings().Save(path);

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(json);
        }

        public Response Set(SettingType type, string value)
        {
            var response = new Response(ResponseType.Text) { Content = "Параметр не найден" };

            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (!propertyInfo.Name.Equals(type.ToString(), StringComparison.OrdinalIgnoreCase))
                    continue;

                var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                if (TryConvert(converter, value, out var converted))
                {
                    propertyInfo.SetValue(this, converted);
                    response.Content = $"Параметр {propertyInfo.Name} установлен на {propertyInfo.GetValue(this)}";
                }
                else
                    response.Content = $"Неверный параметр {value}";
                return response;
            }

            return response;
        }

        private bool TryConvert(TypeConverter converter, string value, out object result)
        {
            try
            {
                result = converter.ConvertFromString(value);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
