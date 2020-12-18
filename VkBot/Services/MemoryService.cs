using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
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
            foreach (var t in src)
            {
                if (string.IsNullOrEmpty(previous)) continue;
                var pair = new MessagePair { First = previous, Second = t };
                _pairRepository.Save(pair);
                previous = t;
            }

            _sizeRepository.Save(src.Count);
        }

        public string Generate(string start, int? size = null)
        {
            var result = "";
            for (var i = 0; i < size; i++)
            {
                var word = GenerateWord(start);
                start = word;
                result += $"{word} ";
            }

            return result;
        }

        private string GenerateWord(string start)
        {
            var variants = _pairRepository.GetAll(start).Select(x => x.Counts).ToList();
            var sum = variants.Sum();
            var rand = new Random(DateTime.Now.Millisecond).Next(sum);
            var i = 0;
            for (; i < variants.Count; i++)
            {
                rand -= variants[i];
                if (rand <= 0) break;
            }

            return _pairRepository.Get(start, i).Second;
        }
    }
}
