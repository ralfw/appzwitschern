using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace az.tinyurlapi
{
    public class TinyUrlOperations
    {
        public Tuple<string,string>[] ShortenMany(IEnumerable<string> longUrls)
        {
            return longUrls.Select(longUrl =>
                                       {
                                           var shortUrl = Shorten(longUrl);
                                           return new Tuple<string, string>(longUrl, shortUrl);
                                       })
                           .ToArray();
        }


        public string Shorten(string longUrl)
        {
            if (longUrl.ToLower().IndexOf("tinyurl.com") >= 0) return longUrl;

            var shortenRequest = string.Format(@"http://tinyurl.com/api-create.php?url={0}", longUrl);
            var request = WebRequest.Create(shortenRequest);
            var response = request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
