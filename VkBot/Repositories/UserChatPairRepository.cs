using System;
using System.Linq;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public class UserChatPairRepository : BaseRepository<UserChatPair>, IUserChatPairRepository
    {
        public UserChatPairRepository(AppDbContext context) : base(context)
        {
        }

        public UserChatPair GetRandom(long chatId)
        {
            var users = Select().Where(x => x.ChatId == chatId);
            var random = new Random(DateTime.Now.Millisecond).Next(users.Count());
            return users.Skip(random).First();
        }
    }
}
