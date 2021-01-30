using VkBot.Entities;

namespace VkBot.Repositories
{
    public interface IUserChatPairRepository : IBaseRepository<UserChatPair>
    {
        UserChatPair GetRandom(long chatId);
    }
}
