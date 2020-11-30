namespace VkBot
{
    public class NotFoundCommand : BaseCommand
    {
        public NotFoundCommand(BaseCommand parent) : base(parent)
        {
            Responses = new[] { "А?", "Чё?" };
        }
    }
}
