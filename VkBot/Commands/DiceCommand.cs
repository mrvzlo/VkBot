using System;
using System.Collections.Generic;
using System.Linq;

namespace VkBot
{
    public class DiceCommand : BaseCommand
    {
        public DiceCommand()
        {
            Filters = new[] {"roll", "кинь", "брось"};
        }

        public override string GetResponse(List<string> src)
        {
            var value = src.Skip(1).FirstOrDefault();
            if (value == null)
                return "А что бросать то?";
            value = value.ToLower().Replace('д', 'd');
            if (!value.Contains('d'))
                return "А что бросать то?";
            if (value[0] == 'd')
                value = $"1{value}";

            List<int> pair;

            try
            {
                pair = value.Split('d').Select(int.Parse).ToList();
            }
            catch 
            {
                return "Чёт я не поняла";
            }

            if(pair.Count != 2)
                return "Чёт я не поняла";

            var result = 0;
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < pair[0]; i++) 
                result += random.Next(pair[1]) + 1;

            return result.ToString();
        }

        public override string GetInfo()
        {
            return "Сапфир брось XдY\nЯ брошу X раз кубик Y и покажу ответ";
        }
    }
}
