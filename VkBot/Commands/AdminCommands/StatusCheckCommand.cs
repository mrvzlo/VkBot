using System.Collections.Generic;
using VkBot.Communication;

namespace VkBot
{
    public class StatusCheckCommand : BaseCommand
    { 
        public StatusCheckCommand()
        {
            Filters = new[] { "статус", "status" };
            Replies = new[] { "Сохранено слов: {0}\nВыбранный режим: {2}\nЧастота ответов: {1}" };
        }

        public override Response GetResponse(List<string> src, UserStatus user)
        {
            var response = base.GetResponse(src, user);
            response.Type = ResponseType.Status;
            return response;
        }
    }
}
