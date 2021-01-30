using System.Collections.Generic;
using System.Linq;

namespace VkBot
{
    public class MagicBallCommand : BaseCommand
    {
        public MagicBallCommand()
        {
            Priority = Priority.Low;
            Filters = new []{"ответь" };
            Replies = new[]
            {
                "Бесспорно",
                "Уже предрешено",
                "Никаких сомнений",
                "Определённо",
                "Можешь быть уверен в этом",
                "Мне кажется да",
                "Вероятнее всего",
                "Хорошие перспективы",
                "Да",
                "Духи с этим согласны",
                "Пока не ясно",
                "Это узнается позже",
                "Лучше не рассказывать",
                "Сейчас нельзя понять",
                "Уточни вопрос",
                "Даже не думай",
                "Зачем ты вообще такое спрашиваешь?",
                "Нет",
                "Судя по предварительным данным - нет",
                "Перспективы не очень хорошие",
                "Сомнительно"
            };
        }

        protected override bool Match(List<string> src) => src.Any() && (src.Last().Contains('?') || base.Match(src));

        public override string GetInfo()
        {
            return "Сапфир скажи ...?\nЯ отвечу на вопрос да, нет или не знаю";
        }
    }
}
