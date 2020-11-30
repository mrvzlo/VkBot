using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IReplyService _replyService;
        private readonly IVkApi _vkApi;

        public CallbackController(IConfiguration configuration, IVkApi vkApi, IReplyService replyService)
        {
            _configuration = configuration;
            _vkApi = vkApi;
            _replyService = replyService;
        }

        [HttpGet]
        public IActionResult Check(string msg)
        {
            var response = _replyService.Generate(new Message { Text = msg, PeerId = 1, FromId = 0});
            return new JsonResult(response);
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
            var result = _replyService.Generate(msg);
            SendText(result, msg.PeerId, msg.ChatId);
        }

        private long SendText(string text, long? receiver, long? chat)
        {
            if (string.IsNullOrEmpty(text))
                return 0;
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
