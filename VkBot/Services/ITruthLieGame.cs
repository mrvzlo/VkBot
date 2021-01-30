using VkBot.Communication;
using VkNet.Model;

namespace VkBot
{
    public interface ITruthLieGame
    {
        Response GenerateResponse(SimplifiedMessage simplified);
    }
}
