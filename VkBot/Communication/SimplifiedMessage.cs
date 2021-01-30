using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VkNet.Model;

namespace VkBot.Communication
{
    public class SimplifiedMessage
    {
        public bool Tagged { get; }
        public bool IsPrivate { get; }
        public long Author { get; set; }
        public long ChatId { get; set; }
        public List<string> Words { get; }
        public bool MustReply => Tagged || IsPrivate;

        public SimplifiedMessage(Message message, string[] tagNames)
        {
            Author = message.FromId ?? 0;
            ChatId = message.ChatId ?? 0;
            IsPrivate = message.PeerId == message.FromId;
            message.Text = RemoveExtraSymbols(message.Text);
            Words = message.Text.Split(' ').Where(x => x.Any()).ToList();
            var first = Words.First();
            Tagged = Words.Any() && tagNames.Any(s => first.Contains(s, StringComparison.InvariantCultureIgnoreCase));
            if (Tagged)
                Words = Words.Skip(1).ToList();
        }

        private string RemoveExtraSymbols(string src)
        {
            var pattern = new Regex("[ ;,\t\r ]|[\n]{2}");
            src = pattern.Replace(src, " ");
            return src;
        }
    }
}
