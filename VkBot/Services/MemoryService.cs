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

        public MemoryService(ISizeRepository sizeRepository, IPairRepository pairRepository)
        {
            _sizeRepository = sizeRepository;
            _pairRepository = pairRepository;
        }

        public void Save(List<string> src, string previous)
        {
            var first = true;
            foreach (var t in src.Where(t => !string.IsNullOrEmpty(previous)))
            {
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

            var result = start;
            size ??= _sizeRepository.GetAverage();

            for (var i = 0; i < size; i++)
            {
                var word = FindPair(start);
                if (word == null) break;
                start = word.Second;
                result += $" {start}";
            }

            return result[0].ToUpper() + result.Substring(1).ToLower();
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
    }
}
