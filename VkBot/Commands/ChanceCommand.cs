using System;
using System.Collections.Generic;

namespace VkBot
{
    public class ChanceCommand : BaseCommand
    {
        public ChanceCommand()
        {
            Filters = new[] { "chance", "шанс", "вероятность" };
        }

        public override string GetResponse(List<string> src)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var first = random.Next(101);
            var second = first == 100 ? 1 : random.Next(10);
            var result = $"Вероятность этого {first}";
            if (second == 0)
            {
                second = random.Next(99) + 1;
                result += $".{second / 10}{second % 10}";
            }

            return $"{result}%";
        }

        public override string GetInfo()
        {
            return "Сапфир шанс ...\nЯ определю точность этого события или факта";
        }
    }
}
