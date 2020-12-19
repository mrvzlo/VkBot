using System.Linq;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public interface IPairRepository
    {
        public void Save(string first, string second, int meaning);
        public string GetRandom();
        public MessagePair Get(string first, int pos);
        public IQueryable<MessagePair> GetAll(string first);
        public bool Any();
    }
}
