using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VkBot.Communication;

namespace VkBot
{
    public class SettingChangeCommand : BaseCommand
    {
        public SettingChangeCommand()
        {
            Replies = new[] { "Готово", "Сделала" };
            Filters = new[] { "установи", "set" };
        }

        public override Response GetResponse(List<string> src, UserStatus user)
        {
            if (user != UserStatus.Admin)
                return new Response(ResponseType.Text) { Content = "Недоступная команда" };
            if (src.Count != 3)
                return new Response(ResponseType.Text) { Content = "Неверное количество аргументов" };
            
            var response = base.GetResponse(src, user);
            var target = src[1];
            if (target.Contains("админ"))
                response.Setting = SettingType.Admin;
            else if (target.Contains("частот"))
                response.Setting = SettingType.Frequency;
            else if (target.Contains("статус"))
                response.Setting = SettingType.Status;
            else
                return new Response(ResponseType.Text) { Content = "Неизвестная настройка" };

            response.Content = src.Last();
            response.Type = ResponseType.SettingChange;
            return response;
        }
    }
}
