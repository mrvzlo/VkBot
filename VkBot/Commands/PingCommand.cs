namespace VkBot
{
    public class PingCommand : BaseCommand
    {
        public PingCommand(BaseCommand parent) : base(parent)
        {
            Responses = new[] { "Я тут", "Да да?", "Слушаю" };
        }
    }
}
