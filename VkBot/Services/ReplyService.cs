using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using VkBot.Communication;
using VkNet.Model;

namespace VkBot
{
    public class ReplyService : IReplyService
    {
        private readonly IMemoryService _memory;
        private readonly ITruthLieGame _truthLieGame;
        private readonly string _settingsPath;

        public ReplyService(IMemoryService memory, IConfiguration configuration, ITruthLieGame truthLieGame)
        {
            _memory = memory;
            _truthLieGame = truthLieGame;
            _settingsPath = configuration["Config:MemoryFile"];
        }

        public Response GetResponse(Message message)
        {
            _memory.SaveUserChat(message.ChatId, message.UserId);
            if (string.IsNullOrEmpty(message.Text))
                return null;

            var settings = Settings.Get(_settingsPath);
            var simplified = new SimplifiedMessage(message, _botNames);

            if (settings.Status == BotStatus.TruthOrLieGame)
                return _truthLieGame.GenerateResponse(simplified);

            var response = new Response(ResponseType.None);

            if (simplified.MustReply)
                response = ReplyOnCommand(simplified.Words, settings.GetUserStatus(message.FromId));

            response = response.Type switch
            {
                ResponseType.SettingChange when response.Setting != null => settings.Set(response.Setting.Value, response.Content),
                ResponseType.Status => SetupStatusResponse(response, settings),
                ResponseType.UserPick => SetupUserPickResponse(response, simplified.ChatId),
               _ => response
            };

            if (response.Type == ResponseType.None && !simplified.Tagged)
            {
                _memory.Save(simplified.Words, settings.LastWord);
                settings.LastWord = simplified.Words.LastOrDefault();
            }

            if (settings.Status == BotStatus.Talk)
                settings.Save(_settingsPath);

            if (response.Type != ResponseType.None)
                return response;

            if (settings.Status == BotStatus.Talk && (simplified.MustReply || settings.RandomReply()))
                return GenerateText(simplified);

            return response;
        }

        private Response GenerateText(SimplifiedMessage message)
        {
            var last = message.MustReply ? message.Words.LastOrDefault() : null;
            return new Response(ResponseType.Text) { Content = _memory.Generate(last) };
        }

        private Response ReplyOnCommand(List<string> src, UserStatus user)
        {
            var commandType = new BaseCommand().GetSubClass(src);
            return commandType?.GetResponse(src, user);
        }

        private Response SetupStatusResponse(Response response, Settings settings)
        {
            response.Content = string.Format(response.Content, _memory.GetPairCount(), settings.Frequency, settings.Status.ToString());
            return response;
        }

        private Response SetupUserPickResponse(Response response, long chatId)
        {
            response.Content = string.Format(response.Content, _memory.GetRandomUser(chatId));
            return response;
        }

        private readonly string[] _botNames = { "saphire", "сапфир" };
    }
}
