using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CsQuery;

namespace VkBot
{
    public class MemeCommand : BaseCommand
    {
        public MemeCommand()
        {
            Filters = new [] {"мем", "прикол", "meme" };
        }

        public override string GetInfo()
        {
            return "Сапфир мем\nЯ скину прикол с сайта anekdot.ru";
        }

        public override string GetResponse(List<string> src)
        {
            var urlAddress = "https://www.anekdot.ru/random/mem/?rank=1";
            var request = (HttpWebRequest)WebRequest.Create(urlAddress);
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                return "Что-то связи нет";
            
            var receiveStream = response.GetResponseStream();
            var readStream = string.IsNullOrWhiteSpace(response.CharacterSet) ? 
                new StreamReader(receiveStream) : 
                new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

            CQ data = readStream.ReadToEnd();

            response.Close();
            readStream.Close();

            var images = data[".topicbox > .text > img"];
            var url = images.Select(x => x.GetAttribute("src")).FirstOrDefault(y => !string.IsNullOrEmpty(y));
            return url ?? "Попробуй ещё раз";
        }
    }
}
