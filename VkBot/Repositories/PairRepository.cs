using System;
using System.Linq;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public class PairRepository : BaseRepository<MessagePair>, IPairRepository
    {
        public PairRepository(AppDbContext dbContext) : base(dbContext) { }

        public void Save(string first, string second, int meaning)
        {
            if (string.Equals(first, second, StringComparison.OrdinalIgnoreCase))
                meaning /= 2;

            var entity = Select().FirstOrDefault(x => x.First.ToLower() == first && x.Second.ToLower() == second)
                         ?? new MessagePair { First = first, Second = second, Count = meaning };
            entity.Count++;
            InsertOrUpdate(entity);
        }

        public MessagePair Get(string first, int pos) =>
            GetAll(first).Skip(pos).FirstOrDefault();

        public IQueryable<MessagePair> GetAll(string first = null)
        {
            var query = Select();
            if (string.IsNullOrEmpty(first)) 
                return query;

            query = query.Where(x => x.First.ToLower() == first);
            if (query.Any()) 
                return query;

            query = query.Where(x => x.Second.Length == 0 || x.First.ToLower().Contains(first));
            return query;
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
