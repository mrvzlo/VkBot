﻿using System.Collections.Generic;

namespace VkBot
{
    public class MagicBallCommand : BaseCommand
    {
        public MagicBallCommand(BaseCommand parent) : base(parent)
        {
            Filters = new []{"ответь", "скажи" };
            Responses = new[]
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
    }
}
