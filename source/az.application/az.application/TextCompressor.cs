using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace az.application
{
    internal class TextCompressor
    {
        public string[] Extract_Urls(string text)
        {
            var urls = new List<string>();

            var match = Regex.Match(text, "http://[^ ]*");
            while(match.Success)
            {
                urls.Add(match.Groups[0].Value);
                match = match.NextMatch();
            }

            return urls.ToArray();
        }


        public string Replace_Urls(Tuple<string, Tuple<string,string>[]> request)
        {
            var text = request.Item1;
            var urlsPairs = request.Item2;

            foreach(var urlPair in urlsPairs)
                text = text.Replace(urlPair.Item1, urlPair.Item2);

            return text;
        }
    }
}
