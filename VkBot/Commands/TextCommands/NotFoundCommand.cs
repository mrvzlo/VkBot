using System.Collections.Generic;
using VkBot.Communication;

namespace VkBot
{
    public class NotFoundCommand : BaseCommand
    {
        public NotFoundCommand()
        {
            Priority = Priority.Lowest;
            Replies = new[] { "А?", "Чё?" };
        }
        public override string GetInfo() => "";

        protected override bool Match(List<string> _) => true;

        public override Response GetResponse(List<string> src, UserStatus _)
        {
            var response = base.GetResponse(src, _);
            response.Type = ResponseType.None;
            return response;
        }
    }
}
