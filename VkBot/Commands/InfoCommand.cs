namespace VkBot
{
    public class InfoCommand : BaseCommand
    {
        public InfoCommand(BaseCommand parent) : base(parent)
        {
            Filters = new[] { "статус", "status" };
            var remembered = Memory.GetMemorizedCount();
            Responses = new[] { $"Я уже записала {remembered} строк." };
        }
    }
}
