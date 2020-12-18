using System;
using System.Collections.Generic;
using VkBot.Communication;

namespace VkBot
{
    public class ChanceCommand : BaseCommand
    {
        public ChanceCommand()
        {
            Filters = new[] { "chance", "шанс", "вероятность" };
        }

        public override Response GetResponse(List<string> src, UserStatus _)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var first = random.Next(101);
            var second = first == 100 ? 1 : random.Next(10);
            var response = new Response(ResponseType.Text) { Content = $"Вероятность этого {first}" };
            if (second == 0)
            {
                second = random.Next(99) + 1;
                response.Content += $".{second / 10}{second % 10}";
            }

            response.Content += "%";
            return response;
        }

        public override string GetInfo()
        {
            return "Сапфир шанс ...\nЯ определю точность этого события или факта";
        }
    }
}
