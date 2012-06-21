using az.contracts;
using az.security;

namespace az.twitterapi
{
    public class TwitterOperations
    {
        public Versandauftrag Versandauftrag_um_access_token_erweitern(Versandauftrag versandauftrag) {
            versandauftrag.Credentials = TokenRepository.LoadFrom("twitter.access.token.txt");
            return versandauftrag;
        }

        public string Versenden(Versandauftrag versandauftrag) {
            var twitter = new Twitter(TokenRepository.LoadFrom("twitter.consumer.token.txt"));

            twitter.SendMessage(versandauftrag.Text, versandauftrag.Credentials.Key, versandauftrag.Credentials.Secret);

            return versandauftrag.Id;
        }
    }
}