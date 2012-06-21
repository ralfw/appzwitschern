using System;
using Twitterizer;
using az.security;

namespace az.twitterapi
{
    public class Twitter
    {
        private readonly Token consumerToken;
        private OAuthTokenResponse requestToken;

        public Twitter(Token consumerToken)
        {
            this.consumerToken = consumerToken;
        }

        public void SendMessage(string message, string accessToken, string accessTokenSecret) {   
            var tokens = new OAuthTokens {
                ConsumerKey = consumerToken.Key,
                ConsumerSecret = consumerToken.Secret,
                AccessToken = accessToken,
                AccessTokenSecret = accessTokenSecret
            };
            var status = TwitterStatus.Update(tokens, message);
        
            if (status.Result != RequestResult.Success) {
                throw new Exception(string.Format("Result={0}, Error={1}", status.Result, status.ErrorMessage));
            }
        }

        public string GetAuthorizationUrl() {
            requestToken = OAuthUtility.GetRequestToken(consumerToken.Key, consumerToken.Secret, "oob");
            var authorizationUri = OAuthUtility.BuildAuthorizationUri(requestToken.Token);
            return authorizationUri.AbsoluteUri;
        }

        public Token GetAccessToken(string pin) {
            var accessToken = OAuthUtility.GetAccessToken(consumerToken.Key, consumerToken.Secret, requestToken.Token, pin);
            return new Token {
                Key = accessToken.Token,
                Secret = accessToken.TokenSecret
            };
        }
    }
}
