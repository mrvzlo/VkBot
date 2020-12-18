namespace VkBot.Communication
{
    public class Settings
    {
        public long Admin { get; set; }
        public Status Status { get; set; }
        public int Frequency { get; set; }
        public string LastWord { get; set; }

        public UserStatus GetUserStatus(long? id) => id == null ? UserStatus.Simple
            : id.Value == Admin ? UserStatus.Admin : UserStatus.Simple;
    }
}
