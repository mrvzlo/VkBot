using System;
using System.Collections.Generic;
using System.Linq;

namespace VkBot
{
    public class ChoiceCommand : BaseCommand
    {

        public ChoiceCommand()
        {
            Filters = new[] { "выбери", "или", "либо" };
            Responses = new[] { "Я думаю, что лучше \n", "Наверное всё таки \n" };
        }

        public override string GetResponse(List<string> src)
        {
            var keys = new[] { "или", "либо" };
            var key = src.FirstOrDefault(x => keys.Any(s => x.Equals(s, StringComparison.InvariantCultureIgnoreCase)));
            if (key == null)
                return "Это иллюзия выбора";

            var index = src.LastIndexOf(key);
            var random = new Random(DateTime.Now.Millisecond);
            var part = random.Next(2);

            var result = Responses[random.Next(Filters.Length)];

            if (part == 0)
                for (var i = 1; i < index; i++)
                    result += $"{src[i]} ";
            else
                for (var i = index + 1; i < src.Count; i++)
                    result += $"{src[i]} ";

            return result;
        }

        public override string GetInfo()
        {
            return "Сапфир выбери ... или ...\nЯ выберу один из двух вариантов";
        }
    }
}
