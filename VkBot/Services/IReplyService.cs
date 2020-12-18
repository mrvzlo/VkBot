using VkBot.Communication;
using VkNet.Model;

namespace VkBot
{
    public interface IReplyService
    {
        Response Generate(Message message);
    }
}
