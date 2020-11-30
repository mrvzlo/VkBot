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

            var isPrivate = message.PeerId == message.FromId;
            if (isPrivate)
                return FuckYou;

            var simplified = Simplify(message.Text);
            var formatted = Split(simplified);

            if (_isTagged)
                return ReplyOnCommand(formatted);

            _memory.Save(simplified);
            return null;
        }

        private string ReplyOnCommand(List<string> src)
        {
            var commandType = new BaseCommand(_memory, src).GetSubClass();
            return commandType?.GetResponse();
        }

        private string Simplify(string src)
        {
            var pattern = new Regex("[ ;,\t\r ]|[\n]{2}");
            var spaces = new Regex("[ ]{2,}");
            src = pattern.Replace(src.ToLower(), " ");
            return spaces.Replace(src, "");
        }

        private List<string> Split(string src)
        {
            var splitted = src.Split(' ').ToList();
            var first = splitted.First();
            _isTagged = splitted.Any() && BotNames.Any(first.Contains);
            if (_isTagged)
                splitted = splitted.Skip(1).ToList();

            return splitted.Where(x => x.Any()).ToList();
        }

        private readonly string[] BotNames = { "saphire", "сапфир" };

        private const string FuckYou = "Пошёл нахуй";
    }
}
