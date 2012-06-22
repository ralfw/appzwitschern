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
        public string Shorten(string longUrl)
        {
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
