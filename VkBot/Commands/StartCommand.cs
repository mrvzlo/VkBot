﻿namespace VkBot
{
    public class StartCommand : BaseCommand
    {
        public StartCommand(BaseCommand parent) : base(parent)
        {
            Filters = new [] {"старт", "start", "запуск"};
            Responses = new[] {"Вас приветствует Система Аналитического Поэтапного Формирования Искусственного Разговора, " +
                              "но можете звать меня просто Сапфир или saphire. Я сижу и слушаю, что вы говорите." +
                              "Чтобы узнать доступные команды скажите 'Сапфир команды'"};
        }
    }
}
