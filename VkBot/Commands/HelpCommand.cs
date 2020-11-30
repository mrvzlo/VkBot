namespace VkBot
{
    public class HelpCommand : BaseCommand
    {
        public HelpCommand(BaseCommand parent) : base(parent)
        {
            Filters = new [] {"помощь", "команды", "help" };
            Responses = new[] {"Я могу ответить на ваш вопрос, если он подразмевает ответ да или нет.\n" +
                               "Например: 'Сапфир, скажи, будет ли завтра дождь?'\n" +
                               "Также вы можете попросить меня бросить кубик.\n" +
                               "Например: 'Сапфир, брось 2д20'"};
        }
    }
}
