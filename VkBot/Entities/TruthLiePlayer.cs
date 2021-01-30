using VkBot.Communication;

namespace VkBot.Entities
{
    public class TruthLiePlayer : BaseEntity
    {
        public long VkId { get; set; }
        public string FirstTruth { get; set; }
        public string SecondTruth { get; set; }
        public string Lie { get; set; }
        public TruthLieStatus Status { get; set; }
        public bool Leading { get; set; }
        public int Points { get; set; }
        public int Choice { get; set; }
    }
}
