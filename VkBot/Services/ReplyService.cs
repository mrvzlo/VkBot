using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VkNet.Model;

namespace VkBot
{
    public class ReplyService : IReplyService
    {
        private bool _isTagged;
        private List<string> _splittedText;
        private IMemoryService _memory;

        public ReplyService(IMemoryService memory)
        {
            _memory = memory;
        }

        public string Generate(Message message)
        {
            if (string.IsNullOrEmpty(message.Text))
                return null;
            
            var formatted = Split(Simplify(message.Text));
            
            var isPrivate = message.PeerId == message.FromId;

            if (_isTagged || isPrivate)
                return ReplyOnCommand(formatted);

            _memory.Save(string.Join(" ", formatted));
            return null;
        }

        private string ReplyOnCommand(List<string> src)
        {
            var commandType = new BaseCommand().GetSubClass(src);
            return commandType?.GetResponse(src);
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

        private const string FuckYou = "Пошёл нахуй";
    }
}
