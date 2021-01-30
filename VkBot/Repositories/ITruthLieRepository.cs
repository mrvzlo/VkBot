using VkBot.Entities;

namespace VkBot.Repositories
{
    public interface ITruthLieRepository : IBaseRepository<TruthLiePlayer>
    {
        TruthLiePlayer Get(long vkId);
        TruthLiePlayer GetLeading();
    }
}
