namespace VkBot.Communication
{
    public class Response
    {
        public ResponseType Type { get; set; }
        public string Content { get; set; }
        public SettingType? Setting { get; set; }

        public Response(ResponseType type)
        {
            Type = type;
        }
    }
}
