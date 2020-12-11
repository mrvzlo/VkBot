using System.Collections.Generic;
using System.Linq;

namespace VkBot
{
    public class PingCommand : BaseCommand
    {
        public PingCommand()
        {
            Priority = VkBot.Priority.High;
            Responses = new[] { "Я тут", "Да-да?", "Слушаю" };
        }

        protected override bool Match(List<string> src) => !src.Any();
    }
}
