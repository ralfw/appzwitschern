using System.IO;

namespace az.twitter
{
    public static class TokenRepository
    {
        public static Token LoadFrom(string filename)
        {
            using(var sr = new StreamReader(filename))
            {
                return new Token {
                    Key = sr.ReadLine(), 
                    Secret = sr.ReadLine()
                };
            }
        }

        public static void SaveTo(string filename, Token token)
        {
            using(var sw = new StreamWriter(filename, false))
            {
                sw.WriteLine(token.Key);
                sw.WriteLine(token.Secret);
            }
        }
    }


    public class Token
    {
        public string Key { get; set; }
        public string Secret { get; set; }
    }
}