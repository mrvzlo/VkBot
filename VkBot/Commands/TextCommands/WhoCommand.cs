using System.Collections.Generic;
using System.Linq;
using VkBot.Communication;

namespace VkBot
{
    public class WhoCommand : BaseCommand
    {
        public WhoCommand()
        {
            Filters = new[] { "кто" };
        }

        public override Response GetResponse(List<string> src, UserStatus user)
        {
            var post = string.Join(" ", src.Skip(1)).Replace("?", "");
            var response = new Response(ResponseType.UserPick) {Content = "[id{0}|Этот человек]" + ReplacePronouns(post) };
            return response;
        }

        private string ReplacePronouns(string src)
        {
            const string temp = "♪X♪";
            foreach (var (item1, item2) in Pairs)
            {
                src = src.Replace(item1, temp);
                src = src.Replace(item2, item1);
                src = src.Replace(temp, item2);
            }

            return src;
        }

        private List<(string, string)> Pairs = new List<(string, string)>
        {
            ("меня", "тебя"),
            ("мне", "тебе"),
            ("мою", "твою")
        };
    }
}
