using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.ExtensionMethods.Internal;
using VkBot.Entities;
using VkBot.Repositories;

namespace VkBot
{
    public class MemoryService : IMemoryService
    {
        private readonly IPairRepository _pairRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IUserChatPairRepository _userChatPairRepository;

        public MemoryService(ISizeRepository sizeRepository, IPairRepository pairRepository, IUserChatPairRepository userChatPairRepository)
        {
            _sizeRepository = sizeRepository;
            _pairRepository = pairRepository;
            _userChatPairRepository = userChatPairRepository;
        }

        public void Save(List<string> src, string previous)
        {
            var first = true;
            foreach (var t in src.Where(t => !string.IsNullOrEmpty(previous) && !IsUrl(t)))
            {
                if (t == "@all")
                    continue;
                _pairRepository.Save(previous, t, first ? 3 : 10);
                previous = t;
                first = false;
            }

            _sizeRepository.Save(src.Count);
        }

        public string Generate(string start, int? size = null)
        {
            if (!_pairRepository.Any())
                return "Я пока ничего не знаю";

            if (string.IsNullOrEmpty(start))
                start = _pairRepository.GetRandom();

            var result = "";
            var random = new Random(DateTime.Now.Millisecond);
            size ??= random.Next(3) + random.Next(3) + random.Next(3) + 1;

            var i = 0;
            for (; i < size; i++)
            {
                var word = FindPair(start);
                if (word == null) break;
                start = word.Second.ToLower();
                if (i == 0)
                    result = word.First;
                result += $" {start}";
            }

            if (i == 0)
                result = $"{start}... неизвестное мне слово";

            return result;
        }

        public void SaveUserChat(long? chatId, long? userId)
        {
            if (userId == null || chatId == null)
                return;
            var pair = new UserChatPair{ ChatId = chatId.Value, UserId = userId.Value};
            _userChatPairRepository.InsertOrUpdate(pair);
        }

        public long GetRandomUser(long chatId)
        {
            return _userChatPairRepository.GetRandom(chatId).UserId;
        }

        private MessagePair FindPair(string start)
        {
            var variants = _pairRepository.GetAll(start).Select(x => x.Count).ToList();
            var sum = variants.Sum();
            var rand = new Random(DateTime.Now.Millisecond).Next(sum);
            var i = 0;
            for (; i < variants.Count; i++)
            {
                rand -= variants[i];
                if (rand <= 0) break;
            }

            return _pairRepository.Get(start, i);
        }

        public int GetPairCount() => _pairRepository.GetAll().Count();

        private bool IsUrl(string src) => src.Contains("http");
    }
}
