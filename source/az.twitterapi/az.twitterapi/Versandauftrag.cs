using az.twitterapi;

namespace az.twitterapi
{
    public class Versandauftrag
    {
        public string Text { get; set; }
        public Token Credentials { get; set; }

        public Versandauftrag(string text, Token credentials) {
            Text = text;
            Credentials = credentials;
        }
    }
}