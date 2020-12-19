using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using VkBot.Communication;
using VkNet.Model;
using Status = VkBot.Communication.Status;

namespace VkBot
{
    public class ReplyService : IReplyService
    {
        private readonly IMemoryService _memory;
        private readonly string _settingsPath;

        public ReplyService(IMemoryService memory, IConfiguration configuration)
        {
            _memory = memory;
            _settingsPath = configuration["Config:MemoryFile"];
        }

        public Response GetResponse(Message message)
        {
            if (string.IsNullOrEmpty(message.Text))
                return null;

            var settings = Settings.Get(_settingsPath);
            var simplified = Simplify(message.Text);
            var response = new Response(ResponseType.None);
            var mustReply = simplified.Tagged || message.PeerId == message.FromId;

            if (mustReply)
            {
                response = ReplyOnCommand(simplified.Words, settings.GetUserStatus(message.FromId));

                if (response.Type == ResponseType.SettingChange && response.Setting != null)
                    response = settings.Set(response.Setting.Value, response.Content);
            }

            if (response.Type == ResponseType.None)
            {
                _memory.Save(simplified.Words, settings.LastWord);
                settings.LastWord = simplified.Words.LastOrDefault();
            }


            settings.Save(_settingsPath);

            if (response.Type != ResponseType.None)
                return response;

            var generatedText = GenerateText(settings, 
                mustReply ? simplified.Words.LastOrDefault() : "",
                mustReply ? 100 : settings.Frequency);

            if (!string.IsNullOrEmpty(generatedText))
                response = new Response(ResponseType.Text) { Content = generatedText };

            return response;
        }

        private string GenerateText(Settings settings, string last, int chance)
        {
            if (settings.Status == Status.Mute)
                return null;
            var speak = new Random(DateTime.Now.Millisecond).Next(100 - chance) == 0;
            return speak ? _memory.Generate(last) : null;
        }

        private Response ReplyOnCommand(List<string> src, UserStatus user)
        {
            var commandType = new BaseCommand().GetSubClass(src);
            return commandType?.GetResponse(src, user);
        }
        
        private SimplifiedMessage Simplify(string src)
        {
            src = RemoveExtraSymbols(src);
            var simplified = new SimplifiedMessage
            {
                Words = src.Split(' ').Where(x => x.Any()).ToList()
            };
            var first = simplified.Words.First();
            simplified.Tagged = simplified.Words.Any() && _botNames.Any(s => first.Contains(s, StringComparison.InvariantCultureIgnoreCase));
            if (simplified.Tagged)
                simplified.Words = simplified.Words.Skip(1).ToList();

            return simplified;
        }
        private string RemoveExtraSymbols(string src)
        {
            var pattern = new Regex("[ ;,\t\r ]|[\n]{2}");
            src = pattern.Replace(src, " ");
            return src;
        }

        private readonly string[] _botNames = { "saphire", "сапфир" };
    }
}
