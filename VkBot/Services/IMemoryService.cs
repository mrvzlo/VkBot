using System.Collections.Generic;

namespace VkBot
{
    public interface IMemoryService
    {
        void Save(List<string> src, string previous);
        string Generate(string start, int? size = null);
    }
}
