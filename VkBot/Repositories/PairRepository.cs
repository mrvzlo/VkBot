using System;
using System.Linq;
using VkBot.Entities;
using VkNet.Model;

namespace VkBot.Repositories
{
    public class PairRepository : BaseRepository<MessagePair>, IPairRepository
    {
        public PairRepository(AppDbContext dbContext) : base(dbContext) { }

        public void Save(string first, string second, int meaning)
        {
            var entity = Select().FirstOrDefault(x =>
                             x.First.ToLower() == first.ToLower() && x.Second.ToLower() == second.ToLower())
                         ?? new MessagePair {First = first, Second = second, Count = meaning };
            entity.Count++;
            InsertOrUpdate(entity);
        }

        public MessagePair Get(string first, int pos) => 
            GetAll(first).Skip(pos).FirstOrDefault();

        public IQueryable<MessagePair> GetAll(string first)
        {
            return Select().Where(x => first == null && x.Second.Length == 0 || x.First.ToLower() == first.ToLower());
        }

        public string GetRandom()
        {
            var query = Select().Select(x => x.First).Distinct();
            var pos = new Random(DateTime.Now.Millisecond).Next(query.Count());
            return query.Skip(pos).FirstOrDefault();
        }

        public bool Any() => Select().Any();
    }
}
