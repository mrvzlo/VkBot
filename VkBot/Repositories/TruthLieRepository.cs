using System.Linq;
using VkBot.Communication;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public class TruthLieRepository : BaseRepository<TruthLiePlayer>, ITruthLieRepository
    {
        public TruthLieRepository(AppDbContext dbContext) : base(dbContext) { }
        
        public TruthLiePlayer Get(long vkId)
        {
            return Select().FirstOrDefault(x => x.VkId == vkId);
        }

        public TruthLiePlayer GetLeading()
        {
            var leading = Select().OrderByDescending(x => x.Status).FirstOrDefault();
            return leading?.Status == TruthLieStatus.WaitingStart ? null : leading;
        }
    }
}
