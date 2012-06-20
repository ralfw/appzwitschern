using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using az.twitter;

namespace az.application
{
    public class Versandauftrag
    {
        public string Text { get; set; }
        public Token Credentials { get; set; }

        public Versandauftrag(string text, Token credentials)
        {
            Text = text;
            Credentials = credentials;
        }
    }


    public class TwitterOperations
    {
        public Versandauftrag Versandauftrag_schnüren(string text)
        {
            return new Versandauftrag(text, TokenRepository.LoadFrom("twitter.access.token.txt"));
        }

        public string Versenden(Versandauftrag versandauftrag)
        {
            var twitter = new Twitter(TokenRepository.LoadFrom("twitter.consumer.token.txt"));

            twitter.SendMessage(versandauftrag.Text, versandauftrag.Credentials.Key, versandauftrag.Credentials.Secret);

            return "Versendet!";
        }
    }
}
