using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVkApi _vkApi;

        public CallbackController(IConfiguration configuration, IVkApi vkApi)
        {
            _configuration = configuration;
            _vkApi = vkApi;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            switch (updates.Type)
            {
                case "confirmation":
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                    Reply(updates.Object);
                    break;
            }

            return Ok("ok");
        }

        private void Reply(JsonElement obj)
        {
            var str = obj.ToString();
            var msg = JsonConvert.DeserializeObject<MessageObject>(str).Message;
            
            SendText(msg.Text, msg.PeerId, msg.ChatId);
        }

        private long SendText(string text, long? receiver, long? chat)
        {
            return _vkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = receiver,
                Message = text,
                ChatId = chat
            });
        }
    }
}
