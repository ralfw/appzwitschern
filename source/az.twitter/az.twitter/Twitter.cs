using System;
using Twitterizer;

namespace az.twitter
{
    public class Twitter
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;
        private OAuthTokenResponse requestToken;

        public Twitter(string consumerKey, string consumerSecret) {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        public void SendMessage(string message, string accessToken, string accessTokenSecret) {   
            var tokens = new OAuthTokens {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                AccessToken = accessToken,
                AccessTokenSecret = accessTokenSecret
            };
            var status = TwitterStatus.Update(tokens, message);
        
            if (status.Result != RequestResult.Success) {
                throw new Exception(string.Format("Result={0}, Error={1}", status.Result, status.ErrorMessage));
            }
        }

        public string GetAuthorizationUrl() {
            requestToken = OAuthUtility.GetRequestToken(consumerKey, consumerSecret, "oob");
            var authorizationUri = OAuthUtility.BuildAuthorizationUri(requestToken.Token);
            return authorizationUri.AbsoluteUri;
        }

        public AccessToken GetAccessToken(string pin) {
            var accessToken = OAuthUtility.GetAccessToken(consumerKey, consumerSecret, requestToken.Token, pin);
            return new AccessToken {
                AccessKey = accessToken.Token,
                AccessSecret = accessToken.TokenSecret
            };
        }
    }

    public class AccessToken
    {
        public string AccessKey { get; set; }

        public string AccessSecret { get; set; }
    }
}
