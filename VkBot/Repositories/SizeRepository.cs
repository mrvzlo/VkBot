using System;
using System.Linq;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public class SizeRepository : BaseRepository<MessageSize>, ISizeRepository
    {
        public SizeRepository(AppDbContext dbContext) : base(dbContext) { }

        public void Save(int size) => base.InsertOrUpdate(new MessageSize { Size = size });

        public int GetAverage() => Select().Any() ? (int)Math.Ceiling(Select().Average(x => x.Size)) : 5;
    }
}
