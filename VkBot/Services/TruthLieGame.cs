using System;
using VkBot.Communication;
using VkBot.Entities;
using VkBot.Repositories;
using VkNet.Model;

namespace VkBot
{
    public class TruthLieGame : ITruthLieGame
    {
        private readonly ITruthLieRepository _truthLieRepository;

        public TruthLieGame(ITruthLieRepository memberRepository)
        {
            _truthLieRepository = memberRepository;
        }

        public Response GenerateResponse(SimplifiedMessage simplified)
        {
            var leading = _truthLieRepository.GetLeading();
            var member = _truthLieRepository.Get(simplified.Author);
            if (member == null)
            {
                member = new TruthLiePlayer{VkId = simplified.Author, Status = TruthLieStatus.WaitingStart};
                _truthLieRepository.InsertOrUpdate(member);
            }

            switch (leading?.Status)
            {
                case null:
                    break;
                case TruthLieStatus.FirstTruth:
                case TruthLieStatus.SecondTruth:
                case TruthLieStatus.Lie:
                    break;
                case TruthLieStatus.WaitingFinish:
                    break;
            }

            return new Response(ResponseType.None);
        }
        /*
        public Response StartGame()
        {

        }

        public Response StartGame()
        {

        }

        public Response StartGame()
        {

        }*/
    }
}
