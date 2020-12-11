using System;
using System.Collections.Generic;
using System.Linq;

namespace VkBot
{
    public class HelpCommand : BaseCommand
    {
        public HelpCommand()
        {
            Filters = new [] {"помощь", "команды", "help" };
        }

        public override string GetInfo()
        {
            return "Сапфир помощь\nЕсли забыл какую-то из команд, спроси, и я напомню";
        }

        public override string GetResponse(List<string> src)
        {
            var i = 0;
            return GetAllCommands().Where(x => !string.IsNullOrEmpty(x.GetInfo()))
                .Aggregate("Список команд:\n", (current, command) => $"{current}\n{++i}) {command.GetInfo()}\nТриггеры: {string.Join(", ", command.Filters)}\n");
        }
    }
}
