using System.Linq;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public interface IPairRepository
    {
        public void Save(MessagePair pair);
        public MessagePair Get(string first, int pos);
        public IQueryable<MessagePair> GetAll(string first);
    }
}
