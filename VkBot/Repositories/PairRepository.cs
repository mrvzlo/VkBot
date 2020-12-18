using System.Linq;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public class PairRepository : IPairRepository
    {
        public void Save(MessagePair pair)
        {
            //todo
        }

        public MessagePair Get(string first, int pos)
        {
            //todo
            return null;
        }

        public IQueryable<MessagePair> GetAll(string first)
        {
            //todo
            return null;
        }
    }
}
