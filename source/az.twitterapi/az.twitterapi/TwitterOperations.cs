﻿using az.security;
using az.twitterapi;

namespace az.twitterapi
{
    public class TwitterOperations
    {
        public Versandauftrag Versandauftrag_schnüren(string text) {
            return new Versandauftrag(text, TokenRepository.LoadFrom("twitter.access.token.txt"));
        }

        public string Versenden(Versandauftrag versandauftrag) {
            var twitter = new Twitter(TokenRepository.LoadFrom("twitter.consumer.token.txt"));

            twitter.SendMessage(versandauftrag.Text, versandauftrag.Credentials.Key, versandauftrag.Credentials.Secret);

            return "Versendet!";
        }
    }
}