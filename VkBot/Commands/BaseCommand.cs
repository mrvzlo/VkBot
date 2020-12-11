using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VkBot
{
    public class BaseCommand
    {
        public bool Enabled { get; set; }
        public Priority Priority { get; set; }

        public string[] Filters { get; set; }
        public string[] Responses;

        public BaseCommand()
        {
            Enabled = true;
            Priority = Priority.Medium;
        }

        public BaseCommand GetSubClass(List<string> src) => 
            GetAllCommands().FirstOrDefault(command => command.Match(src));

        protected virtual bool Match(List<string> src) =>
            src.Any() && Filters.Any(s => src.First().Contains(s, StringComparison.InvariantCultureIgnoreCase));

        public virtual string GetResponse(List<string> src)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            return Responses[rand.Next(Responses.Length)];
        }

        public virtual string GetInfo() => string.Empty;

        protected List<BaseCommand> GetAllCommands()
        {
            var list = Assembly.GetAssembly(typeof(BaseCommand)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseCommand))).AsQueryable()
                .Select(x => (BaseCommand)Activator.CreateInstance(x))
                .Where(x => x != null && x.Enabled).OrderBy(x => x.Priority).ToList();

            return list;
        }
    }
}
