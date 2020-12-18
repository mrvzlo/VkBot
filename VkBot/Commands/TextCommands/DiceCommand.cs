using System;
using System.Collections.Generic;
using System.Linq;
using VkBot.Communication;

namespace VkBot
{
    public class DiceCommand : BaseCommand
    {
        public DiceCommand()
        {
            Filters = new[] { "roll", "кинь", "брось" };
        }

        public override Response GetResponse(List<string> src, UserStatus _)
        {
            var response = new Response(ResponseType.Text) { Content = "А что бросать то?" };
            var value = src.Skip(1).FirstOrDefault();
            if (value == null)
                return response;
            value = value.ToLower().Replace('д', 'd');
            if (!value.Contains('d'))
                return response;
            if (value[0] == 'd')
                value = $"1{value}";

            var splitted = value.Split('d');
            int.TryParse(splitted.FirstOrDefault(), out var count);
            int.TryParse(splitted.LastOrDefault(), out var max);

            if (count <= 0 || max <= 0)
            {
                response.Content = "Чёт я не поняла";
                return response;
            }

            var result = 0;
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < count; i++)
                result += random.Next(max) + 1;

            response.Content = result.ToString();
            return response;
        }

        public override string GetInfo()
        {
            return "Сапфир брось XдY\nЯ брошу X раз кубик Y и покажу ответ";
        }
    }
}
