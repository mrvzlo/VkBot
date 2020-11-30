using System;
using System.Collections.Generic;
using System.Linq;

namespace VkBot
{
    public class BaseCommand
    {
        protected readonly IMemoryService Memory;

        public string[] Filters;
        public string[] Responses;
        protected readonly List<string> _src;

        protected BaseCommand(BaseCommand parent)
        {
            Memory = parent.Memory;
            _src = parent._src;
        }

        public BaseCommand(IMemoryService memory, List<string> src)
        {
            Memory = memory;
            _src = src;
        }

        public BaseCommand GetSubClass()
        {
            if (!_src.Any())
                return new PingCommand(this);

            var tag = _src.First();

            BaseCommand command = new MagicBallCommand(this);
            if (command.Match(tag))
                return command;

            command = new DiceCommand(this);
            if (command.Match(tag))
                return command;
            
            command = new StartCommand(this);
            if (command.Match(tag))
                return command;

            command = new HelpCommand(this);
            if (command.Match(tag))
                return command;

            command = new InfoCommand(this);
            if (command.Match(tag))
                return command;

            return new NotFoundCommand(this);
        }

        private bool Match(string src) => Filters.Any(src.Contains);

        public virtual string GetResponse()
        {
            var rand = new Random();
            return Responses[rand.Next(Responses.Length)];
        }
    }
}
