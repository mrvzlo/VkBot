using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using VkBot.Communication;
using VkNet.Model;

namespace VkBot
{
    public class ReplyService : IReplyService
    {
        private bool _isTagged;
        private List<string> _splittedText;
        private readonly IMemoryService _memory;
        private readonly IConfiguration _configuration;

        public ReplyService(IMemoryService memory, IConfiguration configuration)
        {
            _memory = memory;
            _configuration = configuration;
        }

        public Response Generate(Message message)
        {
            if (string.IsNullOrEmpty(message.Text))
                return null;

            var settings = GetSettings();
            var formatted = Split(Simplify(message.Text));
            var isPrivate = message.PeerId == message.FromId;
            _isTagged |= isPrivate;

            if (_isTagged)
            {
                var response = ReplyOnCommand(formatted, settings.GetUserStatus(message.FromId));
                if (response.Type != ResponseType.None)
                    return response;
            }

            var speak = new Random(DateTime.Now.Millisecond).Next(settings.Frequency) == 0;

            if (_isTagged) //todo
            {
                var last = _isTagged ? formatted.LastOrDefault() : "";
                var response = new Response(ResponseType.Text) {Content = _memory.Generate(last) };
                return response;
            }

            //todo _memory.Save(formatted, settings.LastWord);

            return new Response(ResponseType.None);
        }

        private Response ReplyOnCommand(List<string> src, UserStatus user)
        {
            var commandType = new BaseCommand().GetSubClass(src);
            return commandType?.GetResponse(src, user);
        }

        private string Simplify(string src)
        {
            var pattern = new Regex("[ ;,\t\r ]|[\n]{2}");
            src = pattern.Replace(src, " ");
            return src;
        }

        private List<string> Split(string src)
        {
            var splitted = src.Split(' ').Where(x => x.Any()).ToList();
            var first = splitted.First();
            _isTagged = splitted.Any() && BotNames.Any(s => first.Contains(s, StringComparison.InvariantCultureIgnoreCase));
            if (_isTagged)
                splitted = splitted.Skip(1).ToList();

            return splitted;
        }

        private readonly string[] BotNames = { "saphire", "сапфир" };

        private Settings GetSettings()
        {
            var path = _configuration["Config:MemoryFile"];
            var str = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(str);
        }
    }
}
