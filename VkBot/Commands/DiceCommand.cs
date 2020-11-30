using System;
using System.Collections.Generic;
using System.Linq;

namespace VkBot
{
    public class DiceCommand : BaseCommand
    {
        public DiceCommand(BaseCommand parent) : base(parent)
        {
            Filters = new[] {"roll", "кинь", "брось"};
        }

        public override string GetResponse()
        {
            var value = _src.Skip(1).FirstOrDefault();
            if (value == null)
                return "А что бросать то?";
            value = value.Replace('д', 'd');
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
    }
}
