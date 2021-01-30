using System;
using System.Collections.Generic;
using System.Linq;
using VkBot.Communication;

namespace VkBot
{
    public class ChoiceCommand : BaseCommand
    {

        public ChoiceCommand()
        {
            Filters = new[] { "выбери", "или", "либо" };
            Replies = new[] { "Я думаю, что лучше \n", "Наверное всё таки \n" };
        }

        public override Response GetResponse(List<string> src, UserStatus _)
        {
            var response = new Response(ResponseType.Text);
            var keys = new[] { "или", "либо" };
            var key = src.FirstOrDefault(x => keys.Any(s => x.Equals(s, StringComparison.InvariantCultureIgnoreCase)));
            if (key == null)
            {
                response.Content = "Это иллюзия выбора";
                return response;
            }

            var index = src.LastIndexOf(key);
            var random = new Random(DateTime.Now.Millisecond);
            var part = random.Next(2);

            response.Content = Replies[random.Next(Replies.Length)];

            if (part == 0)
                for (var i = 1; i < index; i++)
                    response.Content += $"{src[i]} ";
            else
                for (var i = index + 1; i < src.Count; i++)
                    response.Content += $"{src[i]} ";

            return response;
        }

        public override string GetInfo()
        {
            return "Сапфир выбери ... или ...\nЯ выберу один из двух вариантов";
        }
    }
}
