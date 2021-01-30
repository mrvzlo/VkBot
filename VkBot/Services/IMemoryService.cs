using System.Collections.Generic;

namespace VkBot
{
    public interface IMemoryService
    {
        void Save(List<string> src, string previous);
        string Generate(string start, int? size = null);
        int GetPairCount();

        void SaveUserChat(long? chatId, long? userId);
        long GetRandomUser(long chatId);
    }
}
