using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VkBot.Communication;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace VkBot
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
            var settings = Settings.Get(_configuration["Config:MemoryFile"]);
            var response = _replyService.GetResponse(
                new Message { Text = msg, PeerId = settings.Admin, ChatId = settings.Admin, FromId = 0 });
            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Callback([FromBody] Updates updates)
        {
            switch (updates.Type)
            {
                case "confirmation":
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                    await Reply(updates.Object);
                    break;
            }

            return Ok("ok");
        }

        private async Task Reply(JsonElement obj)
        {
            var settings = Settings.Get(_configuration["Config:MemoryFile"]);
            if (settings.Status == BotStatus.Mute)
                return;

            var str = obj.ToString();
            var msg = JsonConvert.DeserializeObject<MessageObject>(str).Message;

            try
            {
                var result = _replyService.GetResponse(msg);
                if (result == null) return;

                switch (result.Type)
                {
                    case ResponseType.Image:
                        await SendPhoto(result.Content, msg);
                        return;
                    default:
                        await SendMessage(result.Content, msg);
                        return;
                }
            }
            catch (Exception e)
            {
                if (msg.FromId != settings.Admin)
                    return;
                var error = e.Message + "\n" + e.InnerException?.Message;
                if (msg.PeerId == settings.Admin)
                    error += "\n" + e.StackTrace;
                await SendMessage(error, msg);
            }
        }

        private async Task<long> SendMessage(string text, Message parent, List<MediaAttachment> attachments = null)
        {
            if (string.IsNullOrEmpty(text) && attachments == null)
                return 0;

            var parameters = new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = parent.PeerId,
                ChatId = parent.ChatId,
                Message = text == null ? "" : $"{text[0].ToUpper()}{text.Substring(1)}",
                Attachments = attachments
            };

            return await _vkApi.Messages.SendAsync(parameters);
        }

        private async Task<long> SendPhoto(string url, Message parent)
        {
            var info = await _vkApi.Photo.GetMessagesUploadServerAsync(0);
            var uploader = new WebClient();
            var fileName = "memory/image" + url.Substring(url.LastIndexOf('.'));
            uploader.DownloadFile(url, fileName);
            var uploadResponseInBytes = uploader.UploadFile(info.UploadUrl, fileName);
            var strResponse = Encoding.UTF8.GetString(uploadResponseInBytes);
            var photo = _vkApi.Photo.SaveMessagesPhoto(strResponse);
            var attachments = new List<MediaAttachment> { photo.First() };

            return await SendMessage(null, parent, attachments);
        }
    }
}
