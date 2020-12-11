using System.Collections.Generic;

namespace VkBot
{
    public class NotFoundCommand : BaseCommand
    {
        public NotFoundCommand()
        {
            Priority = Priority.Lowest;
            Responses = new[] { "А?", "Чё?" };
        }
        public override string GetInfo() => "";

        protected override bool Match(List<string> _) => true;
    }
}
