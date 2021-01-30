using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CsQuery;
using VkBot.Communication;

namespace VkBot
{
    public class MemeCommand : BaseCommand
    {
        public MemeCommand()
        {
            Filters = new[] { "мем", "прикол", "meme" };
        }

        public override string GetInfo()
        {
            return "Сапфир мем\nЯ скину прикол с сайта anekdot.ru";
        }

        public override Response GetResponse(List<string> src, UserStatus _)
        {
            var response = new Response(ResponseType.Text) { Content = "Что-то связи нет" };
            var urlAddress = "https://www.anekdot.ru/random/mem/?rank=1";
            var request = (HttpWebRequest)WebRequest.Create(urlAddress);
            var webResponse = (HttpWebResponse)request.GetResponse();
            if (webResponse.StatusCode != HttpStatusCode.OK)
                return response;

            var receiveStream = webResponse.GetResponseStream();
            var readStream = string.IsNullOrWhiteSpace(webResponse.CharacterSet) ?
                new StreamReader(receiveStream) :
                new StreamReader(receiveStream, Encoding.GetEncoding(webResponse.CharacterSet));

            CQ data = readStream.ReadToEnd();
            webResponse.Close();
            readStream.Close();

            var images = data[".topicbox > .text > img"];
            var url = images.Select(x => x.GetAttribute("src")).FirstOrDefault(y => !string.IsNullOrEmpty(y));
            response.Content = url ?? "Попробуй ещё раз";

            if (!string.IsNullOrEmpty(url))
                response.Type = ResponseType.Image;

            return response;
        }
    }
}
